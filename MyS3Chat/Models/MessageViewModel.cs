using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyS3Chat.Models
{
    public class MessageViewModel
    {
        public int ID { get; set; }

        public int From { get; set; }

        public int To { get; set; }

        public string Msg { get; set; }

        public DateTime DateSent { get; set; }

        public bool Read { get; set; }

        public UserViewModel SentFromProfile { get; set; }
    }
}