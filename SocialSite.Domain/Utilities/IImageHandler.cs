namespace SocialSite.Domain.Utilities;

public interface IImageHandler
{
    Task<string> SaveImageAsync(byte[] imageBytes, string originalFileName, bool isPublic = true);
    Task<byte[]> LoadImageAsync(string relativePath);
    bool DeleteImage(string relativePath);
}
