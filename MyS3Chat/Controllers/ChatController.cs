using MyS3Chat.Library.Services;
using MyS3Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyS3Chat.Controllers
{
    public class ChatController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IFriendService friendService;
        private readonly IMessageService messageService;


        public ChatController(IAccountService accountService, IFriendService friendService, IMessageService messageService)
        {
            this.accountService = accountService;
            this.friendService = friendService;
            this.messageService = messageService;
        }

        public ActionResult Index(string userName)
        {
            var friend = accountService.GetUserWithUserName(userName);

           

            // get user
            UserViewModel user = accountService.GetUserWithUserName(User.Identity.Name);

            // viewbag logged user imag path for layout
            ViewBag.LoggedUserImagePath = user.ImagePath;

            //viewbag user firstname for layout
            ViewBag.UserFirstName = user.FirstName;

            //get messages list between user and friend (user,friend)
            List<MessageViewModel> list = messageService.GetMessagesForChat(userName);

            // make chatviewmodel to return
            ChatViewModel VM = new ChatViewModel()
            {
                FriendProfile = friend,
                MessagesList = list
            };


            // make messages as read
            messageService.MakeMessagesAsRead(userName);
         

            return View(VM);

        }
    }
}