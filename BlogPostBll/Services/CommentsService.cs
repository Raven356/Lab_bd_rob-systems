using BlogPostBll.Context;
using BlogPostBll.Interfaces;
using BlogPostBll.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogPostBll.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly BlogPostContext context;

        public CommentsService(BlogPostContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(Guid postId, string text, int authorId)
        {
            var comments = context.GetCollection<BsonDocument>("comments");

            var commentDocument = new BsonDocument
            {
                { "_id", Guid.NewGuid().ToString() },
                { "Text", text },
                { "AuthorId", authorId },
                { "BlogId", postId.ToString() },
                { "CreatedAt", DateTime.UtcNow }
            };

            await comments.InsertOneAsync(commentDocument);
        }

        public async Task DeleteCommentByIdAsync(Guid commentId)
        {
            var comments = context.GetCollection<BsonDocument>("comments");

            var commentDocument = await(await comments.FindAsync(new BsonDocument("_id", commentId.ToString()))).FirstOrDefaultAsync();

            await comments.DeleteOneAsync(commentDocument);
        }

        public async Task EditAsync(Guid commentId, string text)
        {
            var comments = context.GetCollection<BsonDocument>("comments");

            var comment = await(await comments.FindAsync(new BsonDocument("_id", commentId.ToString()))).FirstOrDefaultAsync();

            if (comment == null)
            {
                throw new KeyNotFoundException($"Blog with ID {commentId} not found.");
            }

            var updateDefinition = Builders<BsonDocument>.Update
                .Set("Text", text)
                .Set("CreatedAt", DateTime.UtcNow);

            var result = await comments.UpdateOneAsync(
                new BsonDocument("_id", commentId.ToString()),
                updateDefinition
            );

            if (result.ModifiedCount == 0)
            {
                throw new InvalidOperationException("Blog update failed.");
            }
        }

        public async Task<Comment> GetCommentById(Guid commentId)
        {
            var comments = context.GetCollection<BsonDocument>("comments");

            var comment = await(await comments.FindAsync(new BsonDocument("_id", commentId.ToString()))).FirstOrDefaultAsync();

            return new Comment
            {
                Id = Guid.Parse(comment["_id"].ToString()),
                AuthorId = int.Parse(comment["AuthorId"].ToString()),
                Text = comment["Text"].ToString(),
                BlogId = Guid.Parse(comment["BlogId"].ToString())
            };
        }

        public async Task<IEnumerable<Comment>> GetCommentsForBlog(Guid blogId)
        {
            var commentsCollection = context.GetCollection<BsonDocument>("comments");

            var filter = new BsonDocument()
            {
                { "BlogId", blogId.ToString() }
            };

            var commentDocuments = await commentsCollection.Find(filter).ToListAsync();

            var result = commentDocuments.Select(doc => new Comment
            {
                Id = Guid.Parse(doc["_id"].ToString()),
                AuthorId = int.Parse(doc["AuthorId"].ToString()),
                Text = doc["Text"].ToString(),
                BlogId = Guid.Parse(doc["BlogId"].ToString())
            });

            return result;
        }

    }
}
