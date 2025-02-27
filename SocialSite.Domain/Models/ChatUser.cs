﻿namespace SocialSite.Domain.Models;

public class ChatUser
{
    public int Id { get; set; }

    public int UserId { get; set; }
    public virtual User? User { get; set; }

    public int ChatId { get; set; }
    public virtual Chat? Chat { get; set; }
}
