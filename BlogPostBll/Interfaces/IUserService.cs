using BlogPostBll.Models;

namespace BlogPostBll.Interfaces
{
    public interface IUserService
    {
        Task RegisterAsync(User user);

        Task<bool> LoginAsync(User user);
    }
}
