using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MyS3Chat.Models;
using MyS3Chat.Library.Services;
using System.Web.Mvc;

namespace MyS3Chat
{
    [HubName("echo")]
    public class EchoHub : Hub
    {
        


        public void NotifyOfMessage(string friendUserName)
        {
            AccountService accountService = new AccountService();
            MessageService messageService = new MessageService();

            // Get friend id
            UserViewModel friend = accountService.GetUserWithUserName(friendUserName);
            int friendId = friend.ID;



            // Get unread messages
            List<MessageViewModel> unreadMsgsList = messageService.GetUnreadMessages(friendUserName);

            // get all msgs list of friend to pass to friend
            List<MessageViewModel> allMessagesList = messageService.GetMessagesList(friendUserName);

            // filter messages to of friends msgs list
            List<MessageViewModel> filteredMessagesList = new List<MessageViewModel>();

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



            //make chat viewmodel
            ChatViewModel unreadMsgschatVM = new ChatViewModel()
            {
                FriendProfile = friend,
                MessagesList = unreadMsgsList
            };

            // Set clients
            var callerClients = Clients.Caller;
            var otherClients = Clients.Others;

            // Call js function
            callerClients.newMessagesToCaller(unreadMsgschatVM);
            otherClients.newMessagesToOther(HttpContext.Current.User.Identity.Name, friendUserName, unreadMsgschatVM);

            // new message to other at home
            otherClients.newMessagesToOtherAtHome(friendUserName, filteredMessagesList.OrderByDescending(x => x.ID).ToList()); // pass all msgs list of friend





        }

        
        public void GetFriendsCount(int friendId)
        {
            AccountService accountService = new AccountService();
            MessageService messageService = new MessageService();
            FriendService friendService = new FriendService();

           


            // Get user's friends list 
            List<UserViewModel> userFriendsList = friendService.GetUserFriendsList(HttpContext.Current.User.Identity.Name);

            // get friends friends list
            List<UserViewModel> friendFriendsList = friendService.GetFriendsFriendsList(friendId);



         // Set clients
         var callerClients = Clients.Caller;
         var otherClients = Clients.Others;


            // Call js function
            callerClients.refreshFriendsList(userFriendsList);
            otherClients.refreshFriendsList(friendFriendsList);


        }



        public void NewFriendRequestNotification(string friendUserName)
        {
            AccountService accountService = new AccountService();
            MessageService messageService = new MessageService();
            FriendService friendService = new FriendService();

            List<UserViewModel> allFriendsRequestList = new List<UserViewModel>();

          
            
                // get friend request list
                allFriendsRequestList = friendService.GetAllFriendRequests(friendUserName);
            


            // set client
            var otherClients = Clients.Others;

            // call js function
            otherClients.newFriendRequest(allFriendsRequestList,friendUserName);

        }
        


    }
}