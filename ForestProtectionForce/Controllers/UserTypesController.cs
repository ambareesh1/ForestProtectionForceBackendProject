using ForestProtectionForce.Data;
using ForestProtectionForce.Migrations;
using ForestProtectionForce.Models;
using ForestProtectionForce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypesController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;
        public UserTypesController(ForestProtectionForceContext context)
        {
            _context = context;
           
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserTypes>>> GetUserTypes()
        {
            if (_context.UserTypes == null)
            {
                return NotFound();
            }
            return await _context.UserTypes.ToListAsync();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetails(int id, UserTypes userTypes)
        {
            if (id != userTypes.Id)
            {
                return BadRequest();
            }

            _context.Entry(userTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserTypesExists(id))
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

        // POST api/<UserTypesController>
        [HttpPost]
        public async Task<ActionResult<UserTypes>> PostUserTypes(UserTypes userTypes)
        {
            if (_context.UserTypes == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            _context.UserTypes.Add(userTypes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserTypes", new { id = userTypes.Id }, userTypes);
        }
     
        
        // DELETE api/<UserTypesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserTypes(int id)
        {
            if (_context.UserTypes == null)
            {
                return NotFound();
            }
            var userTypeDetails = await _context.UserTypes.FindAsync(id);
            if (userTypeDetails == null)
            {
                return NotFound();
            }

            _context.UserTypes.Remove(userTypeDetails);
            await _context.SaveChangesAsync();

            return Ok();
        }
        private bool UserTypesExists(int id)
        {
            return (_context.UserTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
