using MyS3Chat.Library.Services;
using MyS3Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyS3Chat.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IFriendService friendService;

        public ProfileController()
        {

        }

        public ProfileController(IAccountService accountService, IFriendService friendService)
        {
            this.accountService = accountService;
            this.friendService = friendService;
        }

        public ActionResult Index(string userName)
        {
            if (userName.Equals(User.Identity.Name))
            {
                return Redirect("~/" + User.Identity.Name);
            }

            // get logged user
            UserViewModel user = accountService.GetUserWithUserName(User.Identity.Name);

            // viewbag logged user imag path for layout
            ViewBag.LoggedUserImagePath = user.ImagePath;

            //viewbag user firstname for layout
            ViewBag.UserFirstName = user.FirstName;


            // get requested user
            UserViewModel friend = accountService.GetUserWithUserName(userName);

    

            // viewbag friend type
            string areFriends = friendService.AreFriends(user.ID, friend.ID);
            ViewBag.AreFriends = areFriends;


            return View(friend);
        }



        [Authorize]
        [HttpGet]
        public ActionResult Edit(string userName)
        {
            // get user
            UserViewModel user = accountService.GetUserWithUserName(userName);


            // viewbag logged user imag path for layout
            ViewBag.LoggedUserImagePath = user.ImagePath;

            //viewbag user firstname for layout
            ViewBag.UserFirstName = user.FirstName;


            if (user == null)
            {

            }

            ProfileEditViewModel vm = new ProfileEditViewModel()
            {
                ID = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Status = user.Status
            };

          

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Edit(ProfileEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                bool response = accountService.EditAccount(vm);
                if (!response)
                {
                   
                }

                TempData["message"] = string.Format("Profile updated");

                return Redirect("~/Profile/Show");

              
            }
            
               return View(vm);
            
            
        }


        [Authorize]
        public ActionResult Show()
        {
            string userName = User.Identity.Name;
            UserViewModel user = accountService.GetUserWithUserName(userName);

            // viewbag logged user imag path for layout
            ViewBag.LoggedUserImagePath = user.ImagePath;

            //viewbag user firstname for layout
            ViewBag.UserFirstName = user.FirstName;

            return View(user);
        }


    }
}