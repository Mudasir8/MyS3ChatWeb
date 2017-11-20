using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyS3Chat.Models;
using MyS3Chat.Library.DAL;
using MyS3Chat.Library.Entities;

namespace MyS3Chat.Library.Services
{
    public class FriendService : IFriendService
    {

        DataContext db = new DataContext();

        public void AcceptFriendRequest(int friendId)
        {
           

            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // accept friend request
            Friend friend = db.Friends.Where(x => x.User1 == friendId && x.User2 == userId).FirstOrDefault();
            friend.AreFriends = true;
            db.SaveChanges();
        }

        public string AreFriends(int userID,int visitingProfileID)
        {
            var f1 = db.Friends.Where(x => x.User1 == userID && x.User2 == visitingProfileID).FirstOrDefault();
            var f2 = db.Friends.Where(x => x.User2 == userID && x.User1 == visitingProfileID).FirstOrDefault();

            if (f1 == null && f2 == null)
            {
                return "false";
            }

            if (f1 != null)
            {
                if (!f1.AreFriends)
                {
                    return "pending";
                }
            }

            if (f2 != null)
            {
                if (!f2.AreFriends)
                {
                    return "pending";
                }
            }

            return "true";

        }

        public void DeclineFriendRequest(int friendId)
        {

            // get user id
            User user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // delete friend request

            Friend friend = db.Friends.Where(x => x.User1 == friendId && x.User2 == userId).FirstOrDefault();
            db.Friends.Remove(friend);
            db.SaveChanges();
        }

        public List<UserViewModel> GetAllFriendRequests(string userName)
        {

            // get user id
            var user = db.Users.Where(x => x.UserName.Equals(userName)).FirstOrDefault();
            int userId = user.ID;

            // crreate friend requests list
            List<Friend> list = db.Friends.Where(x => x.User2 == userId && x.AreFriends == false).ToList();

            // create users list of friend request

            List<User> userList = new List<User>();

            foreach (var item in list)
            {
                User a = db.Users.Find(item.User1);
                userList.Add(a);

            }

            // create view list
            List<UserViewModel> viewList = new List<UserViewModel>();
            foreach (var item in userList)
            {
                UserViewModel vm = new UserViewModel()
                {
                    DateRegistered = item.DateRegistered,
                    DateUpdated = item.DateUpdated,
                    Email = item.Email,
                    EmailConfirmed = item.EmailConfirmed,
                    FirstName = item.FirstName,
                    ID = item.ID,
                    ImagePath = item.ImagePath,
                    LastName = item.LastName,
                    Password = item.Password,
                    Status = item.Status,
                    StatusUpdatedDate = item.StatusUpdatedDate,
                    VisibleTo = item.VisibleTo,
                    UserName = item.UserName
                };
                viewList.Add(vm);
            }

            return viewList;

        }

        public List<UserViewModel> GetFriendsFriendsList(int friendId)
        {
           

            List<Friend> friendList = db.Friends.Where(x => x.User2 == friendId && x.AreFriends == true ||
                                            x.User1 == friendId && x.AreFriends == true
                                            ).ToList();

            List<User> userList = new List<User>();

            foreach (var item in friendList)
            {
                User u = db.Users.Where(x => x.ID == item.User1 && x.ID != friendId ||
                                        x.ID == item.User2 && x.ID != friendId
                                        ).FirstOrDefault();

                userList.Add(u);

            }

            List<UserViewModel> viewList = new List<UserViewModel>();

            foreach (var item in userList)
            {
                UserViewModel a = new UserViewModel()
                {
                    DateRegistered = item.DateRegistered,
                    DateUpdated = item.DateUpdated,
                    Email = item.Email,
                    EmailConfirmed = item.EmailConfirmed,
                    FirstName = item.FirstName,
                    ID = item.ID,
                    ImagePath = item.ImagePath,
                    LastName = item.LastName,
                    Password = item.Password,
                    Status = item.Status,
                    StatusUpdatedDate = item.StatusUpdatedDate,
                    VisibleTo = item.VisibleTo,
                    UserName = item.UserName
                };

                viewList.Add(a);
            }

            return viewList;

        }

        public List<UserViewModel> GetUserFriendsList(string userName)
        {
            // get user id
            var user = db.Users.Where(x => x.UserName.Equals(userName)).FirstOrDefault();
            int userId = user.ID;

            List<Friend> friendList = db.Friends.Where(x => x.User2 == userId && x.AreFriends == true ||
                                            x.User1 == userId && x.AreFriends == true
                                            ).ToList();

            List<User> userList = new List<User>();

            foreach (var item in friendList)
            {
                User u = db.Users.Where(x => x.ID == item.User1 && x.ID != userId ||
                                        x.ID == item.User2 && x.ID != userId
                                        ).FirstOrDefault();

                userList.Add(u);
               
            }

            List<UserViewModel> viewList = new List<UserViewModel>();

            foreach (var item in userList)
            {
                UserViewModel a = new UserViewModel()
                {
                    DateRegistered = item.DateRegistered,
                    DateUpdated = item.DateUpdated,
                    Email = item.Email,
                    EmailConfirmed = item.EmailConfirmed,
                    FirstName = item.FirstName,
                    ID = item.ID,
                    ImagePath = item.ImagePath,
                    LastName = item.LastName,
                    Password = item.Password,
                    Status = item.Status,
                    StatusUpdatedDate = item.StatusUpdatedDate,
                    VisibleTo = item.VisibleTo,
                    UserName = item.UserName
                };

                viewList.Add(a);
            }

            return viewList;
        }

        public bool RemoveFriend(int userID, int friendId)
        {
            var frnd = db.Friends.Where(x => x.User1 == userID && x.User2 == friendId ||
                                             x.User1 == friendId && x.User2 == userID
                                             ).FirstOrDefault();

            // remove from db
            db.Friends.Remove(frnd);
            db.SaveChanges();
            return true;
                                           
            
        }

        public void SendFriendRequest(string friendUserName)
        {
          

            // get logged in user id
            var user = db.Users.Where(x => x.UserName.Equals(HttpContext.Current.User.Identity.Name)).FirstOrDefault();
            int userId = user.ID;

            // get friend to be id
            var friend = db.Users.Where(x => x.UserName.Equals(friendUserName)).FirstOrDefault();
            int friendId = friend.ID;

            // add dto
            Friend friendM = new Friend()
            {
                User1 = userId,
                User2 = friendId,
                AreFriends = false
                
            };

           

            db.Friends.Add(friendM);

            db.SaveChanges();
        }
    }
}