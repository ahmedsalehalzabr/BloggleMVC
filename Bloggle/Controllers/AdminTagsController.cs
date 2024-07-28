using Bloggle.Data;
using Bloggle.Models.Domain;
using Bloggle.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Bloggle.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly AppDbContext appDbContext;

        public AdminTagsController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
         await appDbContext.Tags.AddAsync(tag);
         await appDbContext.SaveChangesAsync();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult List()
        {
          var tags = appDbContext.Tags.ToList();
            return View(tags);
        }
    }
}
