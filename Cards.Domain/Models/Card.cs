using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.Models
{
    public class Card : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int Status { get; set; }
        public Guid UserId { get; set; }
    }
}
