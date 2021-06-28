using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Infrastructure.Context;
using Boilerplate.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Boilerplate.Api.UnitTests
{
    public class PersonRepositoryTests
    {
        private PersonDbContext CreateDbContext(string name)
        {
            var options = new DbContextOptionsBuilder<PersonDbContext>()
            .UseInMemoryDatabase(name)
            .Options;
            return new PersonDbContext(options);
        }

        [Theory]
        [InlineData("4e1a20db-0533-4838-bd97-87d2afc89832")]
        [InlineData("ff57101b-d9c6-4b8a-959e-2d64c7ae8967")]
        [InlineData("2c0176d6-47d6-4ce1-b5e8-bed9a52b9e64")]
        [InlineData("bf15a502-37db-4d4c-ba4c-e231fb5487e6")]
        [InlineData("e141a755-98d4-44d3-a84f-528e6e75f543")]
        public async Task GetById_existing_Persons(Guid id)
        {
            // Arrange

            using (var context = CreateDbContext("GetById_existing_Persons"))
            {
                context.Set<Person>().Add(new Person(id));
                await context.SaveChangesAsync();
            }
            Person Person = null;

            // Act
            using (var context = CreateDbContext("GetById_existing_Persons"))
            {
                var repository = new PersonRepository(context);
                Person = await repository.GetById(id);
            }
            // Assert
            Person.Should().NotBeNull();
            Person.Id.Should().Be(id);
        }

        [Theory]
        [InlineData("4e1a20db-0533-4838-bd97-87d2afc89832")]
        [InlineData("ff57101b-d9c6-4b8a-959e-2d64c7ae8967")]
        [InlineData("2c0176d6-47d6-4ce1-b5e8-bed9a52b9e64")]
        [InlineData("bf15a502-37db-4d4c-ba4c-e231fb5487e6")]
        [InlineData("e141a755-98d4-44d3-a84f-528e6e75f543")]
        public async Task GetById_inexistent_Persons(Guid id)
        {
            // Arrange
            using (var context = CreateDbContext("GetById_inexisting_Persons"))
            {
                context.Set<Person>().Add(new Person(id));
                await context.SaveChangesAsync();
            }
            Person Person = null;

            // Act
            using (var context = CreateDbContext("GetById_inexisting_Persons"))
            {
                var repository = new PersonRepository(context);
                Person = await repository.GetById(Guid.NewGuid());
            }
            // Assert
            Person.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetAll_Persons(int count)
        {
            // Arrange
            using (var context = CreateDbContext($"GetAll_with_Persons_{count}"))
            {
                for (var i = 0; i < count; i++) context.Set<Person>().Add(new Person(new Guid()));
                await context.SaveChangesAsync();
            }
            List<Person> Persons = null;
            // Act
            using (var context = CreateDbContext($"GetAll_with_Persons_{count}"))
            {
                var repository = new PersonRepository(context);
                Persons = await repository.GetAll().ToListAsync();
            }
            // Assert
            Persons.Should().NotBeNull();
            Persons.Count.Should().Be(count);
        }

        [Fact]
        public async Task Create_Person()
        {
            // Arrange
            int result;


            // Act
            var Person = new Person(new Guid())
            {
                Age = 21,
                Name = "Test Male",
                Nickname = "TM",
                Job = "Journalist",
                Sex = Sex.Male
            };

            using (var context = CreateDbContext("Create_Person"))
            {
                var repository = new PersonRepository(context);
                repository.Create(Person);
                result = await repository.SaveChangesAsync();
            }


            // Assert
            result.Should().BeGreaterThan(0);
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Create_Person"))
            {
                (await context.Persons.CountAsync()).Should().Be(1);
                (await context.Persons.FirstAsync()).Should().BeEquivalentTo(Person);
            }
        }

        [Fact]
        public async Task Update_Person()
        {
            // Arrange
            int result;
            Guid? id;
            using (var context = CreateDbContext("Update_Person"))
            {
                var createdPerson = new Person(new Guid())
                {
                    Age = 21,
                    Name = "Test Male",
                    Nickname = "TM",
                    Job = "Journalist",
                    Sex = Sex.Male
                };
                context.Set<Person>().Add(createdPerson);
                context.Set<Person>().Add(new Person(new Guid()) { Name = "Another Person", Sex = Sex.Female, Age = 17 });
                await context.SaveChangesAsync();
                id = createdPerson.Id; //receive autogenerated guid to get the entity later
            }

            // Act

            Person updatePerson;
            using (var context = CreateDbContext("Update_Person"))
            {
                updatePerson = await context.Set<Person>().FirstOrDefaultAsync(x => x.Id == id);
                updatePerson.Age = 15;
                updatePerson.Job = "Writer";
                var repository = new PersonRepository(context);
                repository.Update(updatePerson);
                result = await repository.SaveChangesAsync();
            }


            // Assert
            result.Should().BeGreaterThan(0);
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Update_Person"))
            {
                (await context.Persons.FirstAsync(x => x.Id == updatePerson.Id)).Should().BeEquivalentTo(updatePerson);
            }
        }

        [Fact]
        public async Task Delete_Person()
        {
            // Arrange
            int result;
            Guid? id;
            using (var context = CreateDbContext("Delete_Person"))
            {
                var createdPerson = new Person(new Guid())
                {
                    Age = 21,
                    Name = "Test Male",
                    Nickname = "TM",
                    Job = "Journalist",
                    Sex = Sex.Male
                };
                context.Set<Person>().Add(createdPerson);
                context.Set<Person>().Add(new Person(new Guid()) { Name = "Another Person", Sex = Sex.Female, Age = 17 });
                await context.SaveChangesAsync();
                id = createdPerson.Id; //receive autogenerated guid to get the entity later
            }

            // Act
            using (var context = CreateDbContext("Delete_Person"))
            {
                var repository = new PersonRepository(context);
                await repository.Delete(id.Value);
                result = await repository.SaveChangesAsync();
            }


            // Assert
            result.Should().BeGreaterThan(0);
            result.Should().Be(1);
            // Simulate access from another context to verifiy that correct data was saved to database
            using (var context = CreateDbContext("Delete_Person"))
            {
                (await context.Set<Person>().FirstOrDefaultAsync(x => x.Id == id)).Should().BeNull();
                (await context.Set<Person>().ToListAsync()).Should().NotBeEmpty();
            }
        }

        [Fact]
        public void NullDbContext_Throws_ArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new PersonRepository(null);
            });
            exception.Should().NotBeNull();
            exception.ParamName.Should().Be("dbContext");
        }
    }
}
