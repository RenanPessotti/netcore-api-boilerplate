using System;
using System.ComponentModel.DataAnnotations;
using Boilerplate.Domain.Core.Entities;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Domain.Interfaces.Entities;

namespace Boilerplate.Domain.Entities
{
    public class Person : Entity, IPerson
    {
        public Person(Guid id) : base(id) { }

        [Required]
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Job { get; set; }
        public int? Age { get; set; }

        [Required]
        public Sex? Sex { get; set; }
    }
}
