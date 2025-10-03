using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaoBlissWebApp.Areas.Admin.Pages.Articles
{
	[Authorize(Roles = "Admin")]
	public class EditModel : PageModel
    {
		private readonly ApplicationDbContext _context;

		public EditModel(ApplicationDbContext context)
		{
			_context = context;
		}

		[BindProperty]
		public ArticleInputModel Article { get; set; }

		public class ArticleInputModel
		{
			public int Id { get; set; }

			[Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
			[MaxLength(300, ErrorMessage = "Tiêu đề không được vượt quá 300 ký tự")]
			public string Title { get; set; }
			public string Slug { get; set; }

			[MaxLength(500, ErrorMessage = "Tóm tắt không được vượt quá 500 ký tự")]
			public string Summary { get; set; }

			[MaxLength(500, ErrorMessage = "URL hình ảnh không được vượt quá 500 ký tự")]
			public string ImageUrl { get; set; }

			[Required(ErrorMessage = "Vui lòng nhập nội dung")]
			public string Content { get; set; }

			[MaxLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
			public string Status { get; set; } = "Draft";
		}

		public async Task<IActionResult> OnGetAsync(int id)
		{
			var article = await _context.Articles.FindAsync(id);
			if (article == null)
			{
				return NotFound();
			}

			Article = new ArticleInputModel
			{
				Id = article.Id,
				Title = article.Title,
				Slug = article.Slug,
				Summary = article.Summary,
				ImageUrl = article.ImageUrl,
				Content = article.Content,
				Status = article.Status
			};

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var article = await _context.Articles.FindAsync(Article.Id);
			if (article == null)
			{
				return NotFound();
			}

			article.Title = Article.Title;
			article.Slug = Article.Slug;
			article.Summary = Article.Summary;
			article.ImageUrl = Article.ImageUrl;
			article.Content = Article.Content;
			article.Status = Article.Status;
			article.UpdatedAt = DateTime.Now;

			_context.Articles.Update(article);
			await _context.SaveChangesAsync();

			return RedirectToPage("/Articles/Index");
		}
	}

}
