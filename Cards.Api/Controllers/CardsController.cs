using Cards.Application.Services;
using Cards.Domain.Enums;
using Cards.Domain.IServices;
using Cards.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cards.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        // GET: api/<CardsController>
        [HttpGet]
        public async Task<IActionResult> Get(string searchTerm = "", string searchByValue = "", string sortbyTerm = "", SortDirectionEnum sortbyDirection = SortDirectionEnum.Asc, int PageSize = 10, int PageIndex = 1)
        {
            Guid UserId = GetUserId();

            if (UserId == Guid.Empty)
                return Unauthorized();

            var userRole = GetUserRole();

            var cards = await _cardService.GetAllCards(userRole, UserId);

            if (cards.IsSuccessful == false)
            {
                return BadRequest(cards.ResponseMessage);
            }
            else
            {


                var cardsList = cards.Data;


                //search
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.Trim().ToLower();

                    switch (searchTerm)
                    {
                        case "name":
                            cardsList = cardsList.Where(p => p.Name.ToLower().Trim().Contains(searchByValue.ToLower())).ToList();
                            break;

                        case "description":
                            cardsList = cardsList.Where(p => p.Description.ToLower().Trim().Contains(searchByValue.ToLower())).ToList();
                            break;

                        case "color":
                            cardsList = cardsList.Where(p => p.Color.ToLower().Trim().Contains(searchByValue.ToLower())).ToList();
                            break;

                        default:

                            break;
                    }
                }

                //sort
                if (!string.IsNullOrEmpty(sortbyTerm))
                {
                    string sortTerm = sortbyTerm.Trim().ToLower();

                    switch (sortTerm)
                    {
                        case "name":
                            if (sortbyDirection == SortDirectionEnum.Asc)
                            {
                                cardsList = cardsList.OrderBy(p => p.Name).ToList();
                            }
                            else
                            {
                                cardsList = cardsList.OrderByDescending(p => p.Name).ToList();
                            }
                            break;

                        case "color":
                            if (sortbyDirection == SortDirectionEnum.Asc)
                            {
                                cardsList = cardsList.OrderBy(p => p.Color).ToList();
                            }
                            else
                            {
                                cardsList = cardsList.OrderByDescending(p => p.Color).ToList();
                            }
                            break;
                        case "status":
                            if (sortbyDirection == SortDirectionEnum.Asc)
                            {
                                cardsList = cardsList.OrderBy(p => p.Status).ToList();
                            }
                            else
                            {
                                cardsList = cardsList.OrderByDescending(p => p.Status).ToList();
                            }
                            break;

                        case "date":
                            if (sortbyDirection == SortDirectionEnum.Asc)
                            {
                                cardsList = cardsList.OrderBy(p => p.Status).ToList();
                            }
                            else
                            {
                                cardsList = cardsList.OrderByDescending(p => p.Status).ToList();
                            }
                            break;
                        default:
                            break;
                    }
                }

                //pagination
                int previousPage = PageIndex - 1;
                int skip = previousPage * PageSize;
                int take = PageSize;

                cardsList = cardsList.Skip(skip).Take(take).ToList();


                return Ok(cardsList);

            }
        }

        // GET api/<CardsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            Guid UserId = GetUserId();

            if (UserId == Guid.Empty)
                return Unauthorized();

            var userRole = GetUserRole();

            var cardResponse = await _cardService.GetCardById(id, userRole, UserId);

            if (!cardResponse.IsSuccessful)
                return BadRequest(cardResponse.ResponseMessage);

            return Ok(cardResponse.Data);

        }

        // POST api/<CardsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Card card)
        {

            if (card == null)
                return BadRequest("The card body is null");

            Guid UserId = GetUserId();

            if (UserId == Guid.Empty)
                return Unauthorized();

            var newCard = await _cardService.AddNewCard(card, UserId);

            if (newCard == null)
                return BadRequest("The was a problem saving your card details");

            if (newCard != null && newCard.IsSuccessful == false)
                return BadRequest(newCard.ResponseMessage);

            return Ok(newCard.Data);

        }

        // PUT api/<CardsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Card card)
        {
            Guid UserId = GetUserId();

            if (UserId == Guid.Empty)
                return Unauthorized();

            var userRole = GetUserRole();

            var cardResponse = await _cardService.UpdateCardDetails(id, card, userRole, UserId);

            if (!cardResponse.IsSuccessful)
                return BadRequest(cardResponse.ResponseMessage);

            return Ok(cardResponse.Data);
        }

        // DELETE api/<CardsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            Guid UserId = GetUserId();

            if (UserId == Guid.Empty)
                return Unauthorized();

            var userRole = GetUserRole();

            var cardResponse = await _cardService.DeleteCard(id, userRole, UserId);

            if (!cardResponse.IsSuccessful)
                return BadRequest(cardResponse.ResponseMessage);

            return Ok("Deleted Successfully");


        }

        private Guid GetUserId()
        {
            var claims = new List<Claim>();

            if (User.Identity.IsAuthenticated)
            {
                claims = User.Claims.ToList();

                string UserId = claims[0].Value;

                return new Guid(UserId);
            }
            return Guid.Empty;
        }

        private UserRoleEnum GetUserRole()
        {
            var claims = new List<Claim>();

            if (User.Identity.IsAuthenticated)
            {
                claims = User.Claims.ToList();

                string UserRole = claims[3].Value;

                int UserRoleInt = Convert.ToInt32(UserRole);

                return (UserRoleEnum)UserRoleInt;
            }
            return UserRoleEnum.None;
        }
    }
}
