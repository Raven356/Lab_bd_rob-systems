using BlogPostBll.Enums;

namespace BlogPost.Models.Blogs
{
    public class DetailsBlogModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public CategoryEnum Category { get; set; }
    }
}
