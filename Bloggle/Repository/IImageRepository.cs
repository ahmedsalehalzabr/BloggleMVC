

using Bloggle.Models.Domain;

namespace Bloggle.Repository
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
