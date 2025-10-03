using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Pages
{
	public class ProductDetailModel : PageModel
	{
		private readonly IProductService _productService;

		public ProductDetailModel(IProductService productService)
		{
			_productService = productService;
		}

		public Product Product { get; set; } = default!;
		public IList<Product> RelatedProducts { get; set; } = default!;

		[BindProperty(SupportsGet = true)]
		public string Slug { get; set; }

		[BindProperty]
		public ProductReview ReviewInput { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync()
		{
			Product = await _productService.GetProductBySlugAsync(Slug);
			if (Product == null)
			{
				return NotFound();
			}
			RelatedProducts = await _productService.GetRelatedProductsAsync(Product.Id);
			return Page();
		}

		public async Task<IActionResult> OnPostReviewAsync()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Request.Path });
			}

			ModelState.Remove("ReviewInput.UserId");
			ModelState.Remove("ReviewInput.Title");
			if (!ModelState.IsValid)
			{
				return Page();
			}

			Product = await _productService.GetProductBySlugAsync(Slug);
			if (Product == null)
			{
				return NotFound();
			}

			var review = new ProductReview
			{
				ProductId = Product.Id,
				UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
				Rating = ReviewInput.Rating,
				Comment = ReviewInput.Comment,
				IsApproved = false,
				CreatedAt = DateTime.Now
			};

			await _productService.AddReviewAsync(review);

			return RedirectToPage(new { Slug = Slug });
		}
		//private readonly ApplicationDbContext _context;

		//public ProductDetailModel(ApplicationDbContext context)
		//{
		//	_context = context;
		//}

		//public Product Product { get; set; } = default!;
		//public IList<Product> RelatedProducts { get; set; } = default!;

		//[BindProperty(SupportsGet = true)]
		//public string Slug { get; set; }

		//[BindProperty]
		//public ProductReview ReviewInput { get; set; } = default!;
		//public async Task<IActionResult> OnGetAsync()
		//{
		//	Product = await _context.Products
		//		.Include(p => p.Images)
		//		.Include(p => p.Reviews).ThenInclude(review => review.User)
		//		.Include(p => p.Variants.Where(v => v.IsActive))
		//		.ThenInclude(v => v.Size)
		//		.FirstOrDefaultAsync(p => p.Slug.Equals(Slug) && p.IsActive);
		//	if (Product == null)
		//	{
		//		return NotFound();
		//	}
		//	RelatedProducts = await _context.Products
		//		.Include(p => p.Images)
		//		.Include(p => p.Variants.Where(v => v.IsActive))
		//		.Where(p => p.Id != Product.Id && p.IsActive)
		//		.Take(4)
		//		.ToListAsync();
		//	return Page();
		//}

		//public async Task<IActionResult> OnPostReviewAsync()
		//{
		//	if (!User.Identity.IsAuthenticated)
		//	{
		//		return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Request.Path });
		//	}
		//	ModelState.Remove("ReviewInput.UserId");
		//	ModelState.Remove("ReviewInput.Title");
		//	// title, rating, userid
		//	if (!ModelState.IsValid)
		//	{
		//		return Page();
		//	}
		//	// Load lại product từ Slug
		//	Product = await _context.Products.FirstOrDefaultAsync(p => p.Slug == Slug && p.IsActive);
		//	if (Product == null)
		//	{
		//		return NotFound();
		//	}

		//	var review = new ProductReview
		//	{
		//		ProductId = Product.Id,
		//		UserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value,
		//		Rating = ReviewInput.Rating,
		//		Comment = ReviewInput.Comment,
		//		IsApproved = false,
		//		CreatedAt = DateTime.Now
		//	};

		//	_context.ProductReviews.Add(review);
		//	await _context.SaveChangesAsync();

		//	return RedirectToPage(new { Slug = Slug });
		//}

	}
}
