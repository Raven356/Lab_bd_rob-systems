using BlogPostBll.Models;

namespace BlogPostBll.Interfaces
{
    public interface IBlogService
    {
        public Guid CreateBlog(Blog newBlog);

        public IEnumerable<Blog> GetAll();

        public Blog GetById(Guid id);

        public Guid EditBlog(Blog editBlog);
    }
}
