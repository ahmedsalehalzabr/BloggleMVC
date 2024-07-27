using Microsoft.AspNetCore.Mvc;

namespace Bloggle.Controllers
{
    public class BlogPostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
