using USVStudDocs.Models;

namespace USVStudDocs.BLL.Services.FileService;

public interface IFileService
{
    FileStorage Get(int id);

    FileStorage Create(FileStorage fileStorage);
    
    void Delete(int id);
}