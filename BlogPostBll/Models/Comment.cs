namespace BlogPostBll.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid AuthorId { get; set; }

        public Guid BlogId { get; set; }

        public string Text { get; set; }
    }
}
