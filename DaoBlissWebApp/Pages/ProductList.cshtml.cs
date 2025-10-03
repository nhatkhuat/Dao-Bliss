using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Services;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace DaoBlissWebApp.Pages
{
	public class ProductListModel : PageModel
	{
		private readonly IProductService _productService;

		public ProductListModel(IProductService productService)
		{
			_productService = productService;
		}

		[BindProperty(SupportsGet = true)]
		public string? ProductSearch { get; set; }

		public IList<Product> Products { get; set; } = default!;

		public async Task OnGetAsync()
		{
			if (!string.IsNullOrEmpty(ProductSearch))
			{
				Products = await _productService.SearchProductsAsync(ProductSearch.Trim());
			}
			else
			{
				Products = await _productService.GetAllActiveProductsAsync();
			}
		}
	}
}
