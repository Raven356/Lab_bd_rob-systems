using BlogPost.Models.Blogs;
using BlogPostBll.Models;

namespace BlogPost.Mappers
{
    public static class BlogMapper
    {
        public static Blog Map(CreateBlogModel createBlog)
        {
            return new Blog
            {
                AuthorId = createBlog.AuthorId,
                Category = createBlog.Category,
                Text = createBlog.Text,
                Title = createBlog.Title,
            };
        }

        public static Blog Map(EditBlogModel editBlog) 
        { 
            return new Blog
            {
                Title = editBlog.Title,
                Text = editBlog.Text,
                AuthorId = editBlog.AuthorId,
                Category = editBlog.Category,
                Id = editBlog.Id,
            };
        }
    }
}
