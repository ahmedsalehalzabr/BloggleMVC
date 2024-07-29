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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
             var blgPosts = await blogPostRepository.GetByIdAsync(id);
            var tags = await tagRepository.GetAllAsync();

           if (blgPosts != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = blgPosts.Id,
                    Heading = blgPosts.Heading,
                    ShortDescription = blgPosts.ShortDescription,
                    Author = blgPosts.Author,
                    PublishedDate = blgPosts.PublishedDate,
                    PageTitle = blgPosts.PageTitle,
                    Content = blgPosts.Content,
                    FeaturedImageUrl = blgPosts.FeaturedImageUrl,
                    UrlHandle = blgPosts.UrlHandle,
                    Visible = blgPosts.Visible,
                    Tags = tags.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString(),
                    }),
                    SelectedTag = blgPosts.Tags.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPostDomanModel = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Heading = editBlogPostRequest.Heading,
                ShortDescription = editBlogPostRequest.ShortDescription,
                Author = editBlogPostRequest.Author,
                PageTitle = editBlogPostRequest.PageTitle,
                Content = editBlogPostRequest.Content,
                PublishedDate = editBlogPostRequest.PublishedDate,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible,

            };

            // Map tags into domain model 
            var seTags = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTag)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        seTags.Add(foundTag);
                    }
                }
            }

            blogPostDomanModel.Tags = seTags;

            //Submit information to repository to update
            var updateBlog = await blogPostRepository.UpdateAsync(blogPostDomanModel);
            if (updateBlog != null)
            {
                return RedirectToAction("List");
            }

            return RedirectToAction("Edit");
        }
    }
}
