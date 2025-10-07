using CloudinaryDotNet;
using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Common.Helpers;
using DaoBlissWebApp.Data;
using DaoBlissWebApp.Interfaces;
using DaoBlissWebApp.Interfaces.Repositories;
using DaoBlissWebApp.Interfaces.Services;
using DaoBlissWebApp.Repositories;
using DaoBlissWebApp.Services;
using DaoBlissWebApp.Services.CartService;
using DaoBlissWebApp.Services.OrderServices;
using DaoBlissWebApp.Services.ProductService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Configuration;
using System.Net.Http.Headers;

namespace DaoBlissWebApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddRazorPages();
			// Cấu hình DbContext của EF Core
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

			builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			// Đăng ký các repository hoặc service khác nếu cần
			builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
			builder.Services.AddScoped<ICouponRepository, CouponRepository>();
			builder.Services.AddScoped<ICustomerRankRepository, CustomerRankRepository>();
			builder.Services.AddScoped<IOrderRepository, OrderRepository>();
			builder.Services.AddScoped<IProductRepository, ProductRepository>();
			builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();

			// Register Services
			builder.Services.AddScoped<IArticleService, ArticleService>();
			builder.Services.AddScoped<IProductService, ProductService>();
			builder.Services.AddScoped<IOrderService, OrderService>();
			builder.Services.AddScoped<ICouponService, CouponService>();


			builder.Services.Configure<IdentityOptions>(options =>
			{
				// Thiet lap Password
				options.Password.RequireDigit = false;
				options.Password.RequireLowercase = false;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequiredLength = 3;
				options.Password.RequiredUniqueChars = 1;

				// Cau hinh Lockout - khóa user
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 10;
				options.Lockout.AllowedForNewUsers = false;

				// Cau hinh User.
				options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = true;

				// Cau hinh đăng nhap.
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;
				options.SignIn.RequireConfirmedAccount = false;
			});
			var mailsettings = builder.Configuration.GetSection("MailSettings");
			builder.Services.Configure<MailSettings>(mailsettings);
			builder.Services.AddTransient<IEmailSender, SendMailService>();
			builder.Services.AddScoped<IOrderRepository, OrderRepository>();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/login"; // Trang chuyển đến khi chưa đăng nhập
				options.LogoutPath = "/logout"; // Đường dẫn đăng xuất
				options.AccessDeniedPath = "/accessDenied.html"; // Đường dẫn khi truy cập bị từ chối
																 // Thời gian session khi KHÔNG chọn RememberMe
				options.ExpireTimeSpan = TimeSpan.FromDays(14);
				// Nếu true thì cookie sẽ được reset lại thời gian khi user có hoạt động
				options.SlidingExpiration = false;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Enforce Secure attribute
				options.Cookie.SameSite = SameSiteMode.Strict; // Set SameSite to Lax or Strict
				options.Cookie.HttpOnly = true; // Prevent client-side script access
			});
			// Configure antiforgery cookies
			builder.Services.AddAntiforgery(options =>
			{
				options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Enforce Secure for antiforgery cookies
				options.Cookie.SameSite = SameSiteMode.Strict;
				options.Cookie.HttpOnly = true;
			});
			builder.Services.AddAuthentication().AddGoogle(options =>
			{
				var gconfig = builder.Configuration.GetSection("Authentication:Google");
				options.ClientId = gconfig["ClientId"];
				options.ClientSecret = gconfig["ClientSecret"];
				options.CallbackPath = "/login-with-google";// Đường dẫn callback sau khi đăng nhập thành công
				options.Events.OnRemoteFailure = context =>
				{
					context.Response.Redirect($"/login");
					context.HandleResponse();
					return Task.CompletedTask;
				};
			});
			builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
			builder.Services.AddHttpClient("Gemini", client =>
			{
				client.BaseAddress = new Uri("https://generativelanguage.googleapis.com/");
				client.DefaultRequestHeaders.Add("Accept", "application/json");
				client.Timeout = TimeSpan.FromSeconds(60); // Add timeout
			});
			builder.Services.Configure<GhnSettings>(builder.Configuration.GetSection("GHN"));

			builder.Services.AddHttpClient("GHN", (sp, client) =>
			{
				//var settings = sp.GetRequiredService<IOptions<GhnSettings>>().Value;
				var settings = builder.Configuration.GetSection("GHN").Get<GhnSettings>();
				client.BaseAddress = new Uri(settings.BaseUrl);
				client.DefaultRequestHeaders.Add("Token", settings.ApiKey);
				client.DefaultRequestHeaders.Add("ShopId", settings.ShopId);
			});
			builder.Services.AddLogging();
			var app = builder.Build();
			// Tự động chạy migration khi deploy
			//using (var scope = app.Services.CreateScope())
			//{
			//	var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
			//	try
			//	{
			//		// Tăng timeout trước khi migrate
			//		context.Database.SetCommandTimeout(300);

			//		await context.Database.MigrateAsync();

			//		Console.WriteLine("Migration completed successfully");
			//	}
			//	catch (Exception ex)
			//	{
			//		Console.WriteLine($"Migration failed: {ex.Message}");
			//		throw;
			//	}
			//}

			await CreateRolesAndAdminAsync(app);

			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorPages();
			app.UseStatusCodePages(async context =>
			{
				if (context.HttpContext.Response.StatusCode == 404)
				{
					context.HttpContext.Response.Redirect("/NotFound");
				}
			});
			await app.RunAsync();
		}
		static async Task CreateRolesAndAdminAsync(WebApplication app)
		{
			using (var scope = app.Services.CreateScope())
			{
				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

				// Các role mặc định
				string[] roles = { "Admin", "Manager" };

				foreach (var role in roles)
				{
					if (!await roleManager.RoleExistsAsync(role))
					{
						await roleManager.CreateAsync(new IdentityRole(role));
					}
				}

				// Tạo tài khoản Admin mặc định
				string adminEmail = "admin@yourapp.com";
				string adminPassword = "Admin@123"; // đổi sau khi deploy

				var adminUser = await userManager.FindByEmailAsync(adminEmail);
				if (adminUser == null)
				{
					var newAdmin = new ApplicationUser
					{
						UserName = "admin",
						Email = adminEmail,
						EmailConfirmed = true
					};

					var result = await userManager.CreateAsync(newAdmin, adminPassword);
					if (result.Succeeded)
					{
						await userManager.AddToRoleAsync(newAdmin, "Admin");
					}
				}
			}
		}
	}
}
