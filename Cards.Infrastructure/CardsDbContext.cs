using Cards.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Infrastructure
{
    public class CardsDbContext :DbContext
    {
        public CardsDbContext(DbContextOptions<CardsDbContext> options):base(options)
        {
                
        }
        public DbSet<Card> Cards { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
