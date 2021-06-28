using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Boilerplate.Application.DTOs;
using Boilerplate.Application.DTOs.Person;
using Boilerplate.Application.Extensions;
using Boilerplate.Application.Filters;
using Boilerplate.Application.Interfaces;
using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Application.Services
{
    public class PersonAppService : IPersonAppService
    {
        private readonly IPersonRepository _PersonRepository;

        private readonly IMapper _mapper;

        public PersonAppService(IMapper mapper, IPersonRepository PersonRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _PersonRepository = PersonRepository ?? throw new ArgumentNullException(nameof(PersonRepository));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) _PersonRepository.Dispose();
        }

        #region Person Methods

        public async Task<PaginatedList<GetPersonDto>> GetAllPersons(GetPersonsFilter filter)
        {
            filter ??= new GetPersonsFilter();
            var Persons = _PersonRepository
                .GetAll()
                .WhereIf(!string.IsNullOrEmpty(filter.Name), x => EF.Functions.Like(x.Name, $"%{filter.Name}%"))
                .WhereIf(!string.IsNullOrEmpty(filter.Nickname), x => EF.Functions.Like(x.Nickname, $"%{filter.Nickname}%"))
                .WhereIf(filter.Age != null, x => x.Age == filter.Age)
                .WhereIf(filter.Sex != null, x => x.Sex == filter.Sex)
                .WhereIf(!string.IsNullOrEmpty(filter.Job), x => EF.Functions.Like(x.Job, $"%{filter.Job}%"));
            return await _mapper.ProjectTo<GetPersonDto>(Persons).ToPaginatedListAsync(filter.CurrentPage, filter.PageSize);
        }

        public async Task<GetPersonDto> GetPersonById(Guid id)
        {
            return _mapper.Map<GetPersonDto>(await _PersonRepository.GetById(id));
        }

        public async Task<GetPersonDto> CreatePerson(InsertPersonDto Person)
        {
            var created = _PersonRepository.Create(_mapper.Map<Person>(Person));
            await _PersonRepository.SaveChangesAsync();
            return _mapper.Map<GetPersonDto>(created);
        }

        public async Task<GetPersonDto> UpdatePerson(Guid id, UpdatePersonDto updatedPerson)
        {
            var originalPerson = await _PersonRepository.GetById(id);
            if (originalPerson == null) return null;

            originalPerson.Name = updatedPerson.Name;
            originalPerson.Nickname = updatedPerson.Nickname;
            originalPerson.Job = updatedPerson.Job;
            originalPerson.Age = updatedPerson.Age;
            originalPerson.Sex = updatedPerson.Sex;
            _PersonRepository.Update(originalPerson);
            await _PersonRepository.SaveChangesAsync();
            return _mapper.Map<GetPersonDto>(originalPerson);
        }

        public async Task<bool> DeletePerson(Guid id)
        {
           await _PersonRepository.Delete(id);
           return await _PersonRepository.SaveChangesAsync() > 0;
        }

        #endregion
    }
}
