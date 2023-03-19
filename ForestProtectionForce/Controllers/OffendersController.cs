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
    public class OffendersController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;
        private readonly IWebHostEnvironment _env;
        public OffendersController(ForestProtectionForceContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Offenders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Offender>>> GetOffender()
        {
          if (_context.Offender == null)
          {
              return NotFound();
          }
            return await _context.Offender.ToListAsync();
        }

        // GET: api/Offenders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Offender>> GetOffender(int id)
        {
          if (_context.Offender == null)
          {
              return NotFound();
          }
            var offender = await _context.Offender.FindAsync(id);

            if (offender == null)
            {
                return NotFound();
            }

            return offender;
        }

        // PUT: api/Offenders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOffender(int id, Offender offender)
        {
            if (id != offender.Id)
            {
                return BadRequest();
            }

            _context.Entry(offender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OffenderExists(id))
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

        // POST: api/Offenders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Offender>> PostOffender(Offender offender)
        {
          if (_context.Offender == null)
          {
              return Problem("Entity set 'ForestProtectionForceContext.Offender'  is null.");
          }
            _context.Offender.Add(offender);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOffender", new { id = offender.Id }, offender);
        }

        // DELETE: api/Offenders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOffender(int id)
        {
            if (_context.Offender == null)
            {
                return NotFound();
            }
            var offender = await _context.Offender.FindAsync(id);
            if (offender == null)
            {
                return NotFound();
            }

            _context.Offender.Remove(offender);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OffenderExists(int id)
        {
            return (_context.Offender?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadAsync()
        {
            var file = Request.Form.Files[0];

            if (file.Length > 0)
            {
                // Get the file name and extension
                var fileName = Path.GetFileName(file.FileName);
                var fileExtension = Path.GetExtension(fileName);

                // Generate a unique file name to avoid conflicts
                var newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}{fileExtension}";

                // Save the file to disk
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", newFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Return the file URL
                return Ok(new { url = filePath });
            }

            return BadRequest("No file uploaded.");
        }
    }
}
