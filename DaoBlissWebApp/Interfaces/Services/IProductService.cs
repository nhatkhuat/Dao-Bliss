using DaoBlissWebApp.Common.Entities;


namespace DaoBlissWebApp.Interfaces.Services
{
	public interface IProductService
	{
		Task<Product?> GetProductBySlugAsync(string slug);
		Task<List<Product>> GetAllActiveProductsAsync();
		Task<List<Product>> SearchProductsAsync(string searchTerm);
		Task<List<Product>> GetRelatedProductsAsync(int productId, int take = 4);
		Task AddReviewAsync(ProductReview review);
		Task<ProductVariant?> GetProductVariantByIdAsync(int id);
		Task<List<ProductVariant>> GetVariantsByIdsAsync(List<int> variantIds);
		Task UpdateVariantStockAsync(int variantId, int quantityChange);
	}
}
