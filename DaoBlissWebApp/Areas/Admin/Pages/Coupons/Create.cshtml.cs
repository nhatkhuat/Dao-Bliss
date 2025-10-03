using DaoBlissWebApp.Common.Entities;
using DaoBlissWebApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaoBlissWebApp.Areas.Admin.Pages.Coupons
{
	[Authorize(Roles = "Admin")]
	public class CreateModel : PageModel
    {
        private readonly DaoBlissWebApp.Data.ApplicationDbContext _context;

        public CreateModel(DaoBlissWebApp.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["MinRankId"] = new SelectList(_context.CustomerRanks, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Coupon Coupon { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Coupons.Add(Coupon);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
