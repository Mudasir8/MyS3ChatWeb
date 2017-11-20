using MyS3Chat.Library.Services;
using MyS3Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MyS3Chat.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController() { }

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
            
        }



        #region Index

      
        public ActionResult Index()
        {
            var loginViewModel = new LoginViewModel();
            return View(loginViewModel);
        }


        // Post for login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {

                string userName = accountService.Login(model);
                if (userName != null)
                {
                    FormsAuthentication.SetAuthCookie(userName, false);
                    return Redirect("~/" + userName);

                }
                else
                {
                    ModelState.AddModelError("", "Username or password does not match");

                    return View(model);

                }

            }
            else
            {
                ModelState.AddModelError("", "Username or password does not match");

                return View(model);

            }




        }

        #endregion

        #region Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterViewModel registerViewModel = new RegisterViewModel();
            return View(registerViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                accountService.RegisterNewUser(registerViewModel);
                TempData["welcome"] = string.Format("Welcome to MyS3Chat.");
                return Redirect("~/" + registerViewModel.UserName);
            }
            return View(registerViewModel);
        }


        #endregion



        #region Logout

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect("~/Account");
        }

        #endregion



        #region ForgotPassword
        // view for forgorpassword
        public ActionResult ForgotPassword()
        {
            var model = new ForgotPasswordViewModel();
            return View(model);
        }




        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // check if user exists
                    var user = accountService.GetUserWithEmail(model.Email);


                    if (user == null)
                    {
                        // Don't reveal that the user does not exist 
                        return View("EmailSent");

                    }



                    var token = new StringBuilder();

                    // prepare a 10 characters random text

                    using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
                    {
                        var data = new byte[4];
                        for (int i = 0; i < 10; i++)
                        {
                            // fill with an array of random 10
                            rngCsp.GetBytes(data);
                            // convert in to a 10 character b/w A - Z
                            var randomChar = Convert.ToChar(BitConverter.ToUInt32(data, 0) % 26 + 65);
                            token.Append(randomChar);

                        }
                    }

                    // password reset identifier will be sent to user by email 
                    var tokenId = token.ToString();
                    string finalToken = user.UserName + tokenId;
                    if (user != null)
                    {
                        // save token in db
                        accountService.SaveToken(user.Email, finalToken);

                        //send token to user
                        var gmailSender = new MailAddress("noreply@mys3chat.com", "MyS3Chat"); // replace with noreply@mys3chat.com
                        var gmailReciever = new MailAddress(user.Email, "MyS3Chat"); //replace with reciever email

                        string subject = "MyS3Chat - Password Reset";
                        string body = "Click the link to reset your password " +
                                        "http://www.mys3chat.com/Account/ResetPassword/" + finalToken;

                        var smtp = new SmtpClient
                        {
                            Host = "smtp.gmail.com",
                            Port = 587,
                            EnableSsl = true,
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            UseDefaultCredentials = false,
                            Credentials = new NetworkCredential("taybovag@gmail.com", "livehappy28")
                        };
                        using (var mess = new MailMessage(gmailSender, gmailReciever)
                        {
                            Subject = subject,
                            Body = body

                        })
                            smtp.Send(mess);

                        


                    }

                    return View("EmailSent");


                }

                catch (Exception e)
                {
                    return View("EmailSent");

                }
               
            }
            return View("EmailSent");

        }


        //// get when user click on email
        [HttpGet]
        public ActionResult ResetPassword(string token)
        {
            // get tknviewmodel
            TokenViewModel tkn = accountService.GetToken(token);

            if(tkn == null)
            {
                return Redirect("~/");

            }

            // if token exists retun userviewmodel to reset password
            UserViewModel user = accountService.GetUserWithEmail(tkn.Email);
            ProfileEditViewModel vm = new ProfileEditViewModel()
            {
                ID = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = user.Status,
               
               
            };

            // remove tkn from db
            accountService.RemoveToken(tkn.Email, token);

            return View(vm);
        }


        //// post when user click on email
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ProfileEditViewModel vm)
        {
            if(ModelState.IsValid)
            {
                accountService.EditAccount(vm);
                return Redirect("~/");
            }
           else
            {
                ModelState.AddModelError("", "Password do not mactch or link has expired please try again");
                return View(vm);

            }
        }



        #endregion

    }
}