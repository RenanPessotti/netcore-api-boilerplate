using Boilerplate.Domain.Core.Interfaces.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Domain.Core.Entities
{
    public abstract class Entity : IEntity
    {
        protected Entity(Guid id)
        {
            Id = id;
        }

        [Key]
        public Guid Id { get; set; }
    }
}
