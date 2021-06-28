using System.ComponentModel.DataAnnotations;
using Boilerplate.Domain.Entities.Enums;

namespace Boilerplate.Application.DTOs.Person
{
    public class InsertPersonDto
    {
        [Required(ErrorMessage = "É necessário informar o nome do herói")]
        public string Name { get; set; }

        public string Nickname { get; set; }
        public int? Age { get; set; }
        public string Job { get; set; }

        [Required(ErrorMessage = "É necessário informar o tipo do herói")]
        public Sex? Sex { get; set; }
    }
}
