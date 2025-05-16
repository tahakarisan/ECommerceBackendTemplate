using Microsoft.AspNetCore.Http;

namespace Business.Utilities.File
{
    public interface IProductImageUploadService
    {
        public string DefaultImagePath { get; set; }
        (string, bool) AddImage(IFormFile formFile);
        bool DeleteImage(string imagePath, string? directory = "", string? fullFilePath = "");
        (string, bool) UpdateImage(IFormFile file, string oldFilePath);

    }
}
