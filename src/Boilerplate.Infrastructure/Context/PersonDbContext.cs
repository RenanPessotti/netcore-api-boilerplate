using Boilerplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Infrastructure.Context
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
    }
}
