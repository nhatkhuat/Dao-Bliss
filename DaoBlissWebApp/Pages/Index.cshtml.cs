using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Pages
{
	public class IndexModel : PageModel
	{
		private readonly IProductService _productService;
		private readonly IArticleService _articleService;

		public IndexModel(IProductService productService, IArticleService articleService)
		{
			_productService = productService;
			_articleService = articleService;
		}

		public IList<Product> Product { get; set; } = default!;
		public IList<Article> Articles { get; set; } = default!;

		public async Task OnGetAsync()
		{
			Product = await _productService.GetAllActiveProductsAsync();
			Articles = await _articleService.GetPublishedArticlesAsync(1, int.MaxValue);
		}

		//public async Task OnGetAsync()
		//{
		//	Product = await _context.Products
		//		.Where(p => p.IsActive)
		//		.Include(p => p.Images)
		//		.Include(p => p.Variants.Where(v => v.IsActive))
		//		.ToListAsync();

		//	Articles = await _context.Articles
		//		.Where(p => p.Status.Equals("Published"))
		//		.ToListAsync();
		//}
	}
}
