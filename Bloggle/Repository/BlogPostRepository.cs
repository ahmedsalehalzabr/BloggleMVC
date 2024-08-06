using Bloggle.Data;
using Bloggle.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggle.Repository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly AppDbContext appDbContext;

        public BlogPostRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await appDbContext.AddAsync(blogPost);
            await appDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost> DeleteAsync(Guid id)
        {
            var blogPost = await appDbContext.BlogPosts.FindAsync(id);

            if (blogPost != null)
            {
                appDbContext.BlogPosts.Remove(blogPost);
                await appDbContext.SaveChangesAsync(); 
                return blogPost;
            }
            return null;
        }

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await appDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost> GetByIdAsync(Guid id)
        {
            return await appDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            var existingBlog = await appDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlog != null)
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.Author = blogPost.Author;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Content = blogPost.Content;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Tags = blogPost.Tags;

                await appDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }
    }
}
