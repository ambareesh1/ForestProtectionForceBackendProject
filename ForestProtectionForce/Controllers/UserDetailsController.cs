using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using ForestProtectionForce.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json.Linq;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForestProtectionForce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly ForestProtectionForceContext _context;
        private readonly IEmailService _emailService;
        public UserDetailsController(ForestProtectionForceContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: api/UserDetailss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetails>>> GetUserDetails()
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            return await _context.UserDetails.OrderByDescending(x=>x.Id).ToListAsync();
        }

        [HttpGet("userTypes")]
        public async Task<ActionResult<IEnumerable<UserTypes>>> GetUserTypes()
        {
            if (_context.UserTypes == null)
            {
                return NotFound();
            }
            return await _context.UserTypes.ToListAsync();
        }

        // GET: api/UserDetailss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetails>> GetUserDetails(int id)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetails = await _context.UserDetails.FindAsync(id);

            if (userDetails == null)
            {
                return NotFound();
            }

            return userDetails;
        }

        // PUT: api/UserDetailss/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserDetails(int id, UserDetails userDetails)
        {
            if (id != userDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailsExists(id))
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

        [HttpPut("lockorunlock{id}")]
        public async Task<IActionResult> lockorUnlockUserDetails(int id, UserDetails userDetails)
        {
            if (id != userDetails.Id)
            {
                return BadRequest();
            }

            _context.Entry(userDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDetailsExists(id))
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

        // POST: api/UserDetailss
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDetails>> PostUserDetails(UserDetails userDetails)
        {
            if (_context.UserDetails == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            userDetails.Created_On = DateTime.Now;
            userDetails.Updated_On = DateTime.Now;
             PasswordHashService passwordHashService = new(_context, _emailService);
             string randomPassword = passwordHashService.GenerateRandomPassword();
            userDetails.Password = randomPassword;
            passwordHashService.SendRandomPasswordToUser(userDetails);
            userDetails.Password =  await passwordHashService.GeneratePasswordAsync(userDetails);
            _context.UserDetails.Add(userDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserDetails", new { id = userDetails.Id }, userDetails);
        }

        [HttpPost("verifyUser")]
        public async Task<ActionResult<UserDetails>> VerifyUser([FromBody] object data)
        {
            if (data == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            JObject jsonData = JObject.Parse(data.ToString());
            string? userPassword = jsonData["password"].ToString();
            string ? userName = jsonData["username"].ToString();
            string otp = jsonData["otp"].ToString();
            var result = await _context.UserDetails.FirstOrDefaultAsync(x=>x.Username == userName).ConfigureAwait(false);
            if (result != null ) { 
            PasswordHashService passwordHashService = new(_context, _emailService);
            bool isAuthenticated = passwordHashService.VerifyPassword(result, userPassword);
           if(isAuthenticated) {
                await passwordHashService.sendOtpAsync(result, otp);
                return result; }
            }
             return null; 
        }

        [HttpPost("resendOtp")]
        public async Task<ActionResult<UserDetails>> ResendOtp([FromBody] object data)
        {
            if (data == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            JObject jsonData = JObject.Parse(data.ToString());
            string? userName = jsonData["username"].ToString();
            string otp = jsonData["otp"].ToString();
            var result = await _context.UserDetails.FirstOrDefaultAsync(x => x.Username == userName).ConfigureAwait(false);
            if (result != null)
            {
                PasswordHashService passwordHashService = new(_context, _emailService);
                await passwordHashService.sendOtpAsync(result, otp);
                return result;
            }
            return result;
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult<UserDetails>> ForgotPassword([FromBody] object data)
        {
            if (data == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            JObject jsonData = JObject.Parse(data.ToString());
            string? userName = jsonData["username"].ToString();
            string email = jsonData["email"].ToString();
            var result = await _context.UserDetails.FirstOrDefaultAsync(x => x.Username == userName && x.Email == email).ConfigureAwait(false);
            if (result != null)
            {
                PasswordHashService passwordHashService = new(_context, _emailService);
                string randomPassword = passwordHashService.GenerateRandomPassword();
                result.Password = randomPassword;
                result.Password = await passwordHashService.GeneratePasswordAsync(result);
                _context.Entry(result).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDetailsExists(result.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                result.Password = randomPassword;
                passwordHashService.SendNewPasswordToUser(result);
                return result;
            }
            return result;
        }

        [HttpPost("changePassword")]
        public async Task<ActionResult<UserDetails>> ChangePassword([FromBody] object data)
        {
            if (data == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            JObject jsonData = JObject.Parse(data.ToString());
            string? userName = jsonData["username"].ToString();
            string password = jsonData["password"].ToString();
            string? newpassword = jsonData["newpassword"].ToString();
            var result = await _context.UserDetails.FirstOrDefaultAsync(x => x.Username == userName).ConfigureAwait(false);
            PasswordHashService passwordHashService = new(_context, _emailService);
            bool isAuthenticated = passwordHashService.VerifyPassword(result, password);
            if (isAuthenticated)
            {
                result.Password = newpassword;
                result.Password = await passwordHashService.GeneratePasswordAsync(result);

                _context.Entry(result).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDetailsExists(result.Id))
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
            return null;
        }


        // DELETE: api/UserDetailss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserDetails(int id)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetails = await _context.UserDetails.FindAsync(id);
            if (userDetails == null)
            {
                return NotFound();
            }

            _context.UserDetails.Remove(userDetails);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpGet("verifyusername")]
        public async Task<ActionResult<UserDetails>> VerifyUserName(string username)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetails = await _context.UserDetails.FirstOrDefaultAsync(x => x.Username == username);

            if (userDetails == null)
            {
                return null;
            }

            return userDetails;
        }

        private bool UserDetailsExists(int id)
        {
            return (_context.UserTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
