namespace SocialSite.API.Hubs.Interfaces;

public interface IMessageClient
{
	Task ReceiveNewMessage(string chatId, string messageId);
}