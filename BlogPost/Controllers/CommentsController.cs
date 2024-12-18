using BlogPost.Models.Comments;
using BlogPostBll.Interfaces;
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

        [HttpPost]
        public async Task<IActionResult> Create(Guid id, string text)
        {
            await commentsService.CreateAsync(id, text, 1);

            return RedirectToAction("Details", controllerName: "Blogs", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] CommentEditModel commentEdit)
        {
            await commentsService.EditAsync(commentEdit.CommentId, commentEdit.Text);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromQuery] Guid commentId)
        {
            await commentsService.DeleteCommentByIdAsync(commentId);

            return Ok();
        }
    }
}
