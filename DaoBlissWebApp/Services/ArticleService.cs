using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Interfaces.Repositories;
using DaoBlissWebApp.Interfaces.Services;

namespace DaoBlissWebApp.Services
{
	public class ArticleService : IArticleService
	{
		private readonly IArticleRepository _articleRepository;

		public ArticleService(IArticleRepository articleRepository)
		{
			_articleRepository = articleRepository;
		}

		public async Task<Article?> GetArticleBySlugAsync(string slug)
		{
			return await _articleRepository.GetArticleBySlugAsync(slug);
		}

		public async Task<List<Article>> GetPublishedArticlesAsync(int pageIndex, int pageSize)
		{
			return await _articleRepository.GetPublishedArticlesAsync(pageIndex, pageSize);
		}

		public async Task<int> GetPublishedArticlesCountAsync()
		{
			return await _articleRepository.GetPublishedArticlesCountAsync();
		}
	}
}
