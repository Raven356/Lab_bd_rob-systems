using BlogPostBll.Enums;
using BlogPostBll.Models;

namespace BlogPost.Models.Blogs
{
    public class DetailsBlogModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public CategoryEnum Category { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public Guid AuthorId { get; set; }
    }
}
