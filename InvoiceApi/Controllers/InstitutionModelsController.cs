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
using InvoiceApi.Services;

namespace InvoiceApi.Controllers
{
    [Route("api/institution")]
    [ApiController]
    public class InstitutionModelsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public InstitutionModelsController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        //MAYBE User
        // GET: api/institution
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "Institution")]
        [HttpGet("me")]
        public async Task<ActionResult<InstitutionModel>> GetMyInstitutionModel()
        {
            var id = _userService.GetMyName();

            if (id == null)
            {
                return NotFound();
            }
            var institutionModel = await _context.InstitutionModels.FindAsync(int.Parse(id));

            if (institutionModel == null)
            {
                return NotFound();
            }

            return institutionModel;
        }

        //Institution
        //UpdateMe
        // PUT: api/institution/me
        [Authorize(Roles = "Institution")]
        [HttpPut("me")]
        public async Task<IActionResult> PutInstitutionModel(InstitutionModel institutionModel)
        {
            var id = _userService.GetMyName();

            if (id == null)
            {
                return BadRequest();
            }

            institutionModel.Id = int.Parse(id);

            _context.Entry(institutionModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InstitutionModelExists(int.Parse(id)))
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
