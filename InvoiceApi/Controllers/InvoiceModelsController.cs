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
using System.Security.Cryptography;

namespace InvoiceApi.Controllers
{
    [Route("api/invoice")]
    [ApiController]
    public class InvoiceModelsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public InvoiceModelsController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        //  +param olarak filterlama al status true false
        // GET: api/invoice/user
        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<InvoiceModel>>> GetUsersInvoiceModels([FromQuery(Name = "status")] bool? status = null )
        {
            
            var id = _userService.GetMyName();

            if(id == null)
            {
                return NotFound();
            }

          if (_context.InvoiceModels == null)
          {
              return NotFound();
          }

          if(status == true || status == false)
            {
                return await _context.InvoiceModels.Where(u => u.UserModelId == int.Parse(id) && u.Status == status).ToListAsync();
            }

            return await _context.InvoiceModels.Where(u => u.UserModelId == int.Parse(id)).ToListAsync();
           
        }

        // GET: api/invoice/user/5
        [Authorize(Roles = "User")]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<InvoiceModel>> GetUserInvoiceModel(int id)
        {
            var uid = _userService.GetMyName();
            if (uid == null)
            {
                return BadRequest();
            }

            if (_context.InvoiceModels == null)
          {
              return NotFound();
          }
            var invoiceModel = await _context.InvoiceModels.Where(u => u.UserModelId == int.Parse(uid) && u.Id == id).FirstOrDefaultAsync();

            if (invoiceModel == null)
            {
                return NotFound();
            }

            return invoiceModel;
        }

        //  +param olarak filterlama al status true false
        // GET: api/invoice/user
        [Authorize(Roles = "Institution")]
        [HttpGet("institution")]
        public async Task<ActionResult<IEnumerable<InvoiceModel>>> GetInstitutionsInvoiceModels(bool? status = null)
        {
            var id = _userService.GetMyName();
            
            if (id == null)
            {
                return BadRequest();
            }
            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            if(status == true || status == false)
            {
                return await _context.InvoiceModels.Where(u => u.InstitutionModelId == int.Parse(id) && u.Status == status).ToListAsync();
            }
            return await _context.InvoiceModels.Where(u => u.InstitutionModelId == int.Parse(id)).ToListAsync();
        }

        // GET: api/invoice/user/5
        [Authorize(Roles = "Institution")]
        [HttpGet("institution/{id}")]
        public async Task<ActionResult<InvoiceModel>> GetInstitutionInvoiceModel(int id)
        {
            var uid = _userService.GetMyName();
            if (uid == null)
            {
                return BadRequest();
            }
            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            var invoiceModel = await _context.InvoiceModels.Where(u => u.InstitutionModelId == int.Parse(uid) && u.Id == id).FirstOrDefaultAsync();

            if (invoiceModel == null)
            {
                return NotFound();
            }

            return invoiceModel;
        }

        // PUT: api/invoice/me/5
        [Authorize(Roles = "Institution")]
        [HttpPut("me/{id}")]
        public async Task<IActionResult> PutInvoiceModel(int id, InvoiceModel invoiceModel)
        {
            var uid = _userService.GetMyName();

            if(uid == null)
            {
                return BadRequest();
            }

            /*
            if (invoiceModel.InstitutionModelId != int.Parse(uid))
            {
                return NotFound();
            }
            */

            var myInvoice = await _context.InvoiceModels.Where(u => u.InstitutionModelId == int.Parse(uid) && u.Id == id).FirstOrDefaultAsync();

            if (myInvoice == null)
            {
                return NotFound();
            }


            myInvoice.Name = invoiceModel.Name;
            myInvoice.Detail = invoiceModel.Detail;
            myInvoice.Price = invoiceModel.Price;
            
            _context.Entry(myInvoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceModelExists(id))
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

        // POST: api/invoice
        [Authorize(Roles = "Institution")]
        [HttpPost]
        public async Task<ActionResult<InvoiceModel>> PostInvoiceModel(InvoiceModel invoiceModel)
        {
          if (_context.InvoiceModels == null)
          {
              return Problem("Entity set 'DataContext.InvoiceModels'  is null.");
          }

            var id = _userService.GetMyName();

            Random generator = new Random();
            int r = generator.Next(100000, 1000000);

            invoiceModel.InvoiceNumber = r;
            invoiceModel.Month = DateTime.Now.ToString("MM");
            invoiceModel.Status = false;
            invoiceModel.InstitutionModelId = int.Parse(id);

            _context.InvoiceModels.Add(invoiceModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostInvoiceModel", new { id = invoiceModel.Id }, invoiceModel);
        }

        // DELETE: api/invoice/me/5
        [Authorize(Roles = "Institution")]
        [HttpDelete("me/{id}")]
        public async Task<IActionResult> DeleteInvoiceModel(int id)
        {
            var uid = _userService.GetMyName();
            if(uid == null)
            {
                BadRequest();
            }
            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            var invoiceModel = await _context.InvoiceModels.FindAsync(id);
            if (invoiceModel == null)
            {
                return NotFound();
            }

            if(invoiceModel.InstitutionModelId != int.Parse(uid))
            {
                return NotFound();
            }

            _context.InvoiceModels.Remove(invoiceModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Get: api/pay/id
        [Authorize(Roles = "User")]
        [HttpGet("pay/{id}")]
        public async Task<ActionResult<InvoiceModel>> PayInvoiceModel(int id)
        {
            var uid = _userService.GetMyName();
            if(uid == null)
            {
                return BadRequest();
            }

            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            var invoiceModel = await _context.InvoiceModels.FindAsync(id);
            

            if (invoiceModel == null)
            {
                return NotFound();
            }

            if(invoiceModel.UserModelId != int.Parse(uid))
            {
                return NotFound();
            }

            invoiceModel.Status = true;

            _context.Entry(invoiceModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return invoiceModel;
        }



        private bool InvoiceModelExists(int id)
        {
            return (_context.InvoiceModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
