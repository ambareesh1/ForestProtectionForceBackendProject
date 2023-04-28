using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeizuresController : ControllerBase
    {

        private readonly ForestProtectionForceContext _context;
       
        public SeizuresController(ForestProtectionForceContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seizures_Form_A>>> GetFormA()
        {
            try
            {
                if (_context.Seizures_Form_A == null)
                {
                    return NotFound();
                }
                return await _context.Seizures_Form_A.ToListAsync();
            }catch (Exception ex) {
                return null;
            }
          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFormA(int id, Seizures_Form_A seizures_Form_A)
        {
            if (id != seizures_Form_A.id)
            {
                return BadRequest();
            }

            _context.Entry(seizures_Form_A).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormAExists(id))
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

        [HttpPost("PostFormA")]
        public async Task<ActionResult<Seizures_Form_A>> PostFormA(Seizures_Form_A seizures_Form_A)
        {
            if (_context.Seizures_Form_A == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.FormA'  is null.");
            }
            _context.Seizures_Form_A.Add(seizures_Form_A);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Seizures_Form_A", new { id = seizures_Form_A.id }, seizures_Form_A);
        }

     

        // DELETE api/<Seizures>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [NonAction]
        public Seizures_Form_A createFormAIfNewMonth()
        {
            //var formA = new List<Seizures_Form_A>(new Seizures_Form_A)
            //);



            return null;// new Seizures_Form_A();
        }

        [NonAction]
        public bool checkTheCurrentMonthAndYear()
        {
            var result =   _context.Seizures_Form_A?.LastOrDefault();

            return result?.month == DateTime.Now.Month && result.year == DateTime.Now.Year;

        }

        private bool FormAExists(int id)
        {
            return (_context.Seizures_Form_A?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
