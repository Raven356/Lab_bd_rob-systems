﻿using BlogPostBll.Context;
using BlogPostBll.Interfaces;
using BlogPostBll.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogPostBll.Services
{
    public class UserService : IUserService
    {
        private readonly BlogPostContext context;

        public UserService(BlogPostContext context)
        {
            this.context = context;
        }

        public async Task<bool> LoginAsync(User user)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var users = context.GetCollection<BsonDocument>("users");

            var filter = Builders<BsonDocument>.Filter.Eq("Email", user.Email);

            var userDocument = await users.Find(filter).FirstOrDefaultAsync();

            if (userDocument == null)
            {
                return false;
            }

            string storedHash = userDocument["Password"].AsString;

            if (BCrypt.Net.BCrypt.Verify(user.Password, storedHash))
            {
                return true;
            }

            return false;
        }

        public async Task RegisterAsync(User user)
        {
            var users = context.GetCollection<BsonDocument>("users");
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var userDocument = new BsonDocument
            {
                { "_id", Guid.NewGuid().ToString() },
                { "Email", user.Email },
                { "Password", hashedPassword },
                { "CreatedAt", DateTime.UtcNow }
            };

            await users.InsertOneAsync(userDocument);
        }
    }
}