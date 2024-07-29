using Bloggle.Data;
using Bloggle.Models.Domain;
using Bloggle.Models.ViewModels;
using Bloggle.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggle.Controllers
{
    public class AdminTagsController : Controller
    {
       
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
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

            await tagRepository.AddAsync(tag);
        
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
          var tags = await tagRepository.GetAllAsync();
            return View(tags);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
          //  var tag = appDbContext.Tags.Find(id);
            var tag = await tagRepository.GetAsync(id);

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

            var updateTag = await tagRepository.UpdateAsync(tag);

            if (updateTag != null)
            {
                return RedirectToAction("List");
            }
            else
            {

            }
            return RedirectToAction("Edit", new  { id = editTagRequest.Id });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var delete = await tagRepository.DeleteAsync(editTagRequest.Id);
            if (delete != null)
            {
                // Show success notification
                return RedirectToAction("List");
            }

            // Show an error notification
            return RedirectToAction("Edit", new {id  = editTagRequest.Id});
        }
    }
}
