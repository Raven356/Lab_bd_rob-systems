namespace BlogPost.Models.Comments
{
    public class CommentCreateModel
    {
        public Guid BlogId { get; set; }

        public string Text { get; set; }
    }
}
