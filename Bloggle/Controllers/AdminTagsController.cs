using Bloggle.Data;
using Bloggle.Models.Domain;
using Bloggle.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> List()
        {
          var tags = await appDbContext.Tags.ToListAsync();
            return View(tags);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
          //  var tag = appDbContext.Tags.Find(id);
            var tag = await appDbContext.Tags.FirstOrDefaultAsync( t => t.Id == id);

            if (tag != null)
            {
                var editTag = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName,

                };
                return View(editTag);
            }

            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            { 
                Id=editTagRequest.Id,
                Name = editTagRequest.Name, 
                DisplayName = editTagRequest.DisplayName,

            };

            var exitingTag = await appDbContext.Tags.FindAsync(tag.Id);

            if (exitingTag != null)
            {
                exitingTag.Name = tag.Name;
                exitingTag.DisplayName = tag.DisplayName;
                await appDbContext.SaveChangesAsync();

                return RedirectToAction("List");
            }

            return RedirectToAction("Edit", new  { id = editTagRequest.Id });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await appDbContext.Tags.FindAsync(editTagRequest.Id);

            if(tag != null)
            {
                appDbContext.Tags.Remove(tag);
               await appDbContext.SaveChangesAsync();
                // Show a success notification
                return RedirectToAction("List");
            }

            // Show an error notification
            return RedirectToAction("Edit", new {id  = editTagRequest.Id});
        }
    }
}
