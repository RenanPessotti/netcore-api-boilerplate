using Boilerplate.Domain.Entities.Enums;

namespace Boilerplate.Application.Filters
{
    public class GetPersonsFilter : PaginationInfoFilter
    {
        public string Name { get; set; }
        public string Nickname { get; set; }

        public int? Age { get; set; }

        public string Job { get; set; }
        public Sex? Sex { get; set; }
    }
}
