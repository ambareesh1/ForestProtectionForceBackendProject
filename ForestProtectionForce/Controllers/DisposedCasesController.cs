using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using ForestProtectionForce.utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisposedCasesController : ControllerBase
    {

        private readonly ForestProtectionForceContext _context;
        private readonly IWebHostEnvironment _env;
        public DisposedCasesController(ForestProtectionForceContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/<DisposedCasesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DisposedCases>>> GetDisposedCases()
        {
            if (_context.DisposedCases == null)
            {
                return NotFound();
            }
            string xUserData = HttpContext.Request.Headers["X-User-Data"];
            var user = LogicConvertions.getUserDetails(xUserData ?? "");
            var disposedCases = await _context.DisposedCases.ToListAsync();
            int provinceOfSuperAdmins = LogicConvertions.getSuperAdminOfProvince(user);
            var userDetails = _context.UserDetails?.FirstOrDefault(x => x.Username == user.username) ?? new UserDetails();
            return await _context.DisposedCases.Where(predicateLogicForData(userDetails, provinceOfSuperAdmins)).OrderByDescending(x => x.Id).ToListAsync(); ;
        }

        // GET api/<DisposedCasesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DisposedCases>> GetDisposedCasesById(int id)
        {
            if (_context.DisposedCases == null)
            {
                return NotFound();
            }
            var disposedCases = await _context.DisposedCases.FindAsync(id);

            if (disposedCases == null)
            {
                return NotFound();
            }

            return disposedCases;
        }

        // POST api/<DisposedCasesController>
        [HttpPost]
        public async Task<ActionResult<DisposedCases>> PostDisposedCases(DisposedCases disposedCases)
        {
            if (_context.DisposedCases == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.disposedCases'  is null.");
            }

            _context.DisposedCases.Add(disposedCases);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostDisposedSheet", new { id = disposedCases.Id }, disposedCases);
        }

        // PUT api/<DisposedCasesController>/5

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisposedSheet(int id, DisposedCases disposedCases)
        {
            if (id != disposedCases.Id)
            {
                return BadRequest();
            }

            _context.Entry(disposedCases).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DisposedCasesExists(id))
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

        // DELETE api/<DisposedCasesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [NonAction]
        public Expression<Func<DisposedCases, bool>> predicateLogicForData(UserDetails userData, int provinceOfSuperAdmins)
        {
            Expression<Func<DisposedCases, bool>> condition = null;
            if (userData.UserType_Id == 2)
            {
                condition = x => x.Province == userData.ProvinceId;
            }
            else if (userData.UserType_Id == 3 || userData.UserType_Id == 4)
            {
                condition = x => x.District == userData.DistrictId;
            }
            else if (provinceOfSuperAdmins == 1 || provinceOfSuperAdmins == 2) //jammu or kashmir
            {
                condition = x => x.Province == provinceOfSuperAdmins;
            }
            else
            {
                condition = x => true;
            }

            return condition;
        }
        private bool DisposedCasesExists(int id)
        {
            return (_context.DisposedCases?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

}
