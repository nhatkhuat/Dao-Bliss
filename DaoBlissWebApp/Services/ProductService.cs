
using DaoBlissWebApp.Interfaces.Services;
using DaoBlissWebApp.Interfaces.Repositories;
using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Services.ProductService
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Product?> GetProductBySlugAsync(string slug)
		{
			return await _productRepository.GetProductBySlugAsync(slug);
		}

		public async Task<List<Product>> GetAllActiveProductsAsync()
		{
			return await _productRepository.GetAllActiveProductsAsync();
		}

		public async Task<List<Product>> SearchProductsAsync(string searchTerm)
		{
			return await _productRepository.SearchProductsAsync(searchTerm);
		}

		public async Task<List<Product>> GetRelatedProductsAsync(int productId, int take = 4)
		{
			return await _productRepository.GetRelatedProductsAsync(productId, take);
		}

		public async Task AddReviewAsync(ProductReview review)
		{
			await _productRepository.AddProductReviewAsync(review);
		}

		public async Task<ProductVariant?> GetProductVariantByIdAsync(int id)
		{
			return await _productRepository.GetProductVariantByIdAsync(id);
		}

		public async Task<List<ProductVariant>> GetVariantsByIdsAsync(List<int> variantIds)
		{
			var variants = new List<ProductVariant>();
			foreach (var id in variantIds)
			{
				var variant = await GetProductVariantByIdAsync(id);
				if (variant != null) variants.Add(variant);
			}
			return variants;
		}

		public async Task UpdateVariantStockAsync(int variantId, int quantityChange)
		{
			var variant = await GetProductVariantByIdAsync(variantId);
			if (variant != null)
			{
				variant.Stock += quantityChange;
				await _productRepository.UpdateProductVariantAsync(variant);
			}
		}
	}
}
