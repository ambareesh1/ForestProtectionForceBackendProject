using ForestProtectionForce.Data;
using ForestProtectionForce.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace ForestProtectionForce.Services
{
    public class PasswordHashService
    {

        private readonly ForestProtectionForceContext _context;
        private readonly IEmailService _emailService;
        public PasswordHashService(ForestProtectionForceContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<string> GeneratePasswordAsync(UserDetails userDetails)
        {
            var user = new ApplicationUser
            {
                UserName = userDetails.Username,
                Email = userDetails.Email,
            };
            var hasher = new PasswordHasher<ApplicationUser>();
            var PasswordHash = hasher.HashPassword(user, userDetails.Password);
            userDetails.Password = PasswordHash;
            return PasswordHash;
        }

        public bool VerifyPassword(UserDetails userDetails, string userPassword)
        {
            // create a new PasswordHasher
            var hasher = new PasswordHasher<ApplicationUser>();

            // get the hashed password from the database
            string hashedPasswordFromDatabase = userDetails.Password;

            // get the user's input password
            string userInputPassword = userPassword;

            // verify the password
            var user = new ApplicationUser
            {
                UserName = userDetails.Username 
               
            };
            var result = hasher.VerifyHashedPassword(user, hashedPasswordFromDatabase, userInputPassword);

            if (result == PasswordVerificationResult.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task sendOtpAsync(UserDetails userDetails, string otp)
        {
            await _emailService.SendEmailAsync(userDetails?.Email, "OTP - Verification",
               "<div style=\"display:table;margin:10px;padding:10px;border:10px solid #f0f0f0;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-weight:normal;color:#777;margin:0 auto;font-size:16px\">\r\n  <div style=\"background:#eee\">\r\n    <img src=\"https://live.staticflickr.com/65535/52766112965_969a20a2f0_b.jpg\" alt=\"Forest Protection Force\" style=\"height:60px\" class=\"CToWUd\" data-bit=\"iit\">\r\n  </div>\r\n  <div style=\"text-align:center;padding:0px\">\r\n    <p style=\"text-align:left;padding-left:20px\"> Dear <span>\r\n        <b>"+userDetails.First_Name+" "+userDetails.Last_Name+",</b>\r\n      </span>\r\n    </p>\r\n  </div>\r\n  <div style=\"margin-top:10px;font-size:14px!important;line-height:24px;color:#46535e;padding:0 20px 30px;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;background-color:#fff;font-weight:normal;padding-top:2px;border-radius:2px\">\r\n    <p>As part of our security measures, we have implemented an OTP authentication process to ensure the safety of your account.</p>\r\n    <p>To proceed with the login process, please use the following OTP pin: <b> "+otp+" </b>\r\n    </p>\r\n    <p>This OTP pin will only be valid for a limited time, so please ensure that you use it as soon as possible.</p>\r\n    <p>If you did not initiate this login request, please ignore this email and take appropriate action to secure your account.</p>\r\n    <p>Sincerely, <br>Forest Protection Force Team </p>\r\n  </div>\r\n  <div style=\"padding:5px; background:#eee; display:flex; padding-left:10px\">\r\n    <div style=\"font-weight:normal; float:left; \">\r\n      <p> If you face any issues, Please contact below: </p>\r\n      <p>\r\n        <b>info@forestprotectionforce.com</b>\r\n      </p>\r\n      <p>\r\n        <b> +91 9000009000 </b>\r\n      </p>\r\n    </div>\r\n    <div style=\" float:right; margin-left:50%\">\r\n      <p> Powered by </p>\r\n      <img src=\"https://live.staticflickr.com/65535/52766196513_94f785f49a_m.jpg\" alt=\"\" style=\"height:60px;width:100px\" />\r\n    </div>\r\n  </div>\r\n</div>");

        }

        public string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            var randomBytes = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            var passwordBuilder = new StringBuilder();
            foreach (var b in randomBytes)
            {
                passwordBuilder.Append(validChars[b % validChars.Length]);
            }
            return  passwordBuilder.ToString();
        }

        public void  SendRandomPasswordToUser(UserDetails userDetails)
        {
            string template = "<div style=\"display:table;margin:10px;padding:10px;border:10px solid #f0f0f0;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-weight:normal;color:#777;margin:0 auto;font-size:16px\">\r\n  <div style=\"background:#eee\">\r\n    <img src=\"https://live.staticflickr.com/65535/52766112965_969a20a2f0_b.jpg\" alt=\"Pick My Service Logo\" style=\"height:60px;width:200px\" class=\"CToWUd\" data-bit=\"iit\">\r\n  </div>\r\n  <div style=\"text-align:center;padding:0px\">\r\n    <p style=\"text-align:left;padding-left:20px\"> Dear <span>\r\n        <b>" + userDetails.First_Name + " " + userDetails.Last_Name + ",</b>\r\n      </span>\r\n    </p>\r\n  </div>\r\n  <div style=\"margin-top:10px;font-size:14px!important;line-height:24px;color:#46535e;padding:0 20px 30px;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;background-color:#fff;font-weight:normal;padding-top:2px;border-radius:2px\">\r\n    <p>We are delighted to inform you that your account has been successfully registered on our portal.\r\n    <p>\r\n    <p>user name : <b> " + userDetails.Username + " </b>\r\n    </p>\r\n    <p> Password : <b> " + userDetails.Password + " </b>\r\n    </p>\r\n    <p>Please keep your login credentials safe and secure as they are essential to accessing your account. For security reasons, we recommend that you change your password on your first login by using the \"Change Password\" feature available on the portal.</p>\r\n    <p>Sincerely, <br>Forest Protection Force Team </p>\r\n  </div>\r\n  <div style=\"padding:5px; background:#eee; display:flex; padding-left:10px\">\r\n    <div style=\"font-weight:normal; float:left; \">\r\n      <p> If you face any issues, Please contact below: </p>\r\n      <p>\r\n        <b>info@forestprotectionforce.com</b>\r\n      </p>\r\n      <p>\r\n        <b> +91 9000009000 </b>\r\n      </p>\r\n    </div>\r\n    <div style=\" float:right; margin-left:50%\">\r\n      <p> Powered by </p>\r\n      <img src=\"https://live.staticflickr.com/65535/52766196513_94f785f49a_m.jpg\" alt=\"\" style=\"height:60px;width:100px\" />\r\n    </div>\r\n  </div>\r\n</div>";
             _emailService.SendEmailAsync(userDetails?.Email, "Account Registration Confirmation", template);
        }

        public void SendNewPasswordToUser(UserDetails userDetails)
        {
            string template = "<div style=\"display:table;margin:10px;padding:10px;border:10px solid #f0f0f0;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-weight:normal;color:#777;margin:0 auto;font-size:16px\">\r\n  <div style=\"background:#eee\">\r\n    <img src=\"https://live.staticflickr.com/65535/52766112965_969a20a2f0_b.jpg\" alt=\"Pick My Service Logo\" style=\"height:60px;width:200px\" class=\"CToWUd\" data-bit=\"iit\">\r\n  </div>\r\n  <div style=\"text-align:center;padding:0px\">\r\n    <p style=\"text-align:left;padding-left:20px\"> Dear <span>\r\n        <b>" + userDetails.First_Name + " " + userDetails.Last_Name + ",</b>\r\n      </span>\r\n    </p>\r\n  </div>\r\n  <div style=\"margin-top:10px;font-size:14px!important;line-height:24px;color:#46535e;padding:0 20px 30px;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;background-color:#fff;font-weight:normal;padding-top:2px;border-radius:2px\">\r\n    <p>We received a request to reset your password for your account with "+userDetails.Username+" on our platform. If you did not make this request, please ignore this email and your account will remain secure. <p>\r\n    <p>user name : <b> " + userDetails.Username + " </b>\r\n    </p>\r\n    <p> Password : <b> " + userDetails.Password + " </b>\r\n    </p>\r\n    <p>Please keep your login credentials safe and secure as they are essential to accessing your account. For security reasons, we recommend that you change your password on your first login by using the \"Change Password\" feature available on the portal.</p>\r\n    <p>Sincerely, <br>Forest Protection Force Team </p>\r\n  </div>\r\n  <div style=\"padding:5px; background:#eee; display:flex; padding-left:10px\">\r\n    <div style=\"font-weight:normal; float:left; \">\r\n      <p> If you face any issues, Please contact below: </p>\r\n      <p>\r\n        <b>info@forestprotectionforce.com</b>\r\n      </p>\r\n      <p>\r\n        <b> +91 9000009000 </b>\r\n      </p>\r\n    </div>\r\n    <div style=\" float:right; margin-left:50%\">\r\n      <p> Powered by </p>\r\n      <img src=\"https://live.staticflickr.com/65535/52766196513_94f785f49a_m.jpg\" alt=\"\" style=\"height:60px;width:100px\" />\r\n    </div>\r\n  </div>\r\n</div>";
            _emailService.SendEmailAsync(userDetails?.Email, "New Password Genarated", template);
        }
    }
}
