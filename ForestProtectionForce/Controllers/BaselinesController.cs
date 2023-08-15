using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using ForestProtectionForce.Services;
using Microsoft.Extensions.Options;
using ForestProtectionForce.utilities;
using System.Linq.Expressions;
using Microsoft.CodeAnalysis.Operations;

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaselinesController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;
     
        public BaselinesController(ForestProtectionForceContext context)
        {
            _context = context;
        }

        // GET: api/Baselines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Baseline>>> GetBaseline()
        {
          if (_context.Baseline == null)
          {
                return NotFound();
          }

            string xUserData = HttpContext.Request.Headers["X-User-Data"];
            var user = LogicConvertions.getUserDetails(xUserData ?? "");
            int provinceOfSuperAdmins = LogicConvertions.getSuperAdminOfProvince(user);
            var userDetails = _context.UserDetails?.FirstOrDefault(x => x.Username == user.username) ?? new UserDetails();
            return await _context.Baseline.Where(predicateLogicForData(userDetails, provinceOfSuperAdmins)).OrderByDescending(x => x.Id).ToListAsync();
            //.Where(predicateLogicForData(userDetails))
        }

        // GET: api/Baselines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Baseline>> GetBaseline(string id)
        {
          if (_context.Baseline == null)
          {
                return NotFound();
          }
            var baseline = await _context.Baseline.FirstOrDefaultAsync(x => x.CaseNo == id);

            if (baseline == null)
            {
                return NotFound();
            }

            return baseline;
        }

        // PUT: api/Baselines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBaseline(int id, Baseline baseline)
        {
            if (id != baseline.Id)
            {
                return BadRequest();
            }

            _context.Entry(baseline).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BaselineExists(id))
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

        // POST: api/Baselines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Baseline>> PostBaseline(Baseline baseline)
        {
          if (_context.Baseline == null)
          {
              return Problem("Entity set 'ForestProtectionForceContext.Baseline'  is null.");
          }
            BaselineService baselineService = new(_context);
            baseline.CaseNo = baselineService.GenerateCaseNumber();
            _context.Baseline.Add(baseline);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBaseline", new { id = baseline.Id, caseNo = baseline.CaseNo }, baseline);
        }

        // DELETE: api/Baselines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBaseline(int id)
        {
            if (_context.Baseline == null)
            {
                return NotFound();
            }
            var baseline = await _context.Baseline.FindAsync(id);
            if (baseline == null)
            {
                return NotFound();
            }

            _context.Baseline.Remove(baseline);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BaselineExists(int id)
        {
            return (_context.Baseline?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [NonAction]
        public Expression<Func<Baseline, bool>> predicateLogicForData(UserDetails userData, int provinceOfSuperAdmins)
        {
            Expression<Func<Baseline, bool>> condition = null;
            if(userData.UserType_Id == 2)
            {
                condition = x => x.ProvinceId == userData.ProvinceId;
            }
            else if (userData.UserType_Id == 3 || userData.UserType_Id == 4)
            {
                condition = x => x.ForestDivisionId == userData.DistrictId;
            }
            else if (provinceOfSuperAdmins == 1 || provinceOfSuperAdmins == 2) //jammu or kashmir
            {
                condition = x => x.ProvinceId == provinceOfSuperAdmins;
            }
            else
            {
                condition = x => true;
            }

            return condition;
        }

    }
}
