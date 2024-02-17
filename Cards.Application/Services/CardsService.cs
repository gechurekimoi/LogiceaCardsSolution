
using Cards.Domain.DTOs;
using Cards.Domain.Enums;
using Cards.Domain.IRepository;
using Cards.Domain.IServices;
using Cards.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Application.Services
{
    public class CardsService : ICardService
    {
        private readonly ICardsRepository _cardsRepository;

        public CardsService(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }

        public async Task<BaseResponseDTO<Card>> AddNewCard(Card card,Guid UserId)
        {

            //we do some validations here 
            if (string.IsNullOrEmpty(card.Name))
                return new BaseResponseDTO<Card>() { IsSuccessful = false, ResponseMessage = "Card Name is required" };

            if (!string.IsNullOrEmpty(card.Color))
            {
                if(card.Color.Length != 6 || !card.Color.StartsWith("#"))
                {
                    return new BaseResponseDTO<Card>() { IsSuccessful = false, ResponseMessage = "Card Color Must start with # and Must be equal to 6 digits" };
                }
            }
               
            card.Id = Guid.NewGuid();
            card.UserId = UserId;
            card.Status = (int)StatusEnum.ToDo;
            card.CreatedAt = DateTime.Now;
            card.CreatedBy = UserId;
            card.ModifiedAt = null;
            card.ModifiedBy = null;

            var newCard = await _cardsRepository.AddCard(card);

            return new BaseResponseDTO<Card>() { IsSuccessful = true, ResponseMessage = "", Data = newCard };
        }

        public async Task<BaseResponseDTO<IEnumerable<Card>>> GetAllCards(UserRoleEnum userRole, Guid? UserId)
        {

            IEnumerable<Card> allCards = new List<Card>();

            if(UserRoleEnum.Admin == userRole)
            {
                allCards = await _cardsRepository.GetAllCardsAdmin();
            }
            else
            {
                allCards = await _cardsRepository.GetAllCardsMember(UserId.Value);
            }
           

            return new BaseResponseDTO<IEnumerable<Card>>() { IsSuccessful = true, ResponseMessage = "", Data = allCards };
        }

        public async Task<BaseResponseDTO<Card>> GetCardById(Guid CardId,UserRoleEnum userRole, Guid? UserId)
        {

            var card = await _cardsRepository.GetCardById(CardId);

            if (UserRoleEnum.Admin == userRole)
            {
                return new BaseResponseDTO<Card>() { IsSuccessful = true, ResponseMessage = "", Data = card };
            }
            else
            {
                if(card.UserId == UserId.Value)
                {
                    return new BaseResponseDTO<Card>() { IsSuccessful = true, ResponseMessage = "", Data = card };
                }
                else
                {
                    return new BaseResponseDTO<Card>() { IsSuccessful = false, ResponseMessage = "You do not have access to this card", Data = null };
                }
            }
        }

        public async Task<BaseResponseDTO<Card>> DeleteCard(Guid CardId, UserRoleEnum userRole, Guid? UserId)
        {

            var card = await _cardsRepository.GetCardById(CardId);

            if (UserRoleEnum.Admin == userRole)
            {
                var x = await _cardsRepository.Delete(card.Id);

                return new BaseResponseDTO<Card>() { IsSuccessful = true, ResponseMessage = "", Data = card };
            }
            else
            {
                if (card.UserId == UserId.Value)
                {
                    var x = await _cardsRepository.Delete(card.Id);

                    return new BaseResponseDTO<Card>() { IsSuccessful = true, ResponseMessage = "", Data = card };
                }
                else
                {
                    return new BaseResponseDTO<Card>() { IsSuccessful = false, ResponseMessage = "You do not have access to this card", Data = null };
                }
            }
        }

        public async Task<BaseResponseDTO<Card>> UpdateCardDetails(Guid CardId, Card card, UserRoleEnum userRole, Guid? UserId)
        {

            if (string.IsNullOrEmpty(card.Name))
                return new BaseResponseDTO<Card>() { IsSuccessful = false, ResponseMessage = "Card Name is required" };

            if (!string.IsNullOrEmpty(card.Color))
            {
                if (card.Color.Length != 6 || !card.Color.StartsWith("#"))
                {
                    return new BaseResponseDTO<Card>() { IsSuccessful = false, ResponseMessage = "Card Color Must start with # and Must be equal to 6 digits" };
                }
            }

            var cardToUpdate = await _cardsRepository.GetCardById(CardId);

            if (UserRoleEnum.Admin == userRole)
            {
                cardToUpdate.ModifiedAt = DateTime.Now;
                cardToUpdate.ModifiedBy = UserId.Value;
                cardToUpdate.Name = card.Name;
                cardToUpdate.Description = card.Description;
                cardToUpdate.Color = card.Color;

                var x = await _cardsRepository.Update(cardToUpdate, UserId.Value);

                return new BaseResponseDTO<Card>() { IsSuccessful = true, ResponseMessage = "", Data = card };
            }
            else
            {
                if (card.UserId == UserId.Value)
                {
                    cardToUpdate.ModifiedAt = DateTime.Now;
                    cardToUpdate.ModifiedBy = UserId.Value;
                    cardToUpdate.Name = card.Name;
                    cardToUpdate.Description = card.Description;
                    cardToUpdate.Color = card.Color;

                    var x = await _cardsRepository.Update(cardToUpdate, UserId.Value);

                    return new BaseResponseDTO<Card>() { IsSuccessful = true, ResponseMessage = "", Data = card };
                }
                else
                {
                    return new BaseResponseDTO<Card>() { IsSuccessful = false, ResponseMessage = "You do not have access to this card", Data = null };
                }
            }
        }


    }
}
