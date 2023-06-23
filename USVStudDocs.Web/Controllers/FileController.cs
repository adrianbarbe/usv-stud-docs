using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using USVStudDocs.BLL.Exceptions;
using USVStudDocs.BLL.Services.AwsMinioClient;
using USVStudDocs.BLL.Services.FileService;
using USVStudDocs.Models;
using USVStudDocs.Models.Constants;

namespace USVStudDocs.Web.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IAwsMinioClient _awsMinioClient;
        private readonly IFileService _fileService;

        public FileController(IWebHostEnvironment environment, 
            IAwsMinioClient awsMinioClient,
            IFileService fileService)
        {
            _environment = environment;
            _awsMinioClient = awsMinioClient;
            _fileService = fileService;
        }

        [HttpGet]
        [Route("sample/{type?}")]
        public async Task<ActionResult> GetFile(string? type)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            string filePathConcatenated = Path.Combine(currentDirectory, "assets/usv_students_template_concatenated.csv");
            string filePath = Path.Combine(currentDirectory, "assets/usv_students_template.csv");

            string fileToDownload = filePath;
            
            Console.WriteLine("currentDirectory: " + currentDirectory.ToString());
            Console.WriteLine("filePath: " + filePath.ToString());

            if (type == "concatenated")
            {
                fileToDownload = filePathConcatenated;
            }

            if (System.IO.File.Exists(fileToDownload))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(fileToDownload);

                string contentType = "text/csv";
                
                return File(fileBytes, contentType, Path.GetFileName(fileToDownload));
            }
            else
            {
                throw new NotFoundException("File is not found");
            }
        }
    }
}