using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApi.Data;
using InvoiceApi.Models;
using Microsoft.AspNetCore.Authorization;
using InvoiceApi.RabbitMQ;

namespace InvoiceApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserModelsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IRabitMQProducer _rabitMQProducer;

        public UserModelsController(DataContext context, IUserService userService, IRabitMQProducer rabitMQProducer)
        {
            _context = context;
            _userService = userService;
            _rabitMQProducer = rabitMQProducer;
        }

        // GET: api/user

        [Authorize(Roles = "Institution")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUserModels()
        {
          if (_context.UserModels == null)
          {
              return NotFound();
          }
            return await _context.UserModels.ToListAsync();
        }

        // GET: api/user/5

        [Authorize(Roles = "Institution")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUserModel(int id)
        {
          if (_context.UserModels == null)
          {
              return NotFound();
          }
            var userModel = await _context.UserModels.FindAsync(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;
        }

        // GET: api/user
        [Authorize(Roles = "User")]
        [HttpGet("me")]
        public async Task<ActionResult<UserModel>> GetMyUserModel()
        {
            var id = _userService.GetMyName();

            if(id == null)
            {
                return NotFound();
            }
            var userModel = await _context.UserModels.FindAsync(int.Parse(id));

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;

        }

        // PUT: api/user/me
        [Authorize(Roles = "User")]
        [HttpPut("me")]
        public async Task<IActionResult> PutUserModel(UserModel userModel)
        {

            var id = _userService.GetMyName();
            if (id == null)
            {
                return BadRequest();
            }

            var myUser = await _context.UserModels.Where(u => u.Id == int.Parse(id)).FirstOrDefaultAsync();

            if(myUser == null)
            {
                return NotFound();
            }

            myUser.Id = int.Parse(id);
            myUser.FirstName = userModel.FirstName;
            myUser.LastName = userModel.LastName;
            myUser.Mail = userModel.Mail;
            myUser.TC = userModel.TC;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserModelExists(int.Parse(id)))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [Authorize(Roles = "User")]
        [HttpPost("card")]
        public async Task<ActionResult<CreditCardModel>> PostCreditCardModel(CreditCardModel creditCardModel)
        {
            if (_context.CreditCardModels == null)
            {
                return Problem("Entity set 'DataContext.CreditCardModels'  is null.");
            }

            var id = _userService.GetMyName();

            var userModel = await _context.UserModels.FindAsync(int.Parse(id));

            if (userModel == null)
            {
                return NotFound();
            }
            //BankName bodyden gelcek

            var myName = userModel.FirstName + " " + userModel.LastName;

            creditCardModel.Balance = 0;
            creditCardModel.UserModelId = int.Parse(id);
            creditCardModel.UserName = myName;

            Random generator = new Random();
            int r = generator.Next(100, 1000);
            long x = generator.NextInt64(1000000000000000, 10000000000000000);

            creditCardModel.CVC = r;
            creditCardModel.Number = x;

            var year = int.Parse(DateTime.Now.ToString("yy")) + 10;
            var strYear = year.ToString();
            var strMonth = DateTime.Now.ToString("MM");

            creditCardModel.ValidDate = strMonth + "/" + strYear;

            _context.CreditCardModels.Add(creditCardModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostCreditCardModel", new { id = creditCardModel.Id }, creditCardModel);

        }

        [Authorize(Roles = "User")]
        [HttpGet("card/me")]
        public async Task<ActionResult<IEnumerable<CreditCardModel>>> GetUsersCreditCardModels()
        {
            var id = _userService.GetMyName();

            if (id == null)
            {
                return NotFound();
            }

            if (_context.CreditCardModels == null)
            {
                return NotFound();
            }

            return await _context.CreditCardModels.Where(u => u.UserModelId == int.Parse(id)).ToListAsync();

        }

        [Authorize(Roles = "User")]
        [HttpGet("card/me/{id}")]
        public async Task<ActionResult<CreditCardModel>> GetUserCreditCardModel(int id)
        {
            var uid = _userService.GetMyName();
            if (uid == null)
            {
                return BadRequest();
            }

            if (_context.CreditCardModels == null)
            {
                return NotFound();
            }

            var creditCardModel = await _context.CreditCardModels.Where(u => u.UserModelId == int.Parse(uid) && u.Id == id).FirstOrDefaultAsync();

            if (creditCardModel == null)
            {
                return NotFound();
            }

            return creditCardModel;
            
        }

        [Authorize(Roles = "User")]
        [HttpDelete("card/me/{id}")]
        public async Task<IActionResult> DeleteCreditCardModel(int id)
        {
            var uid = _userService.GetMyName();
            if (uid == null)
            {
                return BadRequest();
            }
            if (_context.CreditCardModels == null)
            {
                return NotFound();
            }

            var creditCardModel = await _context.CreditCardModels.FindAsync(id);
            if (creditCardModel == null)
            {
                return NotFound();
            }

            if (creditCardModel.UserModelId != int.Parse(uid))
            {
                return NotFound();
            }

            _context.CreditCardModels.Remove(creditCardModel);
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [Authorize(Roles = "User")]
        [HttpGet("card/me/money/{id}/{money}")]
        public async Task<ActionResult<bool>> uploadCreditCardModel(int id, int money)
        {
            var uid = _userService.GetMyName();

            if (uid == null)
            {
                return BadRequest();
            }

            if (_context.CreditCardModels == null)
            {
                return NotFound();
            }

            var creditCardModel = await _context.CreditCardModels.Where(u => u.UserModelId == int.Parse(uid) && u.Id == id).FirstOrDefaultAsync();

            if (creditCardModel == null)
            {
                return NotFound();
            }

            creditCardModel.Balance = creditCardModel.Balance + money;

            _context.Entry(creditCardModel).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CreditCardModelExists(id))
                {
                    Console.WriteLine("Error");
                    return false;
                }
                else
                {
                    throw;
                }
            }

        }
        
        [Authorize(Roles = "User")]
        [HttpGet("test/{id}")]
        public async Task<ActionResult<bool>> testPayMethod(int id, InvoiceModel invoiceModel)
        {
            var uid = _userService.GetMyName();
            int[] arr= new int[25];
            string[] dummyStr = {"a", "b", "c", "d", "e", "f", "g", "h","j", "k", "l", "m", "n","o", "p", "r", "s", "t","u","v","y","z","x","w","i"};
            int[] dummyInt = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25 };

            for(var i = 0; i<25; i++)
            {
                invoiceModel.Id = 0; //Reinit id
                Random generator = new Random();
                int r = generator.Next(100000, 1000000);
                invoiceModel.Name = dummyStr[i];
                invoiceModel.Price = dummyInt[i];
                invoiceModel.Detail = dummyStr[i];
                invoiceModel.InvoiceNumber = r;
                invoiceModel.Month = DateTime.Now.ToString("MM");
                invoiceModel.Status = false;
                invoiceModel.InstitutionModelId = id;
                invoiceModel.UserModelId = int.Parse(uid);

                Console.WriteLine(invoiceModel.Name);

                _context.InvoiceModels.Add(invoiceModel);

                await _context.SaveChangesAsync();

                arr[i] = invoiceModel.Id;

                CreatedAtAction("testPayMethod", new { id = invoiceModel.Id }, invoiceModel);
            }
            for(var i= 0; i < arr.Length; i++)
            {
                Console.WriteLine(arr[i]);
                _rabitMQProducer.SendProductMessage(arr[i]);
                Console.WriteLine("DOne");
            }
         
            return true;
        }
        

        private bool CreditCardModelExists(int id)
        {
            return (_context.CreditCardModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        private bool UserModelExists(int id)
        {
            return (_context.UserModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
    }
}
