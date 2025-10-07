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

namespace DaoBlissWebApp.Areas.Admin.Pages.Products
{
	[Authorize(Roles = "Admin")]
	public class CreateModel : PageModel
	{
		private readonly ApplicationDbContext _context;

		public CreateModel(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult OnGet()
		{
			ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
			return Page();
		}

		[BindProperty]
		public Product Product { get; set; } = default!;

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				Console.WriteLine(Product);
				return Page();
			}

			_context.Products.Add(Product);
			await _context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
	}
}
