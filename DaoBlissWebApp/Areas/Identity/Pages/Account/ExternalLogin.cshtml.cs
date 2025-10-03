// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using DaoBlissWebApp.Common.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace DaoBlissWebApp.Areas.Identity.Pages.Account
{
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }
        
        public IActionResult OnGet() => RedirectToPage("./Login");

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Đăng nhập thất bại: Vui lòng cấp quyền truy cập để sử dụng Google.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
<<<<<<< HEAD

=======
>>>>>>> Nhat
				// hệ thống có tk nhưng chưa lket vs gg -> lket tkhoan
				// chua có tk -> tạo tk, liên kết 

				// If the user does not have an account, then ask the user to create an account.
				
                ReturnUrl = returnUrl;
<<<<<<< HEAD
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
=======
				if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
				{
					var email = info.Principal.FindFirstValue(ClaimTypes.Email);
					var user = await _userManager.FindByEmailAsync(email);

					if (user == null)
					{
						// Tạo mới luôn
						user = new ApplicationUser { UserName = email, Email = email };
						var result2 = await _userManager.CreateAsync(user);
						if (result2.Succeeded)
						{
							await _userManager.AddLoginAsync(user, info);
							//var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
							//await _userManager.ConfirmEmailAsync(user, code);
						}
					}

					// Đăng nhập
					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}

				return Page();
>>>>>>> Nhat
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
				var registeredUser = await _userManager.FindByEmailAsync(Input.Email);
				string externalMail = null;
				ApplicationUser externalEmailUser = null;

				if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
				{
					externalMail = info.Principal.FindFirstValue(ClaimTypes.Email);
				}
				if (externalMail != null)
				{
					externalEmailUser = await _userManager.FindByEmailAsync(externalMail);
				}
				// Xử lý khi có thông tin về email từ info, đồng thời có user với email đó
				// trường hợp này sẽ thực hiện liên kết tài khoản ngoài + xác thực email luôn     
				if ((registeredUser != null) && (externalEmailUser != null))
				{
					// xác nhận email luôn nếu chưa xác nhận
<<<<<<< HEAD
					if (registeredUser.Id == externalEmailUser.Id)
=======
					if (registeredUser.Id.Equals(externalEmailUser.Id))
>>>>>>> Nhat
					{
						var resultLink = await _userManager.AddLoginAsync(registeredUser, info);
						if (resultLink.Succeeded)
						{
							await _signInManager.SignInAsync(registeredUser, isPersistent: true);
							return LocalRedirect(returnUrl);
						}
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Không liên kết được tài khoản, hãy dùng email khác");
						return Page();
					}
				}

				if (externalEmailUser != null && registeredUser == null)
				{
					ModelState.AddModelError(string.Empty, "Không hỗ trợ tạo tài khoản mới có email khác với email từ dịch vụ ngoài");
					return Page();
				}

				if (externalEmailUser == null && externalMail == Input.Email)
				{
					// chua co tai khoan -> tao tai khoan moi, lien ket, login ngay
					var newUser = new ApplicationUser()
					{
						UserName = externalMail,
						Email = externalMail
					};
					var resultNewUser = await _userManager.CreateAsync(newUser);
					if (resultNewUser.Succeeded)
					{
						await _userManager.AddLoginAsync(newUser, info);
						var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
						await _userManager.ConfirmEmailAsync(newUser, code);
						await _signInManager.SignInAsync(newUser, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Không tạo được tài khoản mới");
						return Page();
					}
				}

				var user = CreateUser();

				await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

				var result = await _userManager.CreateAsync(user);
				if (result.Succeeded)
				{
					result = await _userManager.AddLoginAsync(user, info);
					if (result.Succeeded)
					{
						_logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

						var userId = await _userManager.GetUserIdAsync(user);
						var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						var callbackUrl = Url.Page(
							"/Account/ConfirmEmail",
							pageHandler: null,
							values: new { area = "Identity", userId = userId, code = code },
							protocol: Request.Scheme);

						await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
							$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

						// If account confirmation is required, we need to show the link if we don't have a real email sender
						if (_userManager.Options.SignIn.RequireConfirmedAccount)
						{
							return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
						}

						await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the external login page in /Areas/Identity/Pages/Account/ExternalLogin.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
