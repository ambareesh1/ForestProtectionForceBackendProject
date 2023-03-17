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
    public class CompartmentsController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;

        public CompartmentsController(ForestProtectionForceContext context)
        {
            _context = context;
        }

        // GET: api/Compartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compartment>>> GetCompartment()
        {
          if (_context.Compartment == null)
          {
              return NotFound();
          }
            var compartmentData = await _context.Compartment.ToListAsync();

            foreach (var compartment in compartmentData)
            {
                compartment.Division = _context.Division.FirstOrDefault(p => p.Id == compartment.DivisionId);
            }

            return compartmentData;
        }

        // GET: api/Compartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Compartment>> GetCompartment(int id)
        {
          if (_context.Compartment == null)
          {
              return NotFound();
          }
            var compartment = await _context.Compartment.FindAsync(id);

            if (compartment == null)
            {
                return NotFound();
            }

            return compartment;
        }

        // PUT: api/Compartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompartment(int id, Compartment compartment)
        {
            if (id != compartment.Id)
            {
                return BadRequest();
            }

            _context.Entry(compartment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompartmentExists(id))
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

        // POST: api/Compartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Compartment>> PostCompartment(Compartment compartment)
        {
          if (_context.Compartment == null)
          {
              return Problem("Entity set 'ForestProtectionForceContext.Compartment'  is null.");
          }
            _context.Compartment.Add(compartment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCompartment", new { id = compartment.Id }, compartment);
        }

        // DELETE: api/Compartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompartment(int id)
        {
            if (_context.Compartment == null)
            {
                return NotFound();
            }
            var compartment = await _context.Compartment.FindAsync(id);
            if (compartment == null)
            {
                return NotFound();
            }

            _context.Compartment.Remove(compartment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompartmentExists(int id)
        {
            return (_context.Compartment?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
