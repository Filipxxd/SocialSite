using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialSite.API.Hubs.Interfaces;
using SocialSite.Application.AppServices;
using SocialSite.Application.Constants;
using SocialSite.Core.Constants;

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
		var user = Context.User;
	}
	
	public async Task JoinChatGroup(int chatId)
	{
		var userIdRaw = Context.User?.FindFirstValue(AppClaimTypes.UserId) 
		                ?? throw new ArgumentNullException(nameof(AppClaimTypes.UserId), "UserId claim not found");
		var userId = int.Parse(userIdRaw);

		var isInChat = await _chatAppService.IsUserInChatAsync(userId, chatId);
        
		if (!isInChat) throw new HubException("You're not authorized to join this chat.");

		await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
	}
    
	public async Task LeaveChatGroup(int chatId)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId.ToString());
	}
    
	public async Task NotifyNewMessage(int chatId, int messageId)
	{
		await Clients.Group(chatId.ToString()).ReceiveNewMessage(chatId.ToString(), messageId.ToString());
	}
}