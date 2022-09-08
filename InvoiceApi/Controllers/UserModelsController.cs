﻿using System;
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
    [Route("api/user")]
    [ApiController]
    public class UserModelsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public UserModelsController(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: api/user
        //MAYBE kurum görmek için

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
        //MAYBE kurum görmek için

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

        //USER
        //GetMe
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

            /*
            if (_context.UserModels == null)
            {
                return NotFound();
            }
            var userModel = await _context.UserModels.FindAsync(id);

            if (userModel == null)
            {
                return NotFound();
            }

            return userModel;*/
        }


        //USER
        //UpdateMe
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

            userModel.Id = int.Parse(id);

            _context.Entry(userModel).State = EntityState.Modified;

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

        //Auth Register + Login
        // POST: api/user
       
        /*
        [HttpPost]
        public async Task<ActionResult<UserModel>> PostUserModel(UserModel userModel)
        {
          if (_context.UserModels == null)
          {
              return Problem("Entity set 'DataContext.UserModels'  is null.");
          }
            _context.UserModels.Add(userModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserModel", new { id = userModel.Id }, userModel);
        }
        */

        //Bu olmucak
        // DELETE: api/user/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserModel(int id)
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

            _context.UserModels.Remove(userModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        */
        private bool UserModelExists(int id)
        {
            return (_context.UserModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        
    }
}
