using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Interfaces.Repositories
{
	public interface IUserAddressRepository
	{
		Task<UserAddress?> GetUserAddressByIdAsync(int id);
		Task<List<UserAddress>> GetUserAddressesByUserIdAsync(string userId);
		Task AddUserAddressAsync(UserAddress address);
		Task UpdateUserAddressAsync(UserAddress address);
		Task DeleteUserAddressAsync(int id);
	}
}
