using Microsoft.AspNetCore.Http;

namespace Core.Utilities.Helpers;
public class FileHelperManager : IFileHelperService
{
    public string DefaultFileForDb { get; set; }
    public string DefaultFile { get; set; }
    public string DefaultFileFullPath { get; set; }
    public string UploadDirectoryPath { get; set; }
    public string FileName { get; set; }
    public bool AddDefaultFile { get; set; }
    public List<string> AllowedContents { get; set; }
    public string MyGetCurrentDirectory { get; set; } = Directory.GetCurrentDirectory();
    public void BindValues(
        string DefaultFileForDb = @"\ProductImages\defaultFile.jpg",
        string? defaultFileFullPath = @"\wwwroot\ProductImages\defaultFile.jpg",
        string defaultFileName = "defaultFile.jpg",
        string? uploadDirectoryPath = @"\wwwroot\ProductImages\",
        string? fileName = @"\ProductImages\",
        bool addDefaultFile = false,
        List<string>? allowedContents = null)
    {
        DefaultFileForDb = DefaultFileForDb;
        DefaultFile = defaultFileName;
        DefaultFileFullPath = MyGetCurrentDirectory + defaultFileName;
        UploadDirectoryPath = MyGetCurrentDirectory + uploadDirectoryPath;
        FileName = fileName;
        AddDefaultFile = addDefaultFile;
        AllowedContents = allowedContents != null ? allowedContents : new List<string> { "image/jpeg", "image/jpg", "image/png", "image/webp", "image/gif", "image/svg", "image/webp" };
    }

    public (string, string, bool) CreatePath(IFormFile file)
    {
        if (!File.Exists(UploadDirectoryPath + "\\" + file.FileName))
        {
            return (UploadDirectoryPath + "\\" + file.FileName, FileName + file.FileName, true);
        };
        if (AllowedContents.Contains(file.ContentType))
        {
            string directory = UploadDirectoryPath;
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            string newFilePath = directory + "\\" + file.FileName;

            int i = 1;
            string tempFileName = file.FileName;
            while (File.Exists(newFilePath))
            {
                if (i == 10)
                {
                    i += new Random().Next() * 12;
                }
                tempFileName = string.Format("{0}({1})", fileName, i++);
                newFilePath = Path.Combine(directory, tempFileName + extension);
            }
            return (newFilePath, FileName + tempFileName + extension, true);
        }
        return ("", "", false);
    }
    public (string, bool) AddFile(IFormFile file)
    {
        if (!Directory.Exists(UploadDirectoryPath))
        {
            Directory.CreateDirectory(UploadDirectoryPath);
        }
        (string, string, bool) result = ("", "", false);
        try
        {
            if (file == null)
            {
                if (AddDefaultFile)
                {
                    return (DefaultFileForDb, true);
                }
                return ("", false);
            }
            else
            {
                result = CreatePath(file);
                if (result.Item3 == false)
                {
                    return ("", false);
                }
                string sourcePath = Path.GetTempFileName();
                using (FileStream stream = new FileStream(sourcePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                File.Move(sourcePath, result.Item1);
                return (result.Item2, true);
            }
        }
        catch (Exception exception)
        {
            return (exception.Message, false);
        }
    }
    /// <summary>
    /// fullFile path varsa direk sonu siler ex:(C:Users/HomeUser/Desktop/water.jpg),
    /// directory varsa directory alıp sonuna imagePath'i ekler ex: (Directory+water.jpg),
    /// sadece imagePath varsa otomatik directory'ı alır.
    /// </summary>
    /// <param name="imagePath"></param>
    /// <param name="directory"></param>
    /// <param name="fullFilePath"></param>
    /// <returns>bool</returns>
    public bool DeleteFile(string? imagePath, string? directory = "", string? fullFilePath = "")
    {
        try
        {
            if (!string.IsNullOrEmpty(fullFilePath))
            {
                File.Delete(fullFilePath);
            }
            else if (!string.IsNullOrEmpty(directory) && string.IsNullOrEmpty(imagePath))
            {
                File.Delete(directory + imagePath);
            }
            else
            {
                directory = Directory.GetCurrentDirectory();
                File.Delete(directory + imagePath);
            }
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    public (string, bool) UpdateFile(IFormFile file, string oldFilePath)
    {
        (string, string, bool) result = ("", "", false);
        try
        {
            if (file == null)
            {
                File.Delete(oldFilePath);
                return (DefaultFileFullPath, true);
            }
            else
            {
                result = CreatePath(file);
                if (result.Item3 == false)
                {
                    return ("", false);
                }
                string sourcePath = Path.GetTempFileName();
                using (FileStream stream = new FileStream(sourcePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                File.Move(sourcePath, result.Item1);
                File.Delete(oldFilePath);
                return (result.Item1, true);
            }
        }
        catch (Exception exception)
        {
            return (exception.Message, false);
        }
    }
}