using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Interfaces.Repositories
{
	public interface IArticleRepository
	{
		Task<Article?> GetArticleByIdAsync(int id);
		Task<Article?> GetArticleBySlugAsync(string slug);
		Task<List<Article>> GetAllArticlesAsync();
		Task<List<Article>> GetPublishedArticlesAsync(int pageIndex = 1, int pageSize = 3);
		Task<int> GetPublishedArticlesCountAsync();
		Task AddArticleAsync(Article article);
		Task UpdateArticleAsync(Article article);
		Task DeleteArticleAsync(int id);
	}
}
