using Cards.Domain.IRepository;
using Cards.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Infrastructure.Repository
{
    public class CardsRepository : ICardsRepository
    {
        private readonly CardsDbContext _context;

        public CardsRepository(CardsDbContext context)
        {
            _context = context;
        }
        public async Task<Card> AddCard(Card card)
        {
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<bool> Delete(Guid Id)
        {
            var card = await _context.Cards.FindAsync(Id);

            if (card != null)
            {
                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Card>> GetAllCardsAdmin()
        {
            return await _context.Cards.OrderBy(p => p.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Card>> GetAllCardsMember(Guid Id)
        {
            return await _context.Cards.Where(p => p.UserId == Id).OrderBy(p => p.CreatedAt).ToListAsync();
        }

        public async Task<Card> Update(Card card, Guid ModifiedById)
        {
            card.ModifiedAt = DateTime.Now;
            card.ModifiedBy = ModifiedById;
            _context.Entry(card).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<Card> GetCardById(Guid Id)
        {
            return await _context.Cards.Where(p => p.Id == Id).FirstOrDefaultAsync();
        }
    }
}
