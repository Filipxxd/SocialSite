using SocialSite.Core.Constants;
using SocialSite.Core.Exceptions;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Utilities;

public class ImageHandler : IImageHandler
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly string _publicImagesPath;
    private readonly string _privateImagesPath;

    public ImageHandler(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _publicImagesPath = Path.Combine(Directory.GetCurrentDirectory(), FileConstants.PublicImagesPath);
        _privateImagesPath = Path.Combine(Directory.GetCurrentDirectory(), FileConstants.PrivateImagesPath);

        Directory.CreateDirectory(_publicImagesPath);
        Directory.CreateDirectory(_privateImagesPath);
    }

    public async Task<string> SaveImageAsync(byte[] imageBytes, string originalFileName, bool isPublic = true)
    {
        if (imageBytes is null || imageBytes.Length == 0)
            throw new NotValidException("Image byte array is invalid.");

        var currentDate = _dateTimeProvider.GetDateTime();
        var monthFolder = currentDate.ToString("MM");
        var dayFolder = currentDate.ToString("dd");
        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
        var savePath = isPublic ? _publicImagesPath : _privateImagesPath;
        var folderPath = Path.Combine(savePath, monthFolder, dayFolder);
        var filePath = Path.Combine(folderPath, uniqueFileName);

        Directory.CreateDirectory(folderPath);
        await File.WriteAllBytesAsync(filePath, imageBytes);

        return isPublic 
            ? Path.Combine(FileConstants.PublicImagesPath, monthFolder, dayFolder, uniqueFileName)
            : Path.Combine(FileConstants.PrivateImagesPath, monthFolder, dayFolder, uniqueFileName);
    }

    public async Task<byte[]> LoadImageAsync(string relativePath)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        if (!File.Exists(filePath))
            throw new NotFoundException("Image not found.");

        return await File.ReadAllBytesAsync(filePath);
    }

    public bool DeleteImage(string relativePath)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
}
