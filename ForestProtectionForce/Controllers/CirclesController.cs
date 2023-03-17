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
    public class CirclesController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;

        public CirclesController(ForestProtectionForceContext context)
        {
            _context = context;
        }

        // GET: api/Circles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Circle>>> GetCircle()
        {
            if (_context.Circle == null)
            {
                return NotFound();
            }

            var circleData = await _context.Circle.ToListAsync();

            foreach (var circle in circleData)
            {
                circle.Province = _context.Province.FirstOrDefault(p => p.Id == circle.ProvinceId);
            }

            return circleData;

        }

        // GET: api/Circles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Circle>> GetCircle(int id)
        {
          if (_context.Circle == null)
          {
              return NotFound();
          }
            var circle = await _context.Circle.FindAsync(id);

            if (circle == null)
            {
                return NotFound();
            }

            return circle;
        }

        // PUT: api/Circles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCircle(int id, Circle circle)
        {
            if (id != circle.Id)
            {
                return BadRequest();
            }

            _context.Entry(circle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CircleExists(id))
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

        // POST: api/Circles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Circle>> PostCircle(Circle circle)
        {
          if (_context.Circle == null)
          {
              return Problem("Entity set 'ForestProtectionForceContext.Circle'  is null.");
          }
            _context.Circle.Add(circle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCircle", new { id = circle.Id }, circle);
        }

        // DELETE: api/Circles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCircle(int id)
        {
            if (_context.Circle == null)
            {
                return NotFound();
            }
            var circle = await _context.Circle.FindAsync(id);
            if (circle == null)
            {
                return NotFound();
            }

            _context.Circle.Remove(circle);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CircleExists(int id)
        {
            return (_context.Circle?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
