﻿namespace SocialSite.Application.Dtos.Chats;

public sealed class ChatInfoDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsDirect { get; set; }
}
