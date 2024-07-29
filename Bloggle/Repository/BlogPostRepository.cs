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

        public Task<BlogPost> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BlogPost>> GetAllPostsAsync()
        {
            return await appDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public Task<BlogPost> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> UpdateAsync(BlogPost blogPost)
        {
            throw new NotImplementedException();
        }
    }
}
