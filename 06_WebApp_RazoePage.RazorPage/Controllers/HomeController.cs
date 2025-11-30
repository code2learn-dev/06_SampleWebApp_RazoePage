using Microsoft.AspNetCore.Mvc;

namespace _06_WebApp_RazoePage.RazorPage.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View("Index", "MVC Controller");
		}

	}
}
