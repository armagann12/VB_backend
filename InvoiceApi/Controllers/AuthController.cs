using InvoiceApi.Data;
using InvoiceApi.Models;
using Microsoft.AspNetCore.Authorization;
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


        [AllowAnonymous]
        [HttpPost("user/register")]
        public async Task<ActionResult<UserModel>>RegisterUser(UserModel request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            UserModel userModel = new UserModel();

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

        [AllowAnonymous]
        [HttpPost("institution/register")]
        public async Task<ActionResult<InstitutionModel>>RegisterInstitution(InstitutionModel request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            InstitutionModel institutionModel = new InstitutionModel();
            
            institutionModel.Name = request.Name;
            institutionModel.Detail = request.Detail;
            institutionModel.Mail = request.Mail;
            institutionModel.PasswordHash = passwordHash;
            institutionModel.PasswordSalt = passwordSalt;
            

            _context.InstitutionModels.Add(institutionModel);

            await _context.SaveChangesAsync();
            
            
            return Ok(institutionModel);
        }

        [AllowAnonymous]
        [HttpPost("user/login")]
        public async Task<ActionResult<string>> UserLogin(UserDto request)
        {
            var dbUser = _context.UserModels.Where(u => u.Mail == request.Mail).FirstOrDefault();

            if (dbUser == null)
            {
                return BadRequest("User not Found");
            }

            if(!VerifyPasswordHash(request.Password, dbUser.PasswordHash, dbUser.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateUserToken(dbUser);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("institution/login")]
        public async Task<ActionResult<string>> InstitutionLogin(InstitutionDto request) //dto
        {
            var dbinst = _context.InstitutionModels.Where(u => u.Mail == request.Mail).FirstOrDefault();
            if (dbinst == null)
            {
                return BadRequest("Institution not Found");
            }

            if (!VerifyPasswordHash(request.Password, dbinst.PasswordHash, dbinst.PasswordSalt))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateInstitutionToken(dbinst);


            return Ok(token);
        }

        private string CreateInstitutionToken(InstitutionModel institutionModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, institutionModel.Mail),
                new Claim(ClaimTypes.Role, "Institution")
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

        private string CreateUserToken(UserModel userModel)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, userModel.Mail),
                new Claim(ClaimTypes.Role, "User")
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
