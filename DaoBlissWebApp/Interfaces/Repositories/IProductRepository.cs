using DaoBlissWebApp.Common.Entities;



namespace DaoBlissWebApp.Interfaces.Repositories
{
	public interface IProductRepository
	{

		Task<Product?> GetProductByIdAsync(int id);
		Task<Product?> GetProductBySlugAsync(string slug);
		Task<List<Product>> GetAllActiveProductsAsync();
		Task<List<Product>> SearchProductsAsync(string searchTerm);
		Task<List<Product>> GetRelatedProductsAsync(int productId, int take = 4);
		Task AddProductAsync(Product product);
		Task UpdateProductAsync(Product product);
		Task DeleteProductAsync(int id);

		Task<ProductVariant?> GetProductVariantByIdAsync(int id);
		Task<List<ProductVariant>> GetProductVariantsByProductIdAsync(int productId);
		Task AddProductVariantAsync(ProductVariant variant);
		Task UpdateProductVariantAsync(ProductVariant variant);
		Task DeleteProductVariantAsync(int id);

		Task<ProductImage?> GetProductImageByIdAsync(int id);
		Task<List<ProductImage>> GetProductImagesByProductIdAsync(int productId);
		Task AddProductImageAsync(ProductImage image);
		Task UpdateProductImageAsync(ProductImage image);
		Task DeleteProductImageAsync(int id);

		Task<ProductReview?> GetProductReviewByIdAsync(int id);
		Task<List<ProductReview>> GetProductReviewsByProductIdAsync(int productId);
		Task AddProductReviewAsync(ProductReview review);
		Task UpdateProductReviewAsync(ProductReview review);
		Task DeleteProductReviewAsync(int id);

		Task<Size?> GetSizeByIdAsync(int id);
		Task<List<Size>> GetAllActiveSizesAsync();
		Task AddSizeAsync(Size size);
		Task UpdateSizeAsync(Size size);
		Task DeleteSizeAsync(int id);
	}
}
