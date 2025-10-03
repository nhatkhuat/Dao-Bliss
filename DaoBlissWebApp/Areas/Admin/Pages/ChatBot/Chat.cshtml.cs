using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace DaoBlissWebApp.Areas.Admin.Pages.ChatBot
{
	[Authorize(Roles = "Admin")]
	public class ChatModel : PageModel
    {
		private readonly IWebHostEnvironment _env;
		private readonly string _filePath;
		private readonly string _modelPath;

		public ChatModel(IWebHostEnvironment env)
		{
			_env = env;
			_filePath = Path.Combine(_env.WebRootPath, "chatbot", "data.txt");
			_modelPath = Path.Combine(_env.WebRootPath, "chatbot", "chatbotmodel.txt");
		}

		[BindProperty]
		public string DataContent { get; set; }
		[BindProperty]
		public string ModelContent { get; set; }

		public void OnGet()
		{
			DataContent = System.IO.File.Exists(_filePath) == true ? DataContent = System.IO.File.ReadAllText(_filePath) : "";
			ModelContent = System.IO.File.Exists(_modelPath) == true ? ModelContent = System.IO.File.ReadAllText(_modelPath) : "";

		}

		public IActionResult OnPostSave()
		{
			System.IO.File.WriteAllText(_filePath, DataContent);
			System.IO.File.WriteAllText(_modelPath, ModelContent);
			TempData["Message"] = "Đã lưu dữ liệu thành công!";
			return RedirectToPage();
		}

	}
}
