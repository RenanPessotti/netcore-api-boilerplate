using System;
using System.ComponentModel.DataAnnotations;
using Boilerplate.Domain.Core.Entities;
using Boilerplate.Domain.Entities.Enums;
using Boilerplate.Domain.Interfaces.Entities;

namespace Boilerplate.Domain.Entities
{
    public class Hero : Entity, IHero
    {
        public Hero(Guid id) : base(id) { }

        [Required]
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string Individuality { get; set; }
        public int? Age { get; set; }

        [Required]
        public HeroType? HeroType { get; set; }

        public string Team { get; set; }
    }
}
