using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.SPA.Controllers
{
	public class HomeController : Controller
	{
		// GET: /<controller>/
		[AllowAnonymous]
		public IActionResult Index()
		{
			return File("index.html", "text/html");
		}
	}
}
