namespace SocialSite.Domain.Utilities;

public interface IFileHandler
{
    Task<string> SaveAsync(byte[] bytes, string fileName, bool isPublic = true);
    Task<byte[]> GetAsync(string relativePath);
    bool Delete(string relativePath);
}
