using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using ForestProtectionForce.Services;
using ForestProtectionForce.utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;
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
            string xUserData = HttpContext.Request.Headers["X-User-Data"];
            var user = LogicConvertions.getUserDetails(xUserData ?? "");
            int provinceOfSuperAdmins = LogicConvertions.getSuperAdminOfProvince(user);
            var userDetails = _context.UserDetails?.FirstOrDefault(x => x.Username == user.username) ?? new UserDetails();
            return await _context.UserDetails.Where(predicateLogicForData(userDetails, provinceOfSuperAdmins)).OrderByDescending(x=>x.Id).ToListAsync();
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

        [HttpGet("GetUserDetailsByUserName{username}")]
        public async Task<ActionResult<UserDetails>> GetUserDetailsByUserName(string userName)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetails = await _context.UserDetails.FirstOrDefaultAsync(x => x.Username == userName);

            if (userDetails == null)
            {
                return NotFound();
            }

            return userDetails;
        }

        [HttpPost("VerifyUserAlreadyExistedWithSameDistrict")]
        public async Task<ActionResult<UserDetails>> VerifyUserAlreadyExistedWithSameDistrict(UserDetails details)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetails = await _context.UserDetails.FirstOrDefaultAsync(x =>  x.Email == details.Email && x.UserType_Id == 4);

            if (userDetails == null)
            {
                return null;
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
            int otp = GenarateOtp();
            var result = await _context.UserDetails.FirstOrDefaultAsync(x=>x.Username == userName).ConfigureAwait(false);
          
            if (result != null ) {
                if (result.IsActive == true)
                {
                    PasswordHashService passwordHashService = new(_context, _emailService);
                    bool isAuthenticated = passwordHashService.VerifyPassword(result, userPassword);
                    if (isAuthenticated)
                    {
                        await UpdateOtp(result.Id, otp, result);
                        await passwordHashService.sendOtpAsync(result, otp);
                        return result;
                    }
                }
                else
                {
                    return result;
                }
              
            }
             return null; 
        }

        [HttpPut("updateOtp")]
        public async Task<IActionResult> UpdateOtp(int id, int otp, UserDetails userDetails)
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
                if (!UserDetailsExists(userDetails.Id))
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

        [HttpPost("resendOtp")]
        public async Task<ActionResult<UserDetails>> ResendOtp([FromBody] object data)
        {
            if (data == null)
            {
                return Problem("Entity set 'ForestProtectionForceContext.UserDetails'  is null.");
            }
            JObject jsonData = JObject.Parse(data.ToString());
            string? userName = jsonData["username"].ToString();
            int otp = GenarateOtp();
            var result = await _context.UserDetails.FirstOrDefaultAsync(x => x.Username == userName).ConfigureAwait(false);
            if (result != null)
            {
                PasswordHashService passwordHashService = new(_context, _emailService);
                await UpdateOtp(result.Id, otp, result);
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
            var result = await _context.UserDetails.FirstOrDefaultAsync(x => x.Username == userName && password == password).ConfigureAwait(false);
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

        [HttpGet("verifyemail")]
        public async Task<ActionResult<UserDetails>> VerifyEmail(string email)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetails = await _context.UserDetails.FirstOrDefaultAsync(x => x.Email == email);

            if (userDetails == null)
            {
                return null;
            }

            return userDetails;
        }

        [HttpGet("verifyphone")]
        public async Task<ActionResult<UserDetails>> VerifyPhone(string phone)
        {
            if (_context.UserDetails == null)
            {
                return NotFound();
            }
            var userDetails = await _context.UserDetails.FirstOrDefaultAsync(x => x.Mobile == phone);

            if (userDetails == null)
            {
                return null;
            }

            return userDetails;
        }

        [NonAction]
        public Expression<Func<UserDetails, bool>> predicateLogicForData(UserDetails userData, int provinceOfSuperAdmins)
        {
            Expression<Func<UserDetails, bool>> condition = null;

            if(userData.UserType_Id == 2)
            {
                return condition = x => x.ProvinceId == userData.ProvinceId;
            }
            else if (userData.UserType_Id == 3 || userData.UserType_Id == 4 )
            {
                return condition = x => x.DistrictId == userData.DistrictId;
            }
            else if ( provinceOfSuperAdmins == 1 || provinceOfSuperAdmins == 2)
            {
                return condition = x => x.ProvinceId == provinceOfSuperAdmins;
            }
            else
            {
                return condition = x => true;
            }
        }

        private bool UserDetailsExists(int id)
        {
            return (_context.UserTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private int GenarateOtp()
        {
            Random random = new Random();
            int otp = random.Next(1000, 10000);
            return otp;
        }
    }
}
