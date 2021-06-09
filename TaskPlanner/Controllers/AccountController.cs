using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using TaskPlanner.Models.DtoModels;
using TaskPlanner.Repositories.Interfaces;
using TaskPlanner.Utilities.Interfaces;

namespace TaskPlanner.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {

        private readonly IUserRepo _userRepo;
        private readonly IHashManager _hashManager;

        public AccountController(IUserRepo userRepo, IHashManager hashManager)
        {
            _userRepo = userRepo;
            _hashManager = hashManager;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var name = (await _userRepo.GetByEmailAsync(email)).Name;
            // Thread.Sleep(5000);
            return Ok(new {name = name, email = email});
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCredentials(UserRegisterDto userRegisterDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);

            user.Name = string.IsNullOrEmpty(userRegisterDto.Name) ? user.Name : userRegisterDto.Name;
            user.Email = string.IsNullOrEmpty(userRegisterDto.Email) ? user.Email : userRegisterDto.Email;
            user.PasswordHash = string.IsNullOrEmpty(userRegisterDto.Password)
                ? user.PasswordHash
                : _hashManager.CreateHash(userRegisterDto.Password);

            await _userRepo.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAccount()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);
            await _userRepo.DeleteAsync(user);
            return NoContent();
        }



    }
}
