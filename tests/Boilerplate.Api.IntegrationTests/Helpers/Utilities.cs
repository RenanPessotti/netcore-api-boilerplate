using System;
using System.Collections.Generic;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Api.IntegrationTests.Helpers
{
    public static class Utilities
    {
        public static void InitializeDbForTests(PersonDbContext db)
        {
            db.Persons.AddRange(GetSeedingPersons());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(PersonDbContext db)
        {
            db.Persons.RemoveRange(db.Persons);
            InitializeDbForTests(db);
        }

        public static List<Person> GetSeedingPersons()
        {
            return new()
            {
                new(new Guid("824a7a65-b769-4b70-bccb-91f880b6ddf1")) { Name = "Test Male", Sex = Sex.Male },
                new(new Guid("b426070e-ccb3-42e6-8fb4-ef6aa5a62cc4")) { Name = "Female Test", Sex = Sex.Female },
                new(new Guid("634769f7-a7b8-4146-9cb2-ff2dd90e886b")) { Name = "Male Test", Sex = Sex.Male }
            };
        }

        public static WebApplicationFactory<Startup> BuildApplicationFactory(this WebApplicationFactory<Startup> factory)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    var sp = services.BuildServiceProvider();

                    using (var scope = sp.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices.GetRequiredService<PersonDbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<WebApplicationFactory<Startup>>>();

                        db.Database.EnsureCreated();

                        try
                        {
                            InitializeDbForTests(db);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding the " +
                                                "database with test messages. Error: {Message}", ex.Message);
                        }
                    }
                });
            });
        }


        public static WebApplicationFactory<Startup> RebuildDb(this WebApplicationFactory<Startup> factory)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var scopedServices = scope.ServiceProvider;
                        var db = scopedServices
                            .GetRequiredService<PersonDbContext>();
                        var logger = scopedServices
                            .GetRequiredService<ILogger<IntegrationTest>>();
                        try
                        {
                            ReinitializeDbForTests(db);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding " +
                                                "the database with test messages. Error: {Message}",
                                ex.Message);
                        }
                    }
                });
            });
        }
    }
}
