using System;
using Boilerplate.Domain.Entities.Enums;

namespace Boilerplate.Application.DTOs.Person
{
    public class GetPersonDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }

        public int? Age { get; set; }

        public string Job { get; set; }
        public Sex? Sex { get; set; }
    }
}
