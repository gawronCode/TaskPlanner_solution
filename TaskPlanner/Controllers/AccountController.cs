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
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AccountController(IUserRepo userRepo,
                                IHashManager hashManager,
                                IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userRepo = userRepo;
            _hashManager = hashManager;
            _jwtAuthenticationManager = jwtAuthenticationManager;
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
        public async Task<IActionResult> UpdateName(UserRegisterDto userRegisterDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);

            user.Name = string.IsNullOrEmpty(userRegisterDto.Name) ? user.Name : userRegisterDto.Name;
            //TODO Ogarnąć czemu zapytanie z postmana przechodzi redirecta a z aplikacji angulara nie
            // user.PasswordHash = string.IsNullOrEmpty(userRegisterDto.Password)
            //     ? user.PasswordHash
            //     : _hashManager.CreateHash(userRegisterDto.Password);
            //
            // if (!string.IsNullOrEmpty(userRegisterDto.Email))
            // {
            //     user.Email = userRegisterDto.Email;
            //     await _userRepo.UpdateAsync(user);
            //     // return RedirectToAction("GetNewToken", "Authentication", new { email = userRegisterDto.Email });
            //     return RedirectToAction(nameof(GetNewToken), new { email = userRegisterDto.Email });
            //
            // }
            await _userRepo.UpdateAsync(user);
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult<TokenDto>> UpdateEmail(UserRegisterDto userRegisterDto)
        {
            if(await _userRepo.GetByEmailAsync(userRegisterDto.Email) is not null) return Problem();

            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);
            user.Email = string.IsNullOrEmpty(userRegisterDto.Email) ? user.Email : userRegisterDto.Email;
            await _userRepo.UpdateAsync(user);
            var token = _jwtAuthenticationManager.GetToken(user.Email);
            return Ok(new TokenDto
            {
                Token = token,
                User = user.Name
            });

        }

        [HttpPut]
        public async Task<ActionResult> UpdatePassword(UserRegisterDto userRegisterDto)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userRepo.GetByEmailAsync(email);

            user.PasswordHash = string.IsNullOrEmpty(userRegisterDto.Password)
                ? user.PasswordHash
                : _hashManager.CreateHash(userRegisterDto.Password);

            await _userRepo.UpdateAsync(user);
            return Ok();
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
