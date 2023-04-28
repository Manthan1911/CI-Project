using CI_Platform_Web.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CI_Platform_Web.Controllers
{
	[Authenticate]
	public class PrivacyPolicyController : Controller
	{
		public IActionResult PrivacyPolicy()
		{
			return View();
		}
	}
}
