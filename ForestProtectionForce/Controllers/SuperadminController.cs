﻿using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using ForestProtectionForce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuperadminController : ControllerBase
    {

        private readonly ForestProtectionForceContext _context;
        private readonly IEmailService _emailService;
        public SuperadminController(ForestProtectionForceContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Superadmin>>> GetSuperadmin()
        {
            if (_context.UserTypes == null)
            {
                return NotFound();
            }
            return await _context.Superadmin.ToListAsync();
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<Superadmin>> GetSuperAdminByuserName(string userName)
        {
            if (_context.Superadmin == null)
            {
                return NotFound();
            }
            var superAdminDetails = await _context.Superadmin.FirstOrDefaultAsync(x=>x.Username == userName);

            if (superAdminDetails == null)
            {
                return NotFound();
            }

            return superAdminDetails;
        }
 


        [HttpPost("verifySuperAdminUser")]
        public async Task<ActionResult<Superadmin>> VerifySuperadmin([FromBody] object data)
        {
            if (data == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            JObject jsonData = JObject.Parse(data.ToString());
            string? userPassword = jsonData["password"].ToString();
            string? userName = jsonData["username"].ToString();
            int otp = GenarateOtp();
            Superadmin? result = await _context.Superadmin.FirstOrDefaultAsync(x => x.Username == userName && x.Password == userPassword)
                .ConfigureAwait(false);
            if (result != null)
            {
                PasswordHashService passwordHashService = new(_context, _emailService);
                await UpdateOtp(result.Id, otp, result);
                await passwordHashService.sendSuperAdminOtpAsync(result, otp);
                return result;

            }
            return null;
        }
        [HttpPut("updateOtp")]
        public async Task<IActionResult> UpdateOtp(int id, int otp, Superadmin userDetails)
        {
            if (id != userDetails.Id)
            {
                return BadRequest();
            }
            userDetails.Otp = otp;
            _context.Entry(userDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuperadminExists(userDetails.Id))
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
        [HttpPost]
        public async Task<ActionResult<Superadmin>> PosSUperadmin(Superadmin superadmin)
        {
            if (_context.Superadmin == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.Superadmin'  is null.");
            }
            _context.Superadmin.Add(superadmin);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserTypes", new { id = superadmin.Id }, superadmin);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutSuperadmin(int id, Superadmin superadmin)
        {
            if (id != superadmin.Id)
            {
                return BadRequest();
            }

            _context.Entry(superadmin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuperadminExists(id))
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
        [HttpPost("changeSuperPassword")]
        public async Task<ActionResult<Superadmin>> ChangePassword([FromBody] object data)
        {
            if (data == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            JObject jsonData = JObject.Parse(data.ToString());
            string? userName = jsonData["username"].ToString();
            string password = jsonData["password"].ToString();
            string? newpassword = jsonData["newpassword"].ToString();
            var result = await _context.Superadmin.FirstOrDefaultAsync(x => x.Username == userName && password == password).ConfigureAwait(false);
            result.Password = newpassword;
            _context.Entry(result).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuperadminExists(result.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return result;
        }
           
        private bool SuperadminExists(int id)
        {
            return (_context.Superadmin?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private int GenarateOtp()
        {
            Random random = new Random();
            int otp = random.Next(1000, 10000);
            return otp;
        }
    }
}
