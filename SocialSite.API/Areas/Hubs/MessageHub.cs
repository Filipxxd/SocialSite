using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialSite.API.Hubs.Interfaces;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;

namespace SocialSite.API.Areas.Hubs;

[Area("hubs")]
[Route("[area]/messages")]
[Authorize(Policy = AuthPolicies.RegularUsers)]
public sealed class MessageHub : Hub<IMessageClient>
{
	private readonly ChatAppService _chatAppService;

	public MessageHub(ChatAppService chatAppService)
	{
		_chatAppService = chatAppService;
	}

	public override async Task OnConnectedAsync()
	{
		throw new NotImplementedException();
	}

	public async Task JoinChatGroup(int chatId)
	{
		throw new NotImplementedException();
	}

	public async Task LeaveChatGroup(int chatId)
	{
		throw new NotImplementedException();
	}

	public async Task NotifyNewMessage(int chatId, int messageId)
	{
		throw new NotImplementedException();
	}
}