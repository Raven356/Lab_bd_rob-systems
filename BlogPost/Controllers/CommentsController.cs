using BlogPost.Models.Comments;
using BlogPostBll.Interfaces;
using BlogPostBll.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogPost.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;
        private readonly IUserService userService;

        public CommentsController(ICommentsService commentsService, IUserService userService)
        {
            this.commentsService = commentsService;
            this.userService = userService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateModel commentCreateModel)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var userId = await userService.GetUserIdByEmailAsync(userEmail);

            await commentsService.CreateAsync(commentCreateModel.BlogId, commentCreateModel.Text, userId);

            return RedirectToAction("Details", controllerName: "Blogs", new { id = commentCreateModel.BlogId });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] CommentEditModel commentEdit)
        {
            await commentsService.EditAsync(commentEdit.CommentId, commentEdit.Text);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete([FromQuery] Guid commentId)
        {
            await commentsService.DeleteCommentByIdAsync(commentId);

            return Ok();
        }
    }
}
