using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DaoBlissWebApp.Repositories
{
	public class UserAddressRepository : IUserAddressRepository
	{
		private readonly ApplicationDbContext _context;

		public UserAddressRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<UserAddress?> GetUserAddressByIdAsync(int id)
		{
			return await _context.UserAddresses.FindAsync(id);
		}

		public async Task<List<UserAddress>> GetUserAddressesByUserIdAsync(string userId)
		{
			return await _context.UserAddresses.Where(a => a.UserId == userId).ToListAsync();
		}

		public async Task AddUserAddressAsync(UserAddress address)
		{
			_context.UserAddresses.Add(address);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateUserAddressAsync(UserAddress address)
		{
			_context.UserAddresses.Update(address);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteUserAddressAsync(int id)
		{
			var address = await _context.UserAddresses.FindAsync(id);
			if (address != null)
			{
				_context.UserAddresses.Remove(address);
				await _context.SaveChangesAsync();
			}
		}
	}
}
