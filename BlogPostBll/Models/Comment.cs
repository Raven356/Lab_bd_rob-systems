namespace BlogPostBll.Models
{
    public class Comment
    {
        public Guid Id { get; set; }

        public int AuthorId { get; set; }

        public Guid BlogId { get; set; }

        public string Text { get; set; }
    }
}
