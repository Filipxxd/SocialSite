namespace SocialSite.Core.Exceptions;

public sealed class NotValidException(string message) : Exception(message) { }
