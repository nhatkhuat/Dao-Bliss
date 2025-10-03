using DaoBlissWebApp.Common.Entities;

namespace DaoBlissWebApp.Interfaces.Repositories
{
	public interface ICouponRepository
	{
		Task<Coupon?> GetCouponByIdAsync(int id);
		Task<Coupon?> GetCouponByCodeAsync(string code);
		Task<List<Coupon>> GetAllActiveCouponsAsync();
		Task AddCouponAsync(Coupon coupon);
		Task UpdateCouponAsync(Coupon coupon);
		Task DeleteCouponAsync(int id);
	}
}
