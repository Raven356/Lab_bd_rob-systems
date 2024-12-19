using BlogPost.Models.Comments;
using BlogPostBll.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogPost.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(Guid id, string text)
        {
            await commentsService.CreateAsync(id, text, 1);

            return RedirectToAction("Details", controllerName: "Blogs", new { id });
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] CommentEditModel commentEdit)
        {
            await commentsService.EditAsync(commentEdit.CommentId, commentEdit.Text);

            return Ok();
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete([FromQuery] Guid commentId)
        {
            await commentsService.DeleteCommentByIdAsync(commentId);

            return Ok();
        }
    }
}
