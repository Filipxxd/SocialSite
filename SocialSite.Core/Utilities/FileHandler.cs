using SocialSite.Core.Constants;
using SocialSite.Core.Exceptions;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Utilities;

 public sealed class FileHandler : IFileHandler
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly string _currentDirectory;
    private readonly string _publicFolderPath;
    private readonly string _privateFolderPath;

    public FileHandler(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        _currentDirectory = Directory.GetCurrentDirectory();
        _publicFolderPath = Path.Combine(_currentDirectory, FileConstants.WwwRoot, FileConstants.PublicPath);
        _privateFolderPath = Path.Combine(_currentDirectory, FileConstants.PrivatePath);

        Directory.CreateDirectory(_publicFolderPath);
        Directory.CreateDirectory(_privateFolderPath);
    }

    public async Task<string> SaveAsync(byte[] bytes, string fileName, bool isPublic = true)
    {
        if (bytes.Length == 0)
            throw new NotValidException("File byte array is invalid.");

        if (bytes.Length > FileConstants.MaxFileSizeBytes)
	        throw new NotValidException($"File size exceeds the maximum allowed size of {FileConstants.MaxFileSizeBytes / (1024 * 1024)} MB.");

        var extension = Path.GetExtension(fileName);
        
        if (!FileConstants.ValidExtensions.Contains(extension))
	        throw new NotValidException($"Invalid file extension ({extension}).");
        
        var currentDate = _dateTimeProvider.GetDateTime();
		var yearFolder = currentDate.ToString("yyyy");
		var monthFolder = currentDate.ToString("MM");
		var dayFolder = currentDate.ToString("dd");
		var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
		
		var savePath = isPublic ? _publicFolderPath : _privateFolderPath;
		var folderPath = Path.Combine(savePath, yearFolder, monthFolder, dayFolder);
		var filePath = Path.Combine(folderPath, uniqueFileName);
		
		Directory.CreateDirectory(folderPath);
		await File.WriteAllBytesAsync(filePath, bytes);
		
		return isPublic 
		   ? Path.Combine(Path.PathSeparator.ToString() ,FileConstants.PublicPath, yearFolder, monthFolder, dayFolder, uniqueFileName)
		   : Path.Combine(FileConstants.PrivatePath, yearFolder, monthFolder, dayFolder, uniqueFileName);
    }

    public async Task<byte[]> GetAsync(string relativePath)
    {
	    if (relativePath.Contains(FileConstants.PublicPath))
		    relativePath = FileConstants.WwwRoot + relativePath;
	    
        var filePath = Path.Combine(_currentDirectory, relativePath);
        if (!File.Exists(filePath))
            throw new NotFoundException("File not found.");

        return await File.ReadAllBytesAsync(filePath);
    }

    public bool Delete(string relativePath)
    {
	    if (relativePath.Contains(FileConstants.PublicPath))
		    relativePath = FileConstants.WwwRoot + relativePath;

        var filePath = Path.Combine(_currentDirectory, relativePath);
        if (!File.Exists(filePath)) return false;
        File.Delete(filePath);
        return true;
    }
}
