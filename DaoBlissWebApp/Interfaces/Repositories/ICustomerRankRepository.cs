using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Interfaces.Repositories
{
	public interface ICustomerRankRepository
	{
		Task<CustomerRank?> GetCustomerRankByIdAsync(int id);
		Task<List<CustomerRank>> GetAllActiveCustomerRanksAsync();
		Task AddCustomerRankAsync(CustomerRank customerRank);
		Task UpdateCustomerRankAsync(CustomerRank customerRank);
		Task DeleteCustomerRankAsync(int id);
	}
}
