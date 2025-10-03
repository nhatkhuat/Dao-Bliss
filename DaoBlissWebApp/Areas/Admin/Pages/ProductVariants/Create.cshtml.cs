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

namespace DaoBlissWebApp.Areas.Admin.Pages.ProductVariants
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
        ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
        ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public ProductVariant ProductVariant { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine(ProductVariant);
                return Page();
            }

            _context.Variants.Add(ProductVariant);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
