using MyS3Chat.Library.Services;
using MyS3Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyS3Chat.Controllers
{
    public class UserController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IFriendService friendService;
        private readonly IMessageService messageService;


        public UserController()
        {

        }

        public UserController(IAccountService accountService, IFriendService friendService, IMessageService messageService)
        {
            this.accountService = accountService;
            this.friendService = friendService;
            this.messageService = messageService;
        }

        [Authorize]
        public ActionResult UserName()
        {

            // get logged user
            var user = accountService.GetUserWithUserName(User.Identity.Name);

            //viewbag user image path for layout
            ViewBag.LoggedUserImagePath = user.ImagePath;

            //viewbag user firstname for layout
            ViewBag.UserFirstName = user.FirstName;

            // get user friends list
            List<UserViewModel> friendsList = friendService.GetUserFriendsList(User.Identity.Name);

            // get user messages list




            // get all friend requests users view models
            List<UserViewModel> allFriendRequestsList = friendService.GetAllFriendRequests(User.Identity.Name);

            // get all messages
            List<MessageViewModel> allMessagesList = messageService.GetMessagesList(User.Identity.Name);

            // filter mesages from unique users
            List<MessageViewModel> filteredMessagesList = new List<MessageViewModel>();

            try
            {
                int from = allMessagesList.FirstOrDefault().From;
                int tmp = from;
                filteredMessagesList.Add(allMessagesList.Where(x => x.From == tmp).FirstOrDefault());


                foreach (var item in allMessagesList)
                {
                    if (item.From != from)
                    {
                        MessageViewModel a = allMessagesList.Where(x => x.From == from && x.From != tmp).FirstOrDefault();
                        if (a != null)
                        {
                            filteredMessagesList.Add(a);
                        }

                        from = item.From;
                    }

                }


                // return view model

                UserCompleteViewModel vm = new UserCompleteViewModel()
                {
                    User = user,
                    FriendsList = friendsList,
                    FriendRequestsList = allFriendRequestsList,
                    MessagesList = filteredMessagesList.OrderByDescending(a => a.ID).ToList()

                };

                return View(vm);
            }
            catch (Exception)
            {

                UserCompleteViewModel vm = new UserCompleteViewModel()
                {
                    User = user,
                    FriendsList = friendsList,
                    FriendRequestsList = allFriendRequestsList


                };
                return View(vm);
            }



        }
    }
}