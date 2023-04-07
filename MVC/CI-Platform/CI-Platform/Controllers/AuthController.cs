using CI_Platform.Models.Models;
using CI_Platform.Models.ViewModels;
using CI_Platform.Repository.Interface;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;


namespace CI_Platform.Controllers
{
    public class AuthController : Controller
    {
        /*private readonly CiPlatformIContext  _db;
        public AuthController(CiPlatformContext db)
        {
            _db = db;
        }*/
        private readonly IRegistrationRepository _registrationDb;
        private readonly ILostPasswordRepository _LostPwdDb;
        private readonly IResetPasswordRepository _ResetPwdDb;
        private readonly ILoginRepository _LoginDb;


        private readonly IRepository<User> _UserDb;

        public AuthController(IRegistrationRepository registrationDb, IRepository<User> UserDb, ILostPasswordRepository lostPwdDb, IResetPasswordRepository resetPwdDb, ILoginRepository loginDb)
        {
            _registrationDb = registrationDb;
            _UserDb = UserDb;
            _LostPwdDb = lostPwdDb;
            _ResetPwdDb = resetPwdDb;
            _LoginDb = loginDb;
        }
        public IActionResult Login()
        {
            LoginViewModel vm = new LoginViewModel();
            this.Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);
            HttpContext.Session.Clear();
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel data)
        {
            if (ModelState.IsValid)
            {
                var userFromDB = _UserDb.ExistUser(u => u.Email == data.Email && u.Password == data.Password);
                if (userFromDB)
                {
                    string username = _LoginDb.getUserName(data.Email);
                    long userid = _LoginDb.getUserId(data.Email);
                    
                    string Avatar = _LoginDb.getUserAvatar(data.Email);
                    HttpContext.Session.SetString("UserName", username);
                    HttpContext.Session.SetString("UserId", Convert.ToString(userid));
                    HttpContext.Session.SetString("UserAvatar", Avatar);
                    return RedirectToAction("MissionListing", "Home");
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid Email or Password");
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
                var userFromDB = _registrationDb.ExistUser(u => u.Email == data.Email);
                if (userFromDB)
                {
                    ModelState.AddModelError("", "Email is already Used");

                    return View(data);
                }
                var user = _registrationDb.NewUser(data);

                _registrationDb.AddNew(user);
                _registrationDb.Save();
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

                var userFromDB = _UserDb.ExistUser(u => u.Email == data.Email);
                if (userFromDB)
                {

                    string token = Guid.NewGuid().ToString();
                    SendResetPasswordEmail(data.Email, token);
                    var reset_pwd = _LostPwdDb.newToken(data, token);
                    _LostPwdDb.AddNew(reset_pwd);
                    _LostPwdDb.Save();
                    TempData["SuccessMessage"] = "Email sent for Password Reset !!!";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("Email", "Email Id does'nt matched with any User");
                }
            }
            return View();
        }

        public void SendResetPasswordEmail(string email, string token)
        {
            var link = Url.ActionLink("Reset_Password", "Auth", new { Email = email, Token = token });
            var fromMail = new MailAddress("divpatel5706@gmail.com");
            var frompwd = "nomqsmrwgidwesns";
            var toEmail = new MailAddress(email);

            string subject = "Reset Password";
            string body = "Link for Reser Password <br>" + link;



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
                var isTokenValid = _ResetPwdDb.IsTokenValid;
                if (isTokenValid == null)
                {

                    ModelState.AddModelError("", "Link is Invalid");
                    return View(data);
                }
                var userFromDB = _UserDb.GetFirstOrDefault(u => u.Email == data.Email);

                if (userFromDB != null)
                {
                    userFromDB.Password = data.Password;
                    userFromDB.UpdatedAt = DateTime.Now;
                }
                _UserDb.Update(userFromDB);
                _UserDb.Save();
                return RedirectToAction("Login");

            }
            else
            {
                //HttpContext.Abort();
                
                
                return RedirectToAction("Login");

            }
        }

        //public IActionResult Logout()
        //{
            
        //    HttpContext.Session.Clear();
        //    //HttpContext.Session.Remove("");
        //    //HttpContext.Abort();
        //    return RedirectToAction("Login", "Auth");
        //}

    }
}
