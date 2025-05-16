using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Helpers;

public interface IFileHelperService
{
    public string DefaultFileForDb { get; set; }
    public string DefaultFile { get; set; }
    public string DefaultFileFullPath { get; set; }
    public string UploadDirectoryPath { get; set; }
    public string FileName { get; set; }
    public bool AddDefaultFile { get; set; }
    public List<string> AllowedContents { get; set; }
    public string MyGetCurrentDirectory { get; set; }

    (string, string, bool) CreatePath(IFormFile file);
    (string, bool) AddFile(IFormFile file);
    bool DeleteFile(string imagePath, string? directory = "", string? fullFilePath = "");
    (string, bool) UpdateFile(IFormFile file, string oldFilePath);
    void BindValues(
       string defaultFileForDb = @"\ProductImages\defaultFile.jpg",
       string? defaultFileFullPath = @"\wwwroot\ProductImages\defaultFile.jpg",
       string defaultFileName = "defaultFile.jpg",
       string? uploadDirectoryPath = @"\wwwroot\ProductImages\",
       string? fileName = @"\ProductImages\",
       bool addDefaultFile = false,
       List<string>? allowedContents = null);
}
