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
    public class ProvincesController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;

        public ProvincesController(ForestProtectionForceContext context)
        {
            _context = context;
        }

        // GET: api/Provinces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Province>>> GetProvince()
        {
          if (_context.Province == null)
          {
              return NotFound();
          }
            return await _context.Province.ToListAsync();
        }

        // GET: api/Provinces/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Province>> GetProvince(int id)
        {
          if (_context.Province == null)
          {
              return NotFound();
          }
            var province = await _context.Province.FindAsync(id);

            if (province == null)
            {
                return NotFound();
            }

            return province;
        }

        // PUT: api/Provinces/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProvince(int id, Province province)
        {
            if (id != province.Id)
            {
                return BadRequest();
            }

            _context.Entry(province).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProvinceExists(id))
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

        // POST: api/Provinces
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Province>> PostProvince(Province province)
        {
          if (_context.Province == null)
          {
              return Problem("Entity set 'ForestProtectionForceContext.Province'  is null.");
          }
            _context.Province.Add(province);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProvince", new { id = province.Id }, province);
        }

        // DELETE: api/Provinces/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProvince(int id)
        {
            if (_context.Province == null)
            {
                return NotFound();
            }
            var province = await _context.Province.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }

            _context.Province.Remove(province);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool ProvinceExists(int id)
        {
            return (_context.Province?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
