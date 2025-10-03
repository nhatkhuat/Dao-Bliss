using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Interfaces.Repositories
{
	public interface ICategoryRepository
	{
		Task<Category?> GetCategoryByIdAsync(int id);
		Task<List<Category>> GetAllActiveCategoriesAsync();
		Task AddCategoryAsync(Category category);
		Task UpdateCategoryAsync(Category category);
		Task DeleteCategoryAsync(int id);
	}
}
