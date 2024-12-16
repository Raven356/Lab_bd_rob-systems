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

        public IActionResult Index()
        {
            var blogs = blogService.GetAll();

            return View(blogs);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = Enum.GetValues(typeof(CategoryEnum));
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateBlogModel createBlogModel)
        {
            var newBlogId = blogService.CreateBlog(BlogMapper.Map(createBlogModel));
            return RedirectToAction("Details", new { id = newBlogId });
        }

        public IActionResult Edit(Guid id)
        {
            var blog = blogService.GetById(id);
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
        public IActionResult Edit(EditBlogModel editBlogModel)
        {
            var id = blogService.EditBlog(BlogMapper.Map(editBlogModel));

            return RedirectToAction("Details", new { id });
        }

        public IActionResult Details(int id)
        {
            return View(new DetailsBlogModel { Id = id, Category = CategoryEnum.Robotics, Text = "test"});
        }
    }
}
