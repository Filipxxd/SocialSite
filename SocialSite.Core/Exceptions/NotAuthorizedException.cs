namespace SocialSite.Core.Exceptions;

public sealed class NotAuthorizedException(string message) : Exception(message) { }
