using BlogPostBll.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogPostBll
{
    public static class Setup
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(provider =>
            {
                var connectionString = configuration.GetConnectionString("MongoDbConnection");
                var databaseName = configuration["DatabaseName"];
                return new BlogPostContext(connectionString, databaseName);
            });

        }
    }
}
