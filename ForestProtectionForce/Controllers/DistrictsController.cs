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
    public class DistrictsController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;

        public DistrictsController(ForestProtectionForceContext context)
        {
            _context = context;
        }

        // GET: api/Districts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<District>>> GetDistrict()
        {
          if (_context.District == null)
          {
              return NotFound();
          }
            var districtData = await _context.District.ToListAsync();

            foreach (var district in districtData)
            {
                district.Circle = _context.Circle.FirstOrDefault(p => p.Id == district.CircleId);
            }

            return districtData;

            
        }

        // GET: api/Districts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<District>> GetDistrict(int id)
        {
          if (_context.District == null)
          {
              return NotFound();
          }
            var district = await _context.District.FindAsync(id);

            if (district == null)
            {
                return NotFound();
            }

            return district;
        }

        // PUT: api/Districts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistrict(int id, District district)
        {
            if (id != district.Id)
            {
                return BadRequest();
            }

            _context.Entry(district).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictExists(id))
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

        // POST: api/Districts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<District>> PostDistrict(District district)
        {
          if (_context.District == null)
          {
              return Problem("Entity set 'ForestProtectionForceContext.District'  is null.");
          }
            _context.District.Add(district);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDistrict", new { id = district.Id }, district);
        }

        // DELETE: api/Districts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistrict(int id)
        {
            if (_context.District == null)
            {
                return NotFound();
            }
            var district = await _context.District.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }

            _context.District.Remove(district);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DistrictExists(int id)
        {
            return (_context.District?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
