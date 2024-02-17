using Cards.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.IRepository
{
    public interface ICardsRepository
    {
        Task<Card> AddCard(Card card);
        Task<Card> Update(Card card, Guid ModifiedById);
        Task<bool> Delete(Guid Id);
        Task<IEnumerable<Card>> GetAllCardsAdmin();
        Task<IEnumerable<Card>> GetAllCardsMember(Guid Id);
        Task<Card> GetCardById(Guid Id);


    }
}
