using Microsoft.EntityFrameworkCore;
using SocialSite.Core.Exceptions;
using SocialSite.Data.EF;
using SocialSite.Domain.Filters;
using SocialSite.Domain.Models;
using SocialSite.Domain.Models.Enums;
using SocialSite.Domain.Services;
using SocialSite.Domain.Utilities;

namespace SocialSite.Core.Services;

public sealed class ReportService : IReportService
{
	private readonly DataContext _context;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IFileHandler _fileHandler;
	
	public ReportService(DataContext context, IDateTimeProvider dateTimeProvider, IFileHandler fileHandler)
	{
		_context = context;
		_dateTimeProvider = dateTimeProvider;
		_fileHandler = fileHandler;
	}
	
	public async Task<IEnumerable<Report>> GetAllReportsAsync(ReportsFilter filter)
	{
		return await GetFilteredReports(filter)			
			.Skip(filter.PageSize * (filter.PageNumber - 1))
			.Take(filter.PageSize)
			.OrderBy(r => r.DateCreated)
			.ToListAsync();
	}
	
	public async Task<int> GetReportsCountAsync(ReportsFilter filter)
	{
		return await GetFilteredReports(filter).CountAsync();
	}
	
	public async Task ResolveReportAsync(int reportId, bool accepted)
	{
		var report = await _context.Reports.SingleOrDefaultAsync(x => x.Id == reportId);
		if (report is null)
			throw new NotFoundException("Report not found");

		if (!accepted)
		{
			report.State = ReportState.Declined;
		}
		else
		{
			var post = await _context.Posts
				.Include(p => p.Images)
				.Include(p => p.Comments)
				.SingleOrDefaultAsync(p => p.Id == report.PostId);
			
			if (post is null)
				throw new NotFoundException("Post not found");

			foreach (var image in post.Images) _fileHandler.Delete(image.Path);

			var otherReports = await _context.Reports
				.Where(r => r.PostId == report.PostId && r.Id != reportId)
				.ToListAsync();
			
			_context.Images.RemoveRange(post.Images);
			_context.Comments.RemoveRange(post.Comments);
			_context.Reports.RemoveRange(otherReports);
			_context.Reports.Remove(report);
			_context.Posts.Remove(post);
		}
		
		await _context.SaveChangesAsync();
	}
	
	public async Task CreateReportAsync(Report report)
	{
		var postExists = await _context.Posts.AnyAsync(x => x.Id == report.PostId);
		if(!postExists)
			throw new NotFoundException("Post not found");
		
		var userExists = await _context.Users.AnyAsync(x => x.Id == report.UserId);
		if(!userExists)
			throw new NotFoundException("User not found");
		
		var alreadyReported = await _context.Reports.AnyAsync(x => x.UserId == report.UserId && x.PostId == report.PostId);
		if (alreadyReported)
			throw new NotValidException("Report already exists");

		report.State = ReportState.Pending;
		report.DateCreated = _dateTimeProvider.GetDateTime();
		
		_context.Add(report);
		await _context.SaveChangesAsync();
	}

	private IQueryable<Report> GetFilteredReports(ReportsFilter filter)
	{
		return _context.Reports.AsNoTracking()
			.Include(r => r.Post)
				.ThenInclude(p => p!.User)
			.Include(r => r.Post)
				.ThenInclude(p => p!.Images)
			.Include(r => r.User)
			.Where(r => r.State == ReportState.Pending);
	}
}