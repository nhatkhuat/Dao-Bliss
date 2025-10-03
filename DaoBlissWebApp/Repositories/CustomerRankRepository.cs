using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Repositories
{
	public class CustomerRankRepository : ICustomerRankRepository
	{
		private readonly ApplicationDbContext _context;

		public CustomerRankRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<CustomerRank?> GetCustomerRankByIdAsync(int id)
		{
			return await _context.CustomerRanks.FindAsync(id);
		}

		public async Task<List<CustomerRank>> GetAllActiveCustomerRanksAsync()
		{
			return await _context.CustomerRanks.Where(r => r.IsActive).ToListAsync();
		}

		public async Task AddCustomerRankAsync(CustomerRank customerRank)
		{
			_context.CustomerRanks.Add(customerRank);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateCustomerRankAsync(CustomerRank customerRank)
		{
			_context.CustomerRanks.Update(customerRank);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteCustomerRankAsync(int id)
		{
			var rank = await _context.CustomerRanks.FindAsync(id);
			if (rank != null)
			{
				_context.CustomerRanks.Remove(rank);
				await _context.SaveChangesAsync();
			}
		}
	}
}
