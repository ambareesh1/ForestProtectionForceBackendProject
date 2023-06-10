using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using ForestProtectionForce.utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorySheetController : ControllerBase
    {
    private readonly ForestProtectionForceContext _context;
    private readonly IWebHostEnvironment _env;
    public HistorySheetController(ForestProtectionForceContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
        // GET: HistorySheetController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorySheet>>> GetHistorySheet()
        {
            if (_context.Offender == null)
            {
                return NotFound();
            }
            string xUserData = HttpContext.Request.Headers["X-User-Data"];
            var user = LogicConvertions.getUserDetails(xUserData ?? "");
            var historySheet = await _context.HistorySheet.ToListAsync();
            return  historySheet;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<HistorySheet>> GetHistoryById(int id)
        {
            if (_context.HistorySheet == null)
            {
                return NotFound();
            }
            var history = await _context.HistorySheet.FindAsync(id);

            if (history == null)
            {
                return NotFound();
            }

            return history;
        }
        // GET: HistorySheetController/Create
        [HttpPost]
        public async Task<ActionResult<HistorySheet>> PostHistorySheet(HistorySheet historySheet)
        {
            if (_context.HistorySheet == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.HistorySheet'  is null.");
            }
          
            _context.HistorySheet.Add(historySheet);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostHistorySheet", new { id = historySheet.Id }, historySheet);
        }


        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistorySheet(int id, HistorySheet historySheet)
        {
            if (id != historySheet.Id)
            {
                return BadRequest();
            }
           
            _context.Entry(historySheet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistorySheetExists(id))
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


        private bool HistorySheetExists(int id)
        {
            return (_context.HistorySheet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
