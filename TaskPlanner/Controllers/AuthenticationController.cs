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
using TaskPlanner.Utilities.Interfaces;
using Task = TaskPlanner.Models.DbModels.Task;


namespace TaskPlanner.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;
        private readonly IHashManager _hashManager;
        private readonly IUserRepo _userRepo;

        public AuthenticationController(IJwtAuthenticationManager jwtAuthenticationManager, 
                                        IHashManager hashManager,
                                        IUserRepo userRepo)
        {
            _jwtAuthenticationManager = jwtAuthenticationManager;
            _hashManager = hashManager;
            _userRepo = userRepo;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            var email = User.FindFirst(ClaimTypes.Email).Value;
            var name = _userRepo.GetByEmailAsync(email).Result.Name;
            var id = _userRepo.GetByEmailAsync(email).Result.Id.ToString();
            var hash = _userRepo.GetByEmailAsync(email).Result.PasswordHash;

            return new List<string>
            {
                id,
                name,
                email,
                hash
            };
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<TokenDto>> Authenticate([FromBody] UserCredentialsDto userCredentials)
        {
            var token =  await _jwtAuthenticationManager.Authenticate(userCredentials.Email, userCredentials.Password);
            if(string.IsNullOrEmpty(token)) return Unauthorized();
            var name = (await _userRepo.GetByEmailAsync(userCredentials.Email)).Name;
            return Ok(new TokenDto
            {
                User = name,
                Token = token
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterDto userRegister)
        {
            var existingUser = await _userRepo.GetByEmailAsync(userRegister.Email);
            if (existingUser is not null) return Problem();
            var passwordHash = _hashManager.CreateHash(userRegister.Password);
            var user = new User
            {
                Email = userRegister.Email,
                Name = userRegister.Name,
                PasswordHash = passwordHash
            };
            var success = await _userRepo.CreateAsync(user);
            if (success) return Ok();

            return Problem();

        }


        // [HttpGet("{email}" ,Name = "GetNewToken")]
        // public async Task<ActionResult<TokenDto>> GetNewToken(string email)
        // {
        //     var token = _jwtAuthenticationManager.GetToken(email);
        //     var user = await _userRepo.GetByEmailAsync(email);
        //     if (user is null) return Problem();
        //
        //     return Ok(new TokenDto
        //     {
        //         Token = token,
        //         User = user.Name
        //     });
        //
        // }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetTest()
        {
            var users = await _userRepo.GetAllAsync();
            return Ok(users);
        }

    }
}
