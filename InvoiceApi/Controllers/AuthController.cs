using InvoiceApi.Data;
using InvoiceApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace InvoiceApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public static UserModel userModel = new UserModel();


        [HttpPost("user/register")]
        public async Task<ActionResult<UserModel>>Register(UserModel request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userModel.FirstName = request.FirstName;
            userModel.LastName = request.LastName;
            userModel.TC = request.TC;
            userModel.Mail = request.Mail;
            userModel.PasswordHash = passwordHash;
            userModel.PasswordSalt = passwordSalt;

            _context.UserModels.Add(userModel);

            await _context.SaveChangesAsync();

            return Ok(userModel); 
        }

        [HttpPost("user/login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if (userModel.Mail != request.Mail)
            {
                return BadRequest("User not Found");
            }

            if(!VerifyPasswordHash(request.Password, userModel.PasswordHash, userModel.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(userModel);

            return Ok(token);
        }

        private string CreateToken(UserModel userModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userModel.Mail)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }

}
