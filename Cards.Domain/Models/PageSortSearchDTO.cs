using Cards.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.Models
{
    public class PageSortSearchDTO
    {
        public string searchTerm { get; set; }
        public string searchByValue { get; set; }
        public string sortbyTerm { get; set; }
        public SortDirectionEnum sortbyDirection { get; set; } = SortDirectionEnum.Asc;
        public int PageSize { get; set; } = 10;
        public int PageIndex { get; set; } = 1;

    }
}
