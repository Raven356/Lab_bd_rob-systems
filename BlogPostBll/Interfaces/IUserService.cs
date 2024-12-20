using BlogPostBll.Models;

namespace BlogPostBll.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(User user);

        Task<bool> LoginAsync(User user);

        Task<Guid> GetUserIdByEmailAsync(string email);
    }
}
