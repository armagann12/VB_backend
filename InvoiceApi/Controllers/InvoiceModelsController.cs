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

namespace InvoiceApi.Controllers
{
    [Route("api/invoice")]
    [ApiController]
    public class InvoiceModelsController : ControllerBase
    {
        private readonly DataContext _context;

        public InvoiceModelsController(DataContext context)
        {
            _context = context;
        }

        //USER
        //getAllUsersInvoices
        //  +param olarak filterlama al status true false
        // GET: api/invoice/user
        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<InvoiceModel>>> GetUsersInvoiceModels()
        {
          if (_context.InvoiceModels == null)
          {
              return NotFound();
          }
            return await _context.InvoiceModels.ToListAsync();
        }

        //USER
        //getUserInvoice
        // GET: api/invoice/user/5
        [Authorize]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<InvoiceModel>> GetUserInvoiceModel(int id)
        {
          if (_context.InvoiceModels == null)
          {
              return NotFound();
          }
            var invoiceModel = await _context.InvoiceModels.FindAsync(id);

            if (invoiceModel == null)
            {
                return NotFound();
            }

            return invoiceModel;
        }

        //KURUM
        //getAllInstitutionsInvoices
        //  +param olarak filterlama al status true false
        // GET: api/invoice/user
        [Authorize]
        [HttpGet("institution")]
        public async Task<ActionResult<IEnumerable<InvoiceModel>>> GetInstitutionsInvoiceModels()
        {
            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            return await _context.InvoiceModels.ToListAsync();
        }

        //KURUM
        //getInstitutionInvoices
        // GET: api/invoice/user/5
        [Authorize]
        [HttpGet("institution/{id}")]
        public async Task<ActionResult<InvoiceModel>> GetInstitutionInvoiceModel(int id)
        {
            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            var invoiceModel = await _context.InvoiceModels.FindAsync(id);

            if (invoiceModel == null)
            {
                return NotFound();
            }

            return invoiceModel;
        }

        //KURUM
        //UpdateMyInvoice
        // PUT: api/invoice/me/5
        [Authorize]
        [HttpPut("me/{id}")]
        public async Task<IActionResult> PutInvoiceModel(int id, InvoiceModel invoiceModel)
        {
            if (id != invoiceModel.Id)
            {
                return BadRequest();
            }
            
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

            return NoContent();
        }

        //Otomatik kurum idsini girecek
        //KURUM
        // POST: api/invoice
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<InvoiceModel>> PostInvoiceModel(InvoiceModel invoiceModel)
        {
          if (_context.InvoiceModels == null)
          {
              return Problem("Entity set 'DataContext.InvoiceModels'  is null.");
          }
            invoiceModel.Month = DateTime.Now.ToString("MM");
            invoiceModel.Status = false;

            _context.InvoiceModels.Add(invoiceModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostInvoiceModel", new { id = invoiceModel.Id }, invoiceModel);
        }

        //KURUM
        //DeleteMyInvoice
        // DELETE: api/invoice/me/5
        [Authorize]
        [HttpDelete("me/{id}")]
        public async Task<IActionResult> DeleteInvoiceModel(int id)
        {
            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            var invoiceModel = await _context.InvoiceModels.FindAsync(id);
            if (invoiceModel == null)
            {
                return NotFound();
            }

            _context.InvoiceModels.Remove(invoiceModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //PAY Invoice sadece status değişecek USER kullanıcak
        [Authorize]
        [HttpGet("pay/{id}")]
        public async Task<ActionResult<InvoiceModel>> PayInvoiceModel(int id)
        {
            if (_context.InvoiceModels == null)
            {
                return NotFound();
            }
            var invoiceModel = await _context.InvoiceModels.FindAsync(id);
            

            if (invoiceModel == null)
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
