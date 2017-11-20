using MyS3Chat.Library.DAL;
using MyS3Chat.Library.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyS3Chat.Models;

namespace MyS3Chat.Library.Services
{
    public class MessageService : IMessageService
    {
        DataContext db = new DataContext();

        public List<MessageViewModel> GetMessagesForChat(string friendUserName)
        {
            
            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // get friend id
            User friend = db.Users.Where(x => x.UserName.Equals(friendUserName)).FirstOrDefault();
            int friendId = friend.ID;



            List<Message> messages = db.Messages.OrderByDescending(x => x.DateSent).Where(x => x.From == userId && x.To == friendId ||
                                                                                               x.From == friendId && x.To == userId
                                                                      ).ToList();


            List<MessageViewModel> viewList = new List<MessageViewModel>();
            foreach (var item in messages)
            {
                User u = db.Users.Find(item.From);
                UserViewModel vm = new UserViewModel()
                {
                    DateRegistered = u.DateRegistered,
                    DateUpdated = u.DateUpdated,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    FirstName = u.FirstName,
                    ID = u.ID,
                    ImagePath = u.ImagePath,
                    LastName = u.LastName,
                    Password = u.Password,
                    Status = u.Status,
                    StatusUpdatedDate = u.StatusUpdatedDate,
                    VisibleTo = u.VisibleTo,
                    UserName = u.UserName
                };

                MessageViewModel a = new MessageViewModel()
                {
                    DateSent = item.DateSent,
                    From = item.From,
                    ID = item.ID,
                    Msg = item.Msg,
                    Read = item.Read,
                    To = item.To,
                    SentFromProfile = vm
                };
                viewList.Add(a);
            }

            return viewList;


        }



        public List<MessageViewModel> GetMessagesList(string userName)
        {


            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(userName)).FirstOrDefault();
            int userId = user.ID;



            // create a list of  messages
            List<Message> list = db.Messages.Where(x => x.To == userId).OrderByDescending(a => a.ID).ToList();


            // make view list read
            List<MessageViewModel> viewList = new List<MessageViewModel>();

            foreach (var item in list)
            {
                User frnd = db.Users.Find(item.From);

                UserViewModel vF = new UserViewModel()
                {
                    DateRegistered = frnd.DateRegistered,
                    DateUpdated = frnd.DateUpdated,
                    Email = frnd.Email,
                    EmailConfirmed = frnd.EmailConfirmed,
                    FirstName = frnd.FirstName,
                    ID = frnd.ID,
                    ImagePath = frnd.ImagePath,
                    LastName = frnd.LastName,
                    Password = frnd.Password,
                    Status = frnd.Status,
                    StatusUpdatedDate = frnd.StatusUpdatedDate,
                    VisibleTo = frnd.VisibleTo,
                    UserName = frnd.UserName

                };


                MessageViewModel a = new MessageViewModel()
                {
                    DateSent = item.DateSent,
                    From = item.From,
                    Msg = item.Msg,
                    Read = item.Read,
                    ID = item.ID,
                    To = item.To,
                    SentFromProfile = vF
                };

                viewList.Add(a);
            }

            return viewList;

        }

        // returns unread message list between user and friend whoom he chating
        public List<MessageViewModel> GetUnreadMessages(string friendUserName) 
        {
            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // get friend id
            User friend = db.Users.Where(x => x.UserName.Equals(friendUserName)).FirstOrDefault();
            int friendId = friend.ID;


            // make list of unread messages
            List<Message> messages = db.Messages.OrderByDescending(x => x.DateSent).Where(x => x.From == userId && x.To == friendId && x.Read == false ||
                                                                                               x.From == friendId && x.To == userId && x.Read == false
                                                                      ).ToList();


            List<MessageViewModel> viewList = new List<MessageViewModel>();
            foreach (var item in messages)
            {
                User u = db.Users.Find(item.From);
                UserViewModel vm = new UserViewModel()
                {
                    DateRegistered = u.DateRegistered,
                    DateUpdated = u.DateUpdated,
                    Email = u.Email,
                    EmailConfirmed = u.EmailConfirmed,
                    FirstName = u.FirstName,
                    ID = u.ID,
                    ImagePath = u.ImagePath,
                    LastName = u.LastName,
                    Password = u.Password,
                    Status = u.Status,
                    StatusUpdatedDate = u.StatusUpdatedDate,
                    VisibleTo = u.VisibleTo,
                    UserName = u.UserName
                };

                MessageViewModel a = new MessageViewModel()
                {
                    DateSent = item.DateSent,
                    From = item.From,
                    ID = item.ID,
                    Msg = item.Msg,
                    Read = item.Read,
                    To = item.To,
                    SentFromProfile = vm
                };
                viewList.Add(a);
            }

            // make unread as read
            foreach (var item in messages )
            {
                item.Read = true;
            }
            db.SaveChanges();

            return viewList;
        }

        public void MakeMessagesAsRead(string friendUserName)
        {
            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // get friend id
            User friend = db.Users.Where(x => x.UserName.Equals(friendUserName)).FirstOrDefault();
            int friendId = friend.ID;



            List<Message> messages = db.Messages.OrderByDescending(x => x.DateSent).Where(x => x.To == userId && x.From == friendId ).ToList();


            // make as read
            foreach (var item in messages)
            {
                item.Read = true;

            }
            db.SaveChanges();

        }

        public void RemoveConservation(string friendUserName)
        {
            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // get friend id
            User friend = db.Users.Where(x => x.UserName.Equals(friendUserName)).FirstOrDefault();
            int friendId = friend.ID;



            List<Message> messages = db.Messages.OrderByDescending(x => x.DateSent).Where(x => x.From == userId && x.To == friendId ||
                                                                                               x.From == friendId && x.To == userId
                                                                      ).ToList();

            foreach (var item in messages)
            {
                db.Messages.Remove(item);
                db.SaveChanges();
            }
          
        }

        public void SendMessage(string friendUserName, string message)
        {
            

            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // get friend id
            User friend = db.Users.Where(x => x.UserName.Equals(friendUserName)).FirstOrDefault();
            int friendId = friend.ID;

            // save message
            Message msg = new Message()
            {
                From = userId,
                To = friendId,
                Msg = message,
                Read = false,
                DateSent = DateTime.Now
            };


            db.Messages.Add(msg);
            db.SaveChanges();
        }
    }
}