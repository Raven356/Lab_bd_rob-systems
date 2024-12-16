using MongoDB.Driver;

namespace BlogPostBll.Context
{
    public class BlogPostContext
    {
        private readonly IMongoDatabase _database;

        public BlogPostContext(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }
    }
}
