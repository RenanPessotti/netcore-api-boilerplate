using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Person;
using Boilerplate.Application.Filters;

namespace Boilerplate.Application.Interfaces
{
    public interface IPersonAppService : IDisposable
    {
        #region Person Methods

        public Task<PaginatedList<GetPersonDto>> GetAllPersons(GetPersonsFilter filter);

        public Task<GetPersonDto> GetPersonById(Guid id);

        public Task<GetPersonDto> CreatePerson(InsertPersonDto Person);

        public Task<GetPersonDto> UpdatePerson(Guid id, UpdatePersonDto updatedPerson);

        public Task<bool> DeletePerson(Guid id);

        #endregion
    }
}