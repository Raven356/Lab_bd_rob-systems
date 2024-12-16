using BlogPostBll.Enums;

namespace BlogPost.Models.Blogs
{
    public class CreateBlogModel
    {
        public string Text { get; set; }

        public string Title { get; set; }

        public CategoryEnum Category { get; set; }

        public int AuthorId { get; set; }
    }
}
