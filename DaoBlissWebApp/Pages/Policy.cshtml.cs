using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DaoBlissWebApp.Pages
{
    public class PolicyModel : PageModel
    {
		public string Title { get; set; }

		public void OnGet(string title)
		{
			Title = title;
		}
	}
}
