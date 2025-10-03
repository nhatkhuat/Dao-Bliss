using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Areas.Admin.Pages.Articles
{
	[Authorize(Roles = "Admin")]
	public class CreateModel : PageModel
    {
        private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

        public CreateModel(DaoBlissWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

		[BindProperty]
		public ArticleInputModel Article { get; set; }

		public class ArticleInputModel
		{
			[Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
			[MaxLength(300, ErrorMessage = "Tiêu đề không được vượt quá 300 ký tự")]
			public string Title { get; set; }

			[MaxLength(500, ErrorMessage = "Tóm tắt không được vượt quá 500 ký tự")]
			public string Summary { get; set; }

			[MaxLength(500, ErrorMessage = "URL hình ảnh không được vượt quá 500 ký tự")]
			public string ImageUrl { get; set; }

			[Required(ErrorMessage = "Vui lòng nhập nội dung")]
			public string Content { get; set; }

			[MaxLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
			public string Status { get; set; } = "Draft";
		}

		public IActionResult OnGet()
		{
			Article = new ArticleInputModel();
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			var article = new Article
			{
				Title = Article.Title,
				Summary = Article.Summary,
				ImageUrl = Article.ImageUrl,
				Content = Article.Content,
				Status = Article.Status,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now
			};

			_context.Articles.Add(article);
			await _context.SaveChangesAsync();

			return RedirectToPage("/Articles/Index");
		}
	}
}
