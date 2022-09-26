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
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Get Institutions
        /// </summary>
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

        // GET: api/institution/id
        /// <summary>
        /// Get Institution
        /// </summary>
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

        // GET: api/institution/me
        /// <summary>
        /// Get Myself (institution)
        /// </summary>
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

        // PUT: api/institution/me
        /// <summary>
        /// Update Myself (institution)
        /// </summary>
        [Authorize(Roles = "Institution")]
        [HttpPut("me")]
        public async Task<IActionResult> PutInstitutionModel(InstitutionModel institutionModel)
        {
            var id = _userService.GetMyName();

            if (id == null)
            {
                return BadRequest();
            }

            var myInstitution = await _context.InstitutionModels.Where(u => u.Id == int.Parse(id)).FirstOrDefaultAsync();

            if (myInstitution == null)
            {
                return NotFound();
            }

            myInstitution.Id = int.Parse(id);
            myInstitution.Name = institutionModel.Name;
            myInstitution.Detail = institutionModel.Detail;
            myInstitution.Mail = institutionModel.Mail;

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

        private bool InstitutionModelExists(int id)
        {
            return (_context.InstitutionModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
