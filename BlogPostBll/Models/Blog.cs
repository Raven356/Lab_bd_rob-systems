using BlogPostBll.Enums;

namespace BlogPostBll.Models
{
    public class Blog
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public CategoryEnum Category { get; set; }

        public int AuthorId { get; set; }
    }
}
