using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TaskPlanner.Models.DbModels;
using TaskPlanner.Models.DtoModels;
using TaskPlanner.Repositories.Interfaces;

namespace TaskPlanner.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {

        private readonly IEventRepo _eventRepo;
        private readonly IUserRepo _userRepo;

        public EventsController(IEventRepo eventRepo,
                                IUserRepo userRepo)
        {
            _eventRepo = eventRepo;
            _userRepo = userRepo;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetAllEvents()
        {
            var events = await _eventRepo.GetAllAsync();
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult> Create(EventDto eventDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);

            await _eventRepo.CreateAsync(new Event
            {
                Content = eventDto.Content,
                EventDate = eventDto.EventDate,
                UserId = user.Id
            });

            return Ok();
        }



    }
}
