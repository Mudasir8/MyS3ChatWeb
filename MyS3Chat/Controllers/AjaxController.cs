using MyS3Chat.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyS3Chat.Controllers
{
    public class AjaxController : Controller
    {
        private readonly IAccountService accountService;
        private readonly IFriendService friendService;
        private readonly IMessageService messageService;

        public AjaxController(IAccountService accountService, IFriendService friendService, IMessageService messageService)
        {
            this.accountService = accountService;
            this.friendService = friendService;
            this.messageService = messageService;
        }

        // check if email already exists
        [HttpPost]
        public string EmailCheck(string email)
        {
            if (email != "")
            {
                string response = accountService.EmailCheck(email);
                return response;
            }

            else
            {
                return "error";
            }

        }


        // check if username already exists
        [HttpPost]
        public string UserNameCheck(string userName)
        {


            string response = accountService.UserNameCheck(userName);

            return response;
        }



        [HttpPost]
        public JsonResult LiveSearch(string searchText)
        {
            // get list which matches search text
            var list = accountService.GetUsersListForLiveSearch(searchText);

            return Json(list);

        }




        [HttpPost]
        public void AddFriend(string friendUserName)
        {
            // send friend request
            friendService.SendFriendRequest(friendUserName);

        }


        [HttpPost]
        public void AcceptFriendRequest(int friendID)
        {
            // send friend request
            friendService.AcceptFriendRequest(friendID);
        }


        [HttpPost]
        public void DeclineFriendRequest(int friendId)
        {
            // decline friend request
            friendService.DeclineFriendRequest(friendId);
        }



        [HttpPost]
        public void SendMessage(string friend, string message)
        {
            // send message 
            messageService.SendMessage(friend, message);

        }


        [HttpPost]
        public JsonResult UploadImage(int id)
        {
            try
            {
                //upload image
                string response =  accountService.UpdateImage(id);
                if(response == "ok")
                {
                    return Json ( new { Result = "ok" });
                }
                else
                {
                    return Json(new { Result = response });

                }


            }
            catch (Exception e)
            {
                return Json(new { Result = e });

            }

        }



        
        [HttpPost]
        public void RemoveFriend(string friendUserName)
        {
            //get current user id
           int userId = accountService.GetUserWithUserName(User.Identity.Name).ID;

            // get friend id
            int frndId = accountService.GetUserWithUserName(friendUserName).ID;

            // remove
            friendService.RemoveFriend(userId, frndId);
            

        }


        
        [HttpPost]
        public void RemoveConservation(string friendUserName)
        {
            // send message 
            messageService.RemoveConservation(friendUserName);

        }



    }
}