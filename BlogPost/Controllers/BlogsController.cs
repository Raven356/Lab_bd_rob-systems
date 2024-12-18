using BlogPost.Mappers;
using BlogPost.Models.Blogs;
using BlogPostBll.Enums;
using BlogPostBll.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogPost.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogService blogService;

        public BlogsController(IBlogService blogService) 
        {
            this.blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await blogService.GetAllAsync();

            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBlogModel createBlogModel)
        {
            var newBlogId = await blogService.CreateBlogAsync(BlogMapper.Map(createBlogModel));
            return RedirectToAction("Details", new { id = newBlogId });
        }

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

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogModel editBlogModel)
        {
            var id = await blogService.EditBlogAsync(BlogMapper.Map(editBlogModel));

            return RedirectToAction("Details", new { id });
        }

        public IActionResult Details(int id)
        {
            return View(new DetailsBlogModel { Id = id, Category = CategoryEnum.Robotics, Text = "test"});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id) 
        {
            await blogService.DeleteBlogAsync(id);

            return RedirectToAction("Index");
        }
    }
}
