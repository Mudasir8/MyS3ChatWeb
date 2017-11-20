using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyS3Chat.Models
{
    public class UserViewModel
    {
        public int ID { get; set; }

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }


        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public DateTime DateRegistered { get; set; }

        public DateTime DateUpdated { get; set; }

        public DateTime StatusUpdatedDate { get; set; }

        public string VisibleTo { get; set; }

        public string Status { get; set; }

        public string ImagePath { get; set; }
    }
}