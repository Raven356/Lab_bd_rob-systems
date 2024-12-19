using BlogPost.Models.Users;
using BlogPostBll.Models;

namespace BlogPost.Mappers
{
    public static class UserMapper
    {
        public static User Map(LoginModel loginModel)
        {
            return new User
            {
                Email = loginModel.Email,
                Password = loginModel.Password,
            };
        }

        public static User Map(RegisterModel registerModel) 
        {
            return new User
            {
                Email = registerModel.Email,
                Password = registerModel.Password,
            };
        }
    }
}
