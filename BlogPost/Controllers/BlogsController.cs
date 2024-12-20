using BlogPost.Mappers;
using BlogPost.Models.Blogs;
using BlogPostBll.Enums;
using BlogPostBll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogPost.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService blogService;
        private readonly ICommentsService commentsService;
        private readonly IUserService userService;

        public BlogsController(IBlogService blogService, ICommentsService commentsService, IUserService userService)
        {
            this.blogService = blogService;
            this.commentsService = commentsService;
            this.userService = userService;
        }

        public async Task<IActionResult> Index(string? name, string? category)
        {
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));

            var blogs = await blogService.GetAllAsync(name, category);

            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBlogModel createBlogModel)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var userId = await userService.GetUserIdByEmailAsync(userEmail);

            var blog = BlogMapper.Map(createBlogModel);
            blog.AuthorId = userId;

            var newBlogId = await blogService.CreateBlogAsync(blog);
            return RedirectToAction("Details", new { id = newBlogId });
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var blog = await blogService.GetByIdAsync(id);
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));

            var viewModel = new EditBlogModel
            {
                Category = blog.Category,
                Id = id,
                Text = blog.Text,
                Title = blog.Title
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditBlogModel editBlogModel)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var userId = await userService.GetUserIdByEmailAsync(userEmail);

            var blog = BlogMapper.Map(editBlogModel);
            blog.AuthorId = userId;

            var id = await blogService.EditBlogAsync(blog);

            return RedirectToAction("Details", new { id });
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var blog = await blogService.GetByIdAsync(id);
            var comments = await commentsService.GetCommentsForBlog(id);

            return View(new DetailsBlogModel
            {
                Id = id,
                Category = blog.Category,
                Text = blog.Text,
                Title = blog.Title,
                Comments = comments,
                AuthorId = blog.AuthorId
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) 
        {
            await blogService.DeleteBlogAsync(id);

            return RedirectToAction("Index", controllerName: "Blogs");
        }
    }
}
