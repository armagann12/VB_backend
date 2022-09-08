using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceApi.Data;
using InvoiceApi.Models;

namespace InvoiceApi.Controllers
{
    [Route("api/institution")]
    [ApiController]
    public class InstitutionModelsController : ControllerBase
    {
        private readonly DataContext _context;

        public InstitutionModelsController(DataContext context)
        {
            _context = context;
        }

        //MAYBE User
        // GET: api/institution

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InstitutionModel>>> GetInstitutionModels()
        {
          if (_context.InstitutionModels == null)
          {
              return NotFound();
          }
            return await _context.InstitutionModels.ToListAsync();
        }

        //MAYBE User
        // GET: api/institution/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InstitutionModel>> GetInstitutionModel(int id)
        {
          if (_context.InstitutionModels == null)
          {
              return NotFound();
          }
            var institutionModel = await _context.InstitutionModels.FindAsync(id);

            if (institutionModel == null)
            {
                return NotFound();
            }

            return institutionModel;
        }

        //Institution
        //GetMe
        // GET: api/institution/me/5
        [HttpGet("me/{id}")]
        public async Task<ActionResult<InstitutionModel>> GetMyInstitutionModel(int id)
        {
            if (_context.InstitutionModels == null)
            {
                return NotFound();
            }
            var institutionModel = await _context.InstitutionModels.FindAsync(id);

            if (institutionModel == null)
            {
                return NotFound();
            }

            return institutionModel;
        }

        //Institution
        //UpdateMe
        // PUT: api/institution/5

        [HttpPut("me/{id}")]
        public async Task<IActionResult> PutInstitutionModel(int id, InstitutionModel institutionModel)
        {
            if (id != institutionModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(institutionModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstitutionModelExists(id))
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

        //Auth Register + Login
        // POST: api/institution
        /*
        [HttpPost]
        public async Task<ActionResult<InstitutionModel>> PostInstitutionModel(InstitutionModel institutionModel)
        {
          if (_context.InstitutionModels == null)
          {
              return Problem("Entity set 'DataContext.InstitutionModels'  is null.");
          }
            _context.InstitutionModels.Add(institutionModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInstitutionModel", new { id = institutionModel.Id }, institutionModel);
        }
        */

        //Bu olmucak
        // DELETE: api/institution/5
        /*
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstitutionModel(int id)
        {
            if (_context.InstitutionModels == null)
            {
                return NotFound();
            }
            var institutionModel = await _context.InstitutionModels.FindAsync(id);
            if (institutionModel == null)
            {
                return NotFound();
            }

            _context.InstitutionModels.Remove(institutionModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */

        private bool InstitutionModelExists(int id)
        {
            return (_context.InstitutionModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
