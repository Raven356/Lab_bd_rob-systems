using BlogPostBll.Models;

namespace BlogPostBll.Interfaces
{
    public interface ICommentsService
    {
        Task CreateAsync(Guid postId, string text, Guid authorId);

        Task EditAsync(Guid commentId, string text);

        Task<Comment> GetCommentById(Guid commentId);

        Task<IEnumerable<Comment>> GetCommentsForBlog(Guid blogId);

        Task DeleteCommentByIdAsync(Guid commentId);
    }
}
