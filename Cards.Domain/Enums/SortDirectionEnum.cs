using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.Enums
{
    public enum SortDirectionEnum
    {
        [Description("Ascending")]
        Asc = 0,
        [Description("Descending")]
        Desc = 1
    }
}
