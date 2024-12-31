using System.Text.RegularExpressions;

namespace SocialSite.Application.Validators;

internal static class ValidationHelper
{
	public static bool BeValidBase64(string input)
	{
		if (string.IsNullOrEmpty(input) || input.Length % 4 != 0)
			return false;
		
		var base64Regex = new Regex(@"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

		if (!base64Regex.IsMatch(input))
			return false;
		
		try
		{
			_ = Convert.FromBase64String(input);
			return true;
		}
		catch (FormatException)
		{
			return false;
		}
	}
}