namespace DaoBlissWebApp.Interfaces.Services
{
	public interface ICouponService
	{
		Task<decimal> ApplyCouponAsync(string code, decimal subTotal);
	}
}
