using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces.Repositories;
using Boilerplate.Infrastructure.Context;

namespace Boilerplate.Infrastructure.Repositories
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(PersonDbContext dbContext) : base(dbContext) { }
    }
}

