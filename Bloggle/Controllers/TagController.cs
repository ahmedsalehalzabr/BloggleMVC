using Microsoft.AspNetCore.Mvc;

namespace Bloggle.Controllers
{
    public class TagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
