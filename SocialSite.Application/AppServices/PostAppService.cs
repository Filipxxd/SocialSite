using SocialSite.Application.Dtos;
using SocialSite.Application.Dtos.Images;
using SocialSite.Application.Dtos.Posts;
using SocialSite.Application.Mappers;
using SocialSite.Domain.Filters;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Application.AppServices;

public sealed class PostAppService
{
	private readonly IPostService _postService;
	private readonly IFileHandler _fileHandler;
	
	public PostAppService(IPostService postService, IFileHandler fileHandler)
	{
		_postService = postService;
		_fileHandler = fileHandler;
	}
	
	public async Task<PagedDto<PostDto>> GetAllPostsAsync(PostFilter filter, int currentUserId)
	{
		var posts = await _postService.GetAllPostsAsync(filter, currentUserId);
		var totalRecords = await _postService.GetRecordsCountAsync(filter, currentUserId);
		
		var postsDto = new List<PostDto>();
    
		foreach (var post in posts)
		{
			var postDto = post.Map(currentUserId);

			foreach (var image in post.Images)
				postDto.Images.Add(new ImageDto
				{
					Name = image.Name,
					Base64 = Convert.ToBase64String(await _fileHandler.GetAsync(image.Path))
				});

			postsDto.Add(postDto);
		}

		return new PagedDto<PostDto>(postsDto, totalRecords);
	}

	public async Task CreatePostAsync(CreatePostDto dto, int currentUserId)
	{
		var post = dto.Map(currentUserId);
		
		foreach (var imageDto in dto.Images)
			post.Images.Add(new()
			{
				Name = imageDto.Name,
				Path = await _fileHandler.SaveAsync(Convert.FromBase64String(imageDto.Base64), imageDto.Name, false)
			});

		await _postService.CreatePostAsync(post);
	}
	
	public async Task UpdatePostAsync(CreatePostDto dto, int currentUserId)
	{
		throw new NotImplementedException();
	}
	
	public async Task DeletePostAsync(int postId, int currentUserId)
	{
		await _postService.DeletePostAsync(postId, currentUserId);
	}
}