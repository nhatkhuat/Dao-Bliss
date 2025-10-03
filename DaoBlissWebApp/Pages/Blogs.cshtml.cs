using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Pages
{
    public class BlogsModel : PageModel
    {
		private readonly IArticleService _articleService;

		public BlogsModel(IArticleService articleService)
		{
			_articleService = articleService;
		}

		public List<Article> Articles { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; } = 3;

		public async Task OnGetAsync(int? pageIndex)
		{
			CurrentPage = pageIndex ?? 1;
			if (CurrentPage < 1) CurrentPage = 1;

			var totalArticles = await _articleService.GetPublishedArticlesCountAsync();
			TotalPages = (int)Math.Ceiling(totalArticles / (double)PageSize);
			if (CurrentPage > TotalPages && TotalPages > 0) CurrentPage = TotalPages;

			Articles = await _articleService.GetPublishedArticlesAsync(CurrentPage, PageSize);
		}
	}
}
