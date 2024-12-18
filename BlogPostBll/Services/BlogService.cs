using BlogPostBll.Context;
using BlogPostBll.Enums;
using BlogPostBll.Interfaces;
using BlogPostBll.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogPostBll.Services
{
    public class BlogService : IBlogService
    {
        private readonly BlogPostContext context;

        public BlogService(BlogPostContext context)
        {
            this.context = context;
        }

        public async Task<Guid> CreateBlogAsync(Blog newBlog)
        {
            var blogs = context.GetCollection<BsonDocument>("blogs");

            var blogDocument = new BsonDocument
            {
                { "_id", Guid.NewGuid().ToString() },
                { "Title", newBlog.Title },
                { "Text", newBlog.Text },
                { "Category", newBlog.Category.ToString() },
                { "AuthorId", newBlog.AuthorId },
                { "CreatedAt", DateTime.UtcNow }
            };

            await blogs.InsertOneAsync(blogDocument);

            var insertedId = blogDocument["_id"];
            return Guid.Parse(insertedId.ToString());
        }

        public async Task DeleteBlogAsync(Guid id)
        {
            var blogs = context.GetCollection<BsonDocument>("blogs");

            var blogDocument = await(await blogs.FindAsync(new BsonDocument("_id", id.ToString()))).FirstOrDefaultAsync();

            await blogs.DeleteOneAsync(blogDocument);
        }

        public async Task<Guid> EditBlogAsync(Blog editBlog)
        {
            var blogs = context.GetCollection<BsonDocument>("blogs");

            var blogDocument = await (await blogs.FindAsync(new BsonDocument("_id", editBlog.Id.ToString()))).FirstOrDefaultAsync();

            if (blogDocument == null)
            {
                throw new KeyNotFoundException($"Blog with ID {editBlog.Id} not found.");
            }

            var updateDefinition = Builders<BsonDocument>.Update
                .Set("Title", editBlog.Title)
                .Set("Text", editBlog.Text)
                .Set("Category", editBlog.Category.ToString())
                .Set("AuthorId", editBlog.AuthorId)
                .Set("CreatedAt", DateTime.UtcNow);

            var result = await blogs.UpdateOneAsync(
                new BsonDocument("_id", editBlog.Id.ToString()),
                updateDefinition
            );

            if (result.ModifiedCount == 0)
            {
                throw new InvalidOperationException("Blog update failed.");
            }

            return editBlog.Id;
        }

        public async Task<IEnumerable<Blog>> GetAllAsync()
        {
            var blogs = context.GetCollection<BsonDocument>("blogs");

            var blogDocuments = await blogs.Find(new BsonDocument()).ToListAsync();

            var result = blogDocuments.Select(doc => new Blog
            {
                Id = Guid.Parse(doc["_id"].ToString()),
                Title = doc["Title"].ToString(),
                Text = doc["Text"].ToString(),
                Category = Enum.Parse<CategoryEnum>(doc["Category"].ToString()),
                AuthorId = int.Parse(doc["AuthorId"].ToString())
            });

            return result;
        }

        public async Task<Blog> GetByIdAsync(Guid id)
        {
            var blogs = context.GetCollection<BsonDocument>("blogs");

            var blogDocument = await (await blogs.FindAsync(new BsonDocument("_id", id.ToString()))).FirstOrDefaultAsync();

            if (blogDocument == null)
            {
                throw new KeyNotFoundException($"Blog with ID {id} not found.");
            }

            var blog = new Blog
            {
                Id = Guid.Parse(blogDocument["_id"].ToString()),
                Title = blogDocument["Title"].ToString(),
                Text = blogDocument["Text"].ToString(),
                Category = Enum.Parse<CategoryEnum>(blogDocument["Category"].ToString()),
                AuthorId = int.Parse(blogDocument["AuthorId"].ToString()),
            };

            return blog;
        }
    }
}
