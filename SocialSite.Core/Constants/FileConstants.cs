namespace SocialSite.Core.Constants;

public static class FileConstants
{
	public static readonly string[] ValidExtensions = [".jpg", ".jpeg", ".png", ".webp"];
	public const int MaxFileSizeBytes = 1 * 1024 * 1024; // 1MB
	
	public const string WwwRoot = "wwwroot";
    public const string PublicPath = "public_files";
    public const string PrivatePath = "private_files";
}