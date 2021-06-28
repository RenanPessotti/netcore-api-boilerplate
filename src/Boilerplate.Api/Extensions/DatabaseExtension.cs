using Boilerplate.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Api.Extensions
{
    public static class DatabaseExtension
    {
        public static void AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<HeroDbContext>(options =>
                    options.UseInMemoryDatabase("HeroesDb"));
            }
            else
            {
                if (environment?.EnvironmentName == "Testing")
                    services.AddDbContextPool<HeroDbContext>(o =>
                    {
                        o.UseSqlite("Data Source=test.db");
                    });
                else
                    services.AddDbContextPool<HeroDbContext>(o =>
                    {
                        o.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                    });
            }
        }
    }
}
