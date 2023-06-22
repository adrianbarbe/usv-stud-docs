using System.IdentityModel.Tokens.Jwt;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using USVStudDocs.BLL.Authorization;
using USVStudDocs.BLL.Mappers;
using USVStudDocs.BLL.Mappers.Storage;
using USVStudDocs.BLL.Services.AuthorizationService;
using USVStudDocs.BLL.Services.AwsMinioClient;
using USVStudDocs.BLL.Services.FileService;
using USVStudDocs.BLL.Services.OAuth2Service;
using USVStudDocs.BLL.Validators;
using USVStudDocs.DAL;
using USVStudDocs.DAL.Helpers;
using USVStudDocs.Entities.Storage;
using USVStudDocs.Models;
using USVStudDocs.Models.Configuration;
using USVStudDocs.Models.Constants;
using USVStudDocs.Web;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using TelegramSink;
using UMSA.StepTest.BLL.Configuration;
using USVStudDocs.BLL.Mappers.Admin;
using USVStudDocs.BLL.Mappers.Secretary;
using USVStudDocs.BLL.Mappers.Student;
using USVStudDocs.BLL.Services.CommonNumberService;
using USVStudDocs.BLL.Services.EmailService;
using USVStudDocs.BLL.Services.FacultyPersonService;
using USVStudDocs.BLL.Services.FacultyService;
using USVStudDocs.BLL.Services.NavigationService;
using USVStudDocs.BLL.Services.ProgramStudyService;
using USVStudDocs.BLL.Services.SecretaryCertificateService;
using USVStudDocs.BLL.Services.SemesterService;
using USVStudDocs.BLL.Services.SettingsService;
using USVStudDocs.BLL.Services.StudentCertificateService;
using USVStudDocs.BLL.Services.StudentService;
using USVStudDocs.BLL.Services.StudentsImportService;
using USVStudDocs.Entities;
using USVStudDocs.Models.Admin;
using USVStudDocs.Models.Secretary;
using USVStudDocs.Models.Student;
using IAuthorizationService = USVStudDocs.BLL.Services.AuthorizationService.IAuthorizationService;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables(prefix: "USV_");

var corsAllowOrigins = "corsAllowOrigins";

builder.Services.AddCors(options =>
{
    var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();
    var origins = corsSettings.AllowedOrigins.Split(";");

    options.AddPolicy(name: corsAllowOrigins,
        corsPolicyBuilder =>
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (env == "Development")
            {
                corsPolicyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }
            else
            {
                corsPolicyBuilder
                    // .AllowAnyOrigin()
                    .WithOrigins(origins)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }
        });
});

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", false, true)
    .AddEnvironmentVariables();


builder.Host
    .UseSerilog((ctx, loggerConfiguration) => 
        loggerConfiguration
            .MinimumLevel.Information()
            .WriteTo.Console(theme: AnsiConsoleTheme.Literate, restrictedToMinimumLevel: LogEventLevel.Information)
            .WriteTo.TeleSink(
                telegramApiKey: builder.Configuration["SerilogTelegram:ApiKey"],
                telegramChatId: builder.Configuration["SerilogTelegram:ChatId"],
                minimumLevel: LogEventLevel.Error
            )
    );

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddHostedService<StartupCheckAdmin>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var expiredException = context.Exception is SecurityTokenExpiredException;

                if (expiredException)
                {
                    context.Response.Headers.Add(HeaderNames.Expires, context.Properties.ExpiresUtc.ToString());
                }

                return Task.CompletedTask;
            },
        };
        o.SaveToken = true;
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "sub",
            RoleClaimType = "role",
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
    });

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy(Policies.Student, policy => policy.Requirements.Add(new RoleRequirement("student")));
    o.AddPolicy(Policies.Secretary, policy => policy.Requirements.Add(new RoleRequirement("secretary")));
    o.AddPolicy(Policies.Admin, policy => policy.Requirements.Add(new RoleRequirement("admin")));
});

builder.Services.AddDbContext<MainContext>(options =>
    {
        var connectionString = Environment.GetEnvironmentVariable("DbConnectionString") ??
                               builder.Configuration.GetSection("DbConnectionString").Get<string>();

        if (connectionString == string.Empty)
        {
            throw new Exception("No DbConnectionString ENV available");
        }

        options.UseNpgsql(connectionString);
    }
);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IAuthorizationHandler, RoleHandler>();


// Configuration
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<AwsMinioSettings>(builder.Configuration.GetSection("AwsMinioSettings"));

// Mappers
builder.Services.AddSingleton<IMapper<FileEntity, FileStorage>, FileMapper>();
builder.Services.AddSingleton<IMapper<ProgramStudyEntity, ProgramStudy>, ProgramStudyMapper>();
builder.Services.AddSingleton<IMapper<YearSemesterEntity, Semester>, SemesterMapper>();
builder.Services.AddSingleton<IMapper<FacultyEntity, Faculty>, FacultyMapper>();
builder.Services.AddSingleton<IMapper<FacultyPersonEntity, FacultyPerson>, FacultyPersonMapper>();
builder.Services.AddSingleton<IMapper<StudentEntity, Student>, StudentMapper>();
builder.Services.AddSingleton<IMapper<CertificateEntity, StudentCertificateListItem>, StudentCertificateListItemMapper>();
builder.Services.AddSingleton<IMapper<CertificateEntity, SecretaryCertificateListItem>, SecretaryCertificateListItemMapper>();

// Services
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IAuthorizationDAHelper, AuthorizationDAHelper>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IOAuth2Service, OAuth2Service>();
builder.Services.AddScoped<IAwsMinioClient, AswMinioClient>();
builder.Services.AddScoped<IProgramStudyService, ProgramStudyService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<IFacultiesService, FacultiesService>();
builder.Services.AddScoped<IFacultyPersonService, FacultyPersonService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<INavigationService, NavigationService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentsImportService, StudentsImportService>();
builder.Services.AddScoped<IStudentCertificateService, StudentCertificateService>();
builder.Services.AddScoped<ISecretaryCertificateService, SecretaryCertificateService>();
builder.Services.AddScoped<ICommonNumberService, CommonNumberService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<StartupCheckAdmin>();

// Validators
builder.Services.AddTransient<IValidator<FileStorage>, FileValidator>();

builder.Services.AddControllers(options => { options.Filters.Add(typeof(ExceptionHandlingFilter)); });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

app.UseCors(corsAllowOrigins);

app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});


app.UseAuthorization();

app.MapControllers();

app.Run();