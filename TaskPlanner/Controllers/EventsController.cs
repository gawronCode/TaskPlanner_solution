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


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetUserEvents()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);

            var events = await _eventRepo.GetAllByUserIdAsync(user.Id);
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult> Create(EventDto eventDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);

            var e = new Event
            {
                Content = eventDto.Content,
                EventDate = eventDto.EventDate,
                UserId = user.Id
            };

            e.EventDate = e.EventDate.Value.AddHours(2);

            await _eventRepo.CreateAsync(e);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(EventDto eventDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);
            var e = await _eventRepo.GetByIdAsync(eventDto.Id);

            if (e.UserId != user.Id) return Unauthorized();

            await _eventRepo.DeleteAsync(e);

            return Ok();
        }


    }
}
