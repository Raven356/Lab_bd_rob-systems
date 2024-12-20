using BlogPostBll.Enums;

namespace BlogPost.Models.Blogs
{
    public class EditBlogModel
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public CategoryEnum Category { get; set; }
    }
}
