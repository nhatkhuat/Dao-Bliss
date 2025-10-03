using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Pages
{
    public class BlogDetailModel : PageModel
    {
		private readonly IArticleService _articleService;

		public BlogDetailModel(IArticleService articleService)
		{
			_articleService = articleService;
		}

		public Article Article { get; set; }

		[BindProperty(SupportsGet = true)]
		public string Slug { get; set; }

		public async Task<IActionResult> OnGetAsync()
		{
			Article = await _articleService.GetArticleBySlugAsync(Slug);
			if (Article == null)
			{
				return NotFound();
			}
			return Page();
		}
	}
}
