using Microsoft.EntityFrameworkCore;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;

namespace SocialSite.Data.EF.Extensions;

public static class ImageQueryableExtensions
{
    public static IQueryable<User> IncludeProfileImage(this IQueryable<User> query)
        => query.Include(u => u.ProfileImage).Where(u => u.ProfileImage!.Entity == EntityType.Profile);

    public static IQueryable<Chat> IncludeChatImage(this IQueryable<Chat> query)
        => query.Include(u => u.Image).Where(u => u.Image!.Entity == EntityType.GroupChat);

    public static IQueryable<Post> IncludePostImages(this IQueryable<Post> query)
        => query.Include(p => p.Images.Where(i => i.Entity == EntityType.Post));

    public static IQueryable<Message> IncludeMessageImages(this IQueryable<Message> query)
        => query.Include(p => p.Images.Where(i => i.Entity == EntityType.Message));
}
