using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyS3Chat.Models
{
    public class UserCompleteViewModel
    {

       public UserViewModel User { get; set; }

       public List<UserViewModel> FriendsList { get; set; }

       public List<UserViewModel> FriendRequestsList { get; set; }

       public List<MessageViewModel> MessagesList { get; set; }





    }
}