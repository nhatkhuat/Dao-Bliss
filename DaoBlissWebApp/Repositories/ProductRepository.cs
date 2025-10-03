
using DaoBlissWebApp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Repositories
{
	public class ProductRepository : IProductRepository
	{
		private readonly ApplicationDbContext _context;

		public ProductRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Product?> GetProductByIdAsync(int id)
		{
			return await _context.Products
				.Include(p => p.Category)
				.Include(p => p.Variants.Where(v => v.IsActive)).ThenInclude(v => v.Size)
				.Include(p => p.Images)
				.Include(p => p.Reviews).ThenInclude(r => r.User)
				.FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
		}

		public async Task<Product?> GetProductBySlugAsync(string slug)
		{
			return await _context.Products
				.Include(p => p.Category)
				.Include(p => p.Variants.Where(v => v.IsActive)).ThenInclude(v => v.Size)
				.Include(p => p.Images)
				.Include(p => p.Reviews).ThenInclude(r => r.User)
				.FirstOrDefaultAsync(p => p.Slug == slug && p.IsActive);
		}

		public async Task<List<Product>> GetAllActiveProductsAsync()
		{
			return await _context.Products
				.Where(p => p.IsActive && p.Category.IsActive)
				.Include(p => p.Category)
				.Include(p => p.Variants.Where(v => v.IsActive)).ThenInclude(v => v.Size)
				.Include(p => p.Images)
				.ToListAsync();
		}

		public async Task<List<Product>> SearchProductsAsync(string searchTerm)
		{
			return await _context.Products
				.Where(p => p.IsActive && p.Category.IsActive &&
							(p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)))
				.Include(p => p.Category)
				.Include(p => p.Variants.Where(v => v.IsActive)).ThenInclude(v => v.Size)
				.Include(p => p.Images)
				.ToListAsync();
		}

		public async Task<List<Product>> GetRelatedProductsAsync(int productId, int take = 4)
		{
			return await _context.Products
				.Where(p => p.Id != productId && p.IsActive)
				.Include(p => p.Images)
				.Include(p => p.Variants.Where(v => v.IsActive))
				.Take(take)
				.ToListAsync();
		}

		public async Task AddProductAsync(Product product)
		{
			_context.Products.Add(product);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductAsync(Product product)
		{
			_context.Products.Update(product);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductAsync(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				_context.Products.Remove(product);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<ProductVariant?> GetProductVariantByIdAsync(int id)
		{
			return await _context.Variants
				.Include(v => v.Product)
				.Include(v => v.Size)
				.FirstOrDefaultAsync(v => v.Id == id && v.IsActive);
		}
		public async Task<List<ProductVariant>> GetProductVariantsByProductIdAsync(int productId)
		{
			return await _context.Variants
				.Where(v => v.ProductId == productId && v.IsActive)
				.Include(v => v.Size)
				.ToListAsync();
		}

		public async Task AddProductVariantAsync(ProductVariant variant)
		{
			_context.Variants.Add(variant);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductVariantAsync(ProductVariant variant)
		{
			_context.Variants.Update(variant);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductVariantAsync(int id)
		{
			var variant = await _context.Variants.FindAsync(id);
			if (variant != null)
			{
				_context.Variants.Remove(variant);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<ProductImage?> GetProductImageByIdAsync(int id)
		{
			return await _context.ProductImages.FindAsync(id);
		}

		public async Task<List<ProductImage>> GetProductImagesByProductIdAsync(int productId)
		{
			return await _context.ProductImages.Where(i => i.ProductId == productId).ToListAsync();
		}

		public async Task AddProductImageAsync(ProductImage image)
		{
			_context.ProductImages.Add(image);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductImageAsync(ProductImage image)
		{
			_context.ProductImages.Update(image);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductImageAsync(int id)
		{
			var image = await _context.ProductImages.FindAsync(id);
			if (image != null)
			{
				_context.ProductImages.Remove(image);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<ProductReview?> GetProductReviewByIdAsync(int id)
		{
			return await _context.ProductReviews
				.Include(r => r.User)
				.FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task<List<ProductReview>> GetProductReviewsByProductIdAsync(int productId)
		{
			return await _context.ProductReviews
				.Where(r => r.ProductId == productId && r.IsApproved)
				.Include(r => r.User)
				.ToListAsync();
		}

		public async Task AddProductReviewAsync(ProductReview review)
		{
			_context.ProductReviews.Add(review);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateProductReviewAsync(ProductReview review)
		{
			_context.ProductReviews.Update(review);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteProductReviewAsync(int id)
		{
			var review = await _context.ProductReviews.FindAsync(id);
			if (review != null)
			{
				_context.ProductReviews.Remove(review);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<Size?> GetSizeByIdAsync(int id)
		{
			return await _context.Sizes.FindAsync(id);
		}

		public async Task<List<Size>> GetAllActiveSizesAsync()
		{
			return await _context.Sizes.Where(s => s.IsActive).ToListAsync();
		}
		public async Task AddSizeAsync(Size size)
		{
			_context.Sizes.Add(size);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateSizeAsync(Size size)
		{
			_context.Sizes.Update(size);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteSizeAsync(int id)
		{
			var size = await _context.Sizes.FindAsync(id);
			if (size != null)
			{
				_context.Sizes.Remove(size);
				await _context.SaveChangesAsync();
			}
		}
	}


}
