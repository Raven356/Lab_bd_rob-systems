using BlogPostBll.Models;

namespace BlogPostBll.Interfaces
{
    public interface IBlogService
    {
        public Task<Guid> CreateBlogAsync(Blog newBlog);

        public Task<IEnumerable<Blog>> GetAllAsync();

        public Task<Blog> GetByIdAsync(Guid id);

        public Task<Guid> EditBlogAsync(Blog editBlog);

        public Task DeleteBlogAsync(Guid id);
    }
}
