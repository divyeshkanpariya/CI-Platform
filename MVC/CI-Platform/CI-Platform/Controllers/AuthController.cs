using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;


namespace CI_Platform.Controllers
{
    public class AuthController : Controller
    {
        private readonly CiPlatformContext  _db;
        public AuthController(CiPlatformContext db)
        {
            _db = db;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel data)
        {
            if (ModelState.IsValid)
            {
                var userFromDB = _db.Users.Any(u => u.Email == data.Email && u.Password == data.Password);
                if (userFromDB)
                {
                    return RedirectToAction("MissionListing", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Email or Password");
                }
                
            }
            return View(data);
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(RegistrationViewModel data)
        {
            if (ModelState.IsValid)
            {
                if(data.Password != data.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Password and Connfirm Password are not same");
                    return View(data);
                }
                var userFromDB = _db.Users.Any(u => u.Email == data.Email);
                if (userFromDB)
                {
                    ModelState.AddModelError("", "Email is already Used");

                    return View(data);
                }
                var user = new User
                {
                    FirstName= data.FirstName,
                    LastName= data.LastName,
                    Email= data.Email,
                    PhoneNumber= data.PhoneNumber,
                    Password= data.Password,
                   
                };
                _db.Users.Add(user);
                _db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(data);

        }
        public IActionResult Lost_Password()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Lost_Password(Lost_passwordViewModel data)
        {
            if (ModelState.IsValid)
            {

                var userFromDB = _db.Users.Any(u => u.Email == data.Email);
                if (userFromDB)
                {
                    Random rand = new Random();
                    string token = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    SendResetPasswordEmail(data.Email, token);
                    var reset_pwd = new PasswordReset
                    {
                        Email = data.Email,
                        Token = token,
                    };
                    _db.PasswordResets.Add(reset_pwd);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public void SendResetPasswordEmail(string email,string token)
        {
            var link = Url.ActionLink("Reset_Password", "Auth", new { Email = email, Token = token });
            var fromMail = new MailAddress("divpatel5706@gmail.com");
            var frompwd = "nomqsmrwgidwesns";
            var toEmail = new MailAddress(email);

            string subject = "Reset Password";
            string body = "Link for Reser Password <br>"+link;

            

            var smtp = new SmtpClient
            {

                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail.Address, frompwd)
            };

            MailMessage message = new MailMessage(fromMail, toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            smtp.Send(message); 
        }
        public IActionResult Reset_Password()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Reset_Password(string Email, string Token)
        {
            ResetPasswordViewModel rp = new ResetPasswordViewModel()
            {
                Email = Email,
                Token = Token,
            };
            return View();
            
        }
        [HttpPost]
        public IActionResult Reset_Password(ResetPasswordViewModel data)
        {
            if (ModelState.IsValid)
            {
                if (data.Password != data.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Password and Connfirm Password are not same");
                    return View(data);
                }
                var isTokenValid = _db.PasswordResets.Where(u=> u.Email == data.Email).OrderBy(u=>u.Id).LastOrDefault();
                if (isTokenValid ==null)
                {
                    ModelState.AddModelError("", "Link is Invalid");
                    return View(data);
                }else if(isTokenValid.Token != data.Token)
                {
                    ModelState.AddModelError("", "Link is Invalid");
                    return View(data);
                }
                var userFromDB = _db.Users.Where(u => u.Email == data.Email).FirstOrDefault();
                
                if (userFromDB != null)
                {
                    userFromDB.Password = data.Password;
                    userFromDB.UpdatedAt = DateTime.Now;
                }
                _db.Users.Update(userFromDB);
                _db.SaveChanges();
                return Json(data);
            }
            else
            {
                return Json(data);
            }
                

            
        }


    }
}
