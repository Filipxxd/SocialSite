namespace SocialSite.Application.Constants;

internal static class ValidationConstants
{
    public const string CzechAlphabetRegex = @"^[a-zA-ZáčďéěíňóřšťúůýžÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ\s]*$";
    public const string AlphaNumericRegex = "^[a-zA-Z0-9]*$";
    public const string PasswordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]*$";
}