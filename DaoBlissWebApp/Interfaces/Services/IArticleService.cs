using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Interfaces.Services
{
	public interface IArticleService
	{
		Task<Article?> GetArticleBySlugAsync(string slug);
		Task<List<Article>> GetPublishedArticlesAsync(int pageIndex, int pageSize);
		Task<int> GetPublishedArticlesCountAsync();
	}
}
