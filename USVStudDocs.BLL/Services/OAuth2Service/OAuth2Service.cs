using System.IdentityModel.Tokens.Jwt;
using JWT.Algorithms;
using JWT.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.DAL;
using USVStudDocs.Entities.Authentication;
using USVStudDocs.Models;
using RestSharp;
using Serilog;
using UMSA.StepTest.BLL.Configuration;

namespace USVStudDocs.BLL.Services.OAuth2Service;

public class OAuth2Service : IOAuth2Service
{
    private readonly MainContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly RestClient _googleRestClient;
    private readonly RestClient _googlePersonRestClient;

    public OAuth2Service(MainContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
        _googleRestClient = new RestClient("https://oauth2.googleapis.com/token");
        _googlePersonRestClient = new RestClient("https://people.googleapis.com/v1/people/me");
    }

    public AuthTokenResponse? AuthorizeCode(string code)
    {
        var token = AuthorizeCodeRequest(code);
        
        if (token?.IdToken == null)
        {
            throw new ValidationException("Cannot validate token");
        }
        
        var stream = token.IdToken;  
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(stream);
        var tokenS = jsonToken as JwtSecurityToken;

        if (tokenS == null)
        {
            throw new ValidationException("Cannot read token");
        }

        var sub = tokenS.Claims.FirstOrDefault(c => c.Type == "sub");
        var email = tokenS.Claims.FirstOrDefault(c => c.Type == "email");
        var firstName = tokenS.Claims.FirstOrDefault(c => c.Type == "given_name");
        var lastName = tokenS.Claims.FirstOrDefault(c => c.Type == "family_name");
        
        // Profile info request
        var profilePhotoRequest = new RestRequest("", Method.Get);
        profilePhotoRequest.AddHeader("Authorization", $"Bearer {token.AccessToken}");
        profilePhotoRequest.AddQueryParameter("personFields", "photos");
        var profileResponse = _googlePersonRestClient.Execute<ProfilePhotoResponse>(profilePhotoRequest);
        var profile = profileResponse.Data;

        var userEntity = _context.User
            .Include(u => u.Role)
            .FirstOrDefault(us => email != null && us.Email == email.Value);

        if (userEntity == null)
        {
            throw new ValidationException("Associated user is not found in the system");
        }

        _context.SaveChanges();

        // Log.ForContext<OAuth2Service>().Warning("New user was created! {UserEntityEmail}", userEntity.Email);
        
        // Generate token after creating user
        token.IdToken = GenerateJwtToken(userEntity);
        
        return token;
    }

    private AuthTokenResponse? AuthorizeCodeRequest(string code)
    {
        var clientId = Environment.GetEnvironmentVariable("GoogleClientId") ?? "";
        var clientSecret = Environment.GetEnvironmentVariable("GoogleClientSecret") ?? "";
        var redirectUri = Environment.GetEnvironmentVariable("GoogleRedirectUri") ?? "";
        
        var codeExchangeRequest = new RestRequest("", Method.Post);

        codeExchangeRequest.AddQueryParameter("code", code);
        codeExchangeRequest.AddQueryParameter("client_id", clientId);
        codeExchangeRequest.AddQueryParameter("client_secret", clientSecret);
        codeExchangeRequest.AddQueryParameter("redirect_uri", redirectUri);
        codeExchangeRequest.AddQueryParameter("grant_type", "authorization_code");

        var token = new AuthTokenResponse();
        try
        {
            var tokenResponse = _googleRestClient.Execute<AuthTokenResponse>(codeExchangeRequest);
            token = tokenResponse.Data;
        }
        catch (Exception e)
        {
            Log.ForContext<OAuth2Service>().Error("Something went wrong with token authentication on Google");
        }

        return token;
    }
    
    private string GenerateJwtToken(UserEntity userEntity)
    {
        var jwtSecretKey = Environment.GetEnvironmentVariable("JwtSecretKey") ?? _jwtSettings.SecretKey;

        var tokenBuilder = new JwtBuilder()
            .WithAlgorithm(new HMACSHA256Algorithm())
            .WithSecret(jwtSecretKey)
            .AddClaim(JwtRegisteredClaimNames.Exp,
                DateTimeOffset.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes).ToUnixTimeSeconds())
            .AddClaim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer)
            .AddClaim(JwtRegisteredClaimNames.Aud, _jwtSettings.Audience)
            .AddClaim(JwtRegisteredClaimNames.Sub, userEntity.Id)
            .AddClaim(JwtRegisteredClaimNames.UniqueName, userEntity.Username)
            .AddClaim(JwtRegisteredClaimNames.Email, userEntity.Email)
            .AddClaim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.AddMilliseconds(1).ToUnixTimeSeconds());
        
        string role = userEntity.Role.Name;
        tokenBuilder.AddClaim("role", role);
            
        return tokenBuilder.Encode();
    }
}