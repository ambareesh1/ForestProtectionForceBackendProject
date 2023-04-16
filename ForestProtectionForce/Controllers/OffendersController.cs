using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using System.Net.Http.Headers;
using System.IO;
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
            return await _context.Offender.OrderByDescending(x=>x.Id).ToListAsync();
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

        [HttpGet("GetOffenderWithAadhar")]
        public async Task<ActionResult<Offender>> GetOffenderWithAadhar(string aadhar)
        {
            if (_context.Offender == null)
            {
                return NotFound();
            }
            var offender = await _context.Offender.FirstOrDefaultAsync(x=>x.AadhaarNo == aadhar);

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

        [HttpPut("RemoveCaseId")]
        public async Task<IActionResult> RemoveCaseIdFromOffender(string caseId, Offender offender)
        {
          
            offender.CaseId = removeCaseId(offender.CaseId, caseId);
            _context.Entry(offender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OffenderExists(offender.Id))
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

        [HttpPut("UpdateOffendersFromBaseLine")]
        public async Task<IActionResult> UpdateOffendersFromBaseLine(string caseId, List<Offender> offenders)
        {
            foreach (var offender in offenders)
            {
                offender.CaseId = offender.CaseId == string.Empty? caseId : offender.CaseId + "," +caseId;
                offender.CaseId = getDistinctCaseIds(offender.CaseId);
                _context.Entry(offender).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception
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

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync()
        {
            try
            {

          
            var file = Request.Form.Files[0];

            if (file.Length > 0)
            {
                // Get the file name and extension
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
              
                var fileExtension = Path.GetExtension(fileName);

                // Generate a unique file name to avoid conflicts
                var newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}{fileExtension}";

                // Save the file to disk 
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", newFileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                // Return the file URL
                return Ok(new { url = filePath });
            }
            }
            catch (Exception ex)
            {
                string fileName = "example.txt"; // specify the file name
                string content = ex.ToString(); // specify the content of the text file

                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", fileName); // get the file path

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(content); // write the content to the file
                }
            }
            return BadRequest("No file uploaded.");
        }
        [NonAction]
        public  string getDistinctCaseIds(string input)
        {
            if (input.Contains(","))
            {
                string[] values = input.Split(',');
                // Get distinct values
                string[] distinctValues = values.Distinct().ToArray();
                // Convert back to string
                string result = string.Join(",", distinctValues);
                return result;
            }
            return input;
           
        }

        [NonAction]
        public string removeCaseId(string originalString, string caseIdToRemove)
        {
            // Split the original string by commas to create an array of caseIds
            string[] caseIds = originalString.Split(',');

            // Use List<string> to store the updated list of caseIds
            List<string> updatedCaseIds = new List<string>();

            // Iterate through each caseId in the array and add it to the updated list,
            // skipping the caseId to be removed
            foreach (string caseId in caseIds)
            {
                if (!caseId.Trim().Equals(caseIdToRemove))
                {
                    updatedCaseIds.Add(caseId.Trim());
                }
            }

            // Join the updated list of caseIds back into a string with commas
            string updatedString = string.Join(", ", updatedCaseIds);

            return updatedString;

        }
    }
}
