using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyS3Chat.Library.Entities
{
    public class Friend
    {
        [Key]
        public int ID { get; set; }

        public int User1 { get; set; }

        public int User2 { get; set; }

        public bool AreFriends { get; set; }
    }
}