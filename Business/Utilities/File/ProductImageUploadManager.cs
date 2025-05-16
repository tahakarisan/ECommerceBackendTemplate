using Core.Utilities.Helpers;
using Microsoft.AspNetCore.Http;

namespace Business.Utilities.File
{
    public class ProductImageUploadManager : IProductImageUploadService
    {
        private IFileHelperService _fileHelperService;
        public string DefaultImagePath { get; set; }

        public ProductImageUploadManager(IFileHelperService fileHelperService)
        {
            _fileHelperService = fileHelperService;
            _fileHelperService.BindValues(
                defaultFileForDb: @"\ProductImages\defaultFile.jpg",
                defaultFileFullPath: @"\wwwroot\ProductImages\defaultFile.jpg",
                defaultFileName: "defaultFile.jpg",
                uploadDirectoryPath: @"\wwwroot\ProductImages\",
                fileName: @"\ProductImages\",
                addDefaultFile: true,
                allowedContents: new List<string> { "image/jpeg", "image/jpg", "image/png", "image/webp", "image/gif", "image/svg", "image/webp" }
                );
            DefaultImagePath = Directory.GetCurrentDirectory() + @"\wwwroot\ProductImages\defaultFile.jpg";
        }

        public (string, bool) AddImage(IFormFile formFile)
        {
            return _fileHelperService.AddFile(formFile);
        }
        public bool DeleteImage(string imagePath, string? directory = "", string? fullFilePath = "")
        {
            return _fileHelperService.DeleteFile(imagePath, directory, fullFilePath);
        }
        public (string, bool) UpdateImage(IFormFile file, string oldFilePath)
        {
            return _fileHelperService.UpdateFile(file, oldFilePath);
        }
    }
}
