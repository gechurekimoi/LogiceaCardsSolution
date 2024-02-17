using Cards.Domain.DTOs;
using Cards.Domain.Enums;
using Cards.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.IServices
{
    public interface ICardService
    {
        Task<BaseResponseDTO<Card>> AddNewCard(Card card, Guid UserId);
        Task<BaseResponseDTO<IEnumerable<Card>>> GetAllCards(UserRoleEnum userRole, Guid? UserId);
        Task<BaseResponseDTO<Card>> GetCardById(Guid CardId, UserRoleEnum userRole, Guid? UserId);
        Task<BaseResponseDTO<Card>> DeleteCard(Guid CardId, UserRoleEnum userRole, Guid? UserId);
        Task<BaseResponseDTO<Card>> UpdateCardDetails(Guid CardId, Card card, UserRoleEnum userRole, Guid? UserId);
    }
}
