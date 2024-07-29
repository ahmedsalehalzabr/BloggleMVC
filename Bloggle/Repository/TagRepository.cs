using Bloggle.Data;
using Bloggle.Models.Domain;
using Bloggle.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Bloggle.Repository
{
    public class TagRepository : ITagRepository
    {
        private readonly AppDbContext appDbContext;

        public TagRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await appDbContext.Tags.AddAsync(tag);
            await appDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var tag = await appDbContext.Tags.FindAsync(id);

            if (tag != null)
            {
                appDbContext.Tags.Remove(tag);
                await appDbContext.SaveChangesAsync();
                return tag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
           return await appDbContext.Tags.ToListAsync();
        }

        public async Task<Tag> GetAsync(Guid id)
        {
            return await appDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var exitingTag = await appDbContext.Tags.FindAsync(tag.Id);
            if (exitingTag != null)
            {
                exitingTag.Name = tag.Name;
                exitingTag.DisplayName = tag.DisplayName;


                await appDbContext.SaveChangesAsync();
                return exitingTag;
            }

            return null;
           
        }
    }
}
