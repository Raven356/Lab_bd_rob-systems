using BlogPost.Mappers;
using BlogPost.Models.Blogs;
using BlogPostBll.Enums;
using BlogPostBll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPost.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService blogService;
        private readonly ICommentsService commentsService;

        public BlogsController(IBlogService blogService, ICommentsService commentsService)
        {
            this.blogService = blogService;
            this.commentsService = commentsService;
        }

        public async Task<IActionResult> Index(string? name, string? category)
        {
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));

            var blogs = await blogService.GetAllAsync(name, category);

            return View(blogs);
        }

        //[Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));
            return View();
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogModel createBlogModel)
        {
            var newBlogId = await blogService.CreateBlogAsync(BlogMapper.Map(createBlogModel));
            return RedirectToAction("Details", new { id = newBlogId });
        }

        //[Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            var blog = await blogService.GetByIdAsync(id);
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));

            var viewModel = new EditBlogModel
            {
                AuthorId = blog.AuthorId,
                Category = blog.Category,
                Id = id,
                Text = blog.Text,
                Title = blog.Title
            };

            return View(viewModel);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogModel editBlogModel)
        {
            var id = await blogService.EditBlogAsync(BlogMapper.Map(editBlogModel));

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
                Comments = comments
            });
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) 
        {
            await blogService.DeleteBlogAsync(id);

            return RedirectToAction("Index", controllerName: "Blogs");
        }
    }
}
