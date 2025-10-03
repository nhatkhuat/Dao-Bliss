// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Bogus.DataSets;
using DaoBlissWebApp.Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DaoBlissWebApp.Areas.Identity.Pages.Account.Manage
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
        public class InputModel
        {
<<<<<<< HEAD
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone(ErrorMessage = "Wrong {0} format")]
=======
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required]
            [Phone(ErrorMessage = "Sai định dạng số điện thoại")]
>>>>>>> Nhat
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
			[Required]
			[StringLength(50)]
<<<<<<< HEAD
			[Display(Name = "FirstName")]
			public string FirstName { get; set; }
			[Required]
			[StringLength(50)]
			[Display(Name = "LastName")]
			public string LastName { get; set; }
			[Required]
			[Display(Name = "Gender")]
			public bool? Gender { get; set; }
            [Required]
			[Display(Name = "Birthdate")]
			public DateOnly? DateOfBirth { get; set; }
		}

		private async Task LoadAsync(ApplicationUser user)
		{
			var userName = await _userManager.GetUserNameAsync(user);
			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

			Username = userName;

			Input = new InputModel
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
=======
			[Display(Name = "Họ Tên")]
			public string FirstName { get; set; }

			[Display(Name = "Giới tính")]
			public bool? Gender { get; set; }
			[Display(Name = "Ngày sinh")]
			public DateOnly? DateOfBirth { get; set; }
		}

		private async Task LoadAsync(ApplicationUser user)
		{
			var userName = await _userManager.GetUserNameAsync(user);
			var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

			Username = userName;

			Input = new InputModel
			{
				FirstName = user.FirstName,
				//LastName = user.LastName,
>>>>>>> Nhat
				DateOfBirth = user.DateOfBirth,
				Gender = user.Gender,
				PhoneNumber = phoneNumber
			};
		}

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
				TempData["ToastType"] = "error";
				TempData["ToastMessage"] = "Có lỗi, vui lòng thử lại!";
				return Page();
            }

            //var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            //if (Input.PhoneNumber != phoneNumber)
            //{
            //    var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            //    if (!setPhoneResult.Succeeded)
            //    {
            //        StatusMessage = "Unexpected error when trying to set phone number.";
            //        return RedirectToPage();
            //    }
            //}
            user.PhoneNumber = Input.PhoneNumber;
            user.FirstName = Input.FirstName;
<<<<<<< HEAD
            user.LastName = Input.LastName;
=======
>>>>>>> Nhat
			if (Input.DateOfBirth.HasValue)
			{
				user.DateOfBirth = Input.DateOfBirth.Value;
			}
			user.Gender = Input.Gender;

            await _userManager.UpdateAsync(user);

			await _signInManager.RefreshSignInAsync(user);
<<<<<<< HEAD

            StatusMessage = "Your profile has been updated";
=======

			TempData["ToastType"] = "success";
			TempData["ToastMessage"] = "Thông tin cá nhân đã được cập nhật!";

>>>>>>> Nhat
            return RedirectToPage();
        }
    }
}
