using Bloggle.Models.Domain;
using Bloggle.Models.ViewModels;
using Bloggle.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bloggle.Controllers
{
    public class AdminBlogPostController : Controller
    {
        private readonly ITagRepository tagRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public AdminBlogPostController(ITagRepository tagRepository,IBlogPostRepository blogPostRepository)
        {
            this.tagRepository = tagRepository;
            this.blogPostRepository = blogPostRepository;
        }

        [HttpGet]
      public async Task<IActionResult> Add()
        {
            var tags = await tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {

                Heading = addBlogPostRequest.Heading,
                ShortDescription = addBlogPostRequest.ShortDescription,
                Author = addBlogPostRequest.Author,
                PublishedDate = addBlogPostRequest.PublishedDate,
                PageTitle = addBlogPostRequest.PageTitle,
                Content = addBlogPostRequest.Content,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                UrlHandle = addBlogPostRequest.UrlHandle,
                Visible = addBlogPostRequest.Visible,

            };
            var selectTags = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTag)
            {
                var selectedTagAsGuid = Guid.Parse(selectedTagId);
                var existingTag = await tagRepository.GetAsync(selectedTagAsGuid);
                
                if (existingTag != null)
                {
                    selectTags.Add(existingTag);
                }
            }
            // Mapping tags back to domain model 
            blogPost.Tags = selectTags;

            await blogPostRepository.AddAsync(blogPost);
            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var blogPosts = await blogPostRepository.GetAllPostsAsync();

            return View(blogPosts);
        }
    }
}
