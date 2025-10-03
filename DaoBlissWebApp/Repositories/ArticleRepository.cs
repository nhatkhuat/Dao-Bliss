using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Repositories
{
	public class ArticleRepository : IArticleRepository
	{
		private readonly ApplicationDbContext _context;

		public ArticleRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Article?> GetArticleByIdAsync(int id)
		{
			return await _context.Articles.FindAsync(id);
		}

		public async Task<Article?> GetArticleBySlugAsync(string slug)
		{
			return await _context.Articles
				.FirstOrDefaultAsync(a => a.Slug == slug && a.Status == "Published");
		}

		public async Task<List<Article>> GetAllArticlesAsync()
		{
			return await _context.Articles.ToListAsync();
		}

		public async Task<List<Article>> GetPublishedArticlesAsync(int pageIndex = 1, int pageSize = 3)
		{
			return await _context.Articles
				.Where(a => a.Status == "Published")
				.OrderByDescending(a => a.CreatedAt)
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<int> GetPublishedArticlesCountAsync()
		{
			return await _context.Articles.CountAsync(a => a.Status == "Published");
		}

		public async Task AddArticleAsync(Article article)
		{
			_context.Articles.Add(article);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateArticleAsync(Article article)
		{
			_context.Articles.Update(article);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteArticleAsync(int id)
		{
			var article = await _context.Articles.FindAsync(id);
			if (article != null)
			{
				_context.Articles.Remove(article);
				await _context.SaveChangesAsync();
			}
		}
	}
}
