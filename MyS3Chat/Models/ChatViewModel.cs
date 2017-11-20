using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyS3Chat.Models
{
    public class ChatViewModel
    {
        public UserViewModel FriendProfile { get; set; }

        public List<MessageViewModel> MessagesList { get; set; }
    }
}