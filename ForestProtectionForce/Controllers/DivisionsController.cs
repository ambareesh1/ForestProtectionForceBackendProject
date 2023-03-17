using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForestProtectionForce.Data;
using ForestProtectionForce.Models;

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;

        public DivisionsController(ForestProtectionForceContext context)
        {
            _context = context;
        }

        // GET: api/Divisions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Division>>> GetDivision()
        {
          if (_context.Division == null)
          {
              return NotFound();
          }
            var divisionData = await _context.Division.ToListAsync();

            foreach (var division in divisionData)
            {
                division.District = _context.District.FirstOrDefault(p => p.Id == division.DistrictId);
            }

            return divisionData;
        }

        // GET: api/Divisions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Division>> GetDivision(int id)
        {
          if (_context.Division == null)
          {
              return NotFound();
          }
            var division = await _context.Division.FindAsync(id);

            if (division == null)
            {
                return NotFound();
            }

            return division;
        }

        // PUT: api/Divisions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDivision(int id, Division division)
        {
            if (id != division.Id)
            {
                return BadRequest();
            }

            _context.Entry(division).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DivisionExists(id))
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

        // POST: api/Divisions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Division>> PostDivision(Division division)
        {
          if (_context.Division == null)
          {
              return Problem("Entity set 'ForestProtectionForceContext.Division'  is null.");
          }
            _context.Division.Add(division);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDivision", new { id = division.Id }, division);
        }

        // DELETE: api/Divisions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDivision(int id)
        {
            if (_context.Division == null)
            {
                return NotFound();
            }
            var division = await _context.Division.FindAsync(id);
            if (division == null)
            {
                return NotFound();
            }

            _context.Division.Remove(division);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DivisionExists(int id)
        {
            return (_context.Division?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
