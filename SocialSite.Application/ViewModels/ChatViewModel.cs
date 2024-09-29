using SocialSite.Domain.Models;

namespace SocialSite.Application.ViewModels;

public class ChatViewModel
{
    public string Name { get; set; } = string.Empty;
    public IEnumerable<Message> Messages { get; set; } = [];
}
