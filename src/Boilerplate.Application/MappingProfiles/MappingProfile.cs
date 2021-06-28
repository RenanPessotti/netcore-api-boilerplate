using AutoMapper;
using Boilerplate.Application.DTOs.Person;
using Boilerplate.Domain.Entities;

namespace Boilerplate.Application.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Person Map
            CreateMap<Person, GetPersonDto>().ReverseMap();
            CreateMap<InsertPersonDto, Person>();
            CreateMap<UpdatePersonDto, Person>();
        }
    }
}
