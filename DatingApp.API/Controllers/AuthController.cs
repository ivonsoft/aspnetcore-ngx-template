using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //z tym contrlerm moga przyjsc nulle, my tego nie chcemy, ale dziala validator w kontrolerze UserForRegisterDto
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repository, IConfiguration config) // DI with config to get token
        {
            this._config = config;
            _repository = repository;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegisterDto userRegister)
        {
            // this is useed together with [FromBody]UserForRegisterDto
            // if(!ModelState.IsValid) return BadRequest("Istnieje taki użytkownik");
            userRegister.Username = userRegister.Username.ToLower();
            if (await _repository.userExist(userRegister.Username))
                return BadRequest("Istnieje taki użytkownik");

            var user = new User
            {
                UserName = userRegister.Username
            };

            var createdUser = await _repository.Register(user,userRegister.Password);
            return StatusCode(201);
            //CreatedAtRoute()
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userLogin)
        {
            // this is useed together with [FromBody]UserForRegisterDto
            // if(!ModelState.IsValid) return BadRequest("Istnieje taki użytkownik");
            var UserFromDB = await _repository.Login(userLogin.Username, userLogin.Password);
            if (UserFromDB == null)
                return Unauthorized(); //BadRequest("Błdne hasło");
            // we build token with two info: Id, username, we dont have to do round back to DB
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, UserFromDB.Id.ToString()),
                new Claim(ClaimTypes.Name, UserFromDB.UserName)
            };
            // uwaga: jezli Appsettings:Token jest za krotki to generuje sie PII blad
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:Token").Value));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDesc = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesc);
            // return StatusCode(201);
            return Ok(new{
                token = tokenHandler.WriteToken(token)
            });
            //CreatedAtRoute()
        }
    }
}