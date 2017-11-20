using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyS3Chat.Models
{
    public class ProfileEditViewModel
    {

        public int ID { get; set; }


        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


       
        [Display(Name = "Status")]
        [StringLength(30, ErrorMessage = "Status cannpt be more than 30t least")]
        public string Status { get; set; }


        //[Display(Name = "Visible To")]
        //public string VisibleTo { get; set; }





        //public IEnumerable<SelectListItem> VisibleToList
        //{
        //    get
        //    {
        //        return new List<SelectListItem>
        //        {
        //               new SelectListItem {Text="Public", Value="Public" },
        //               new SelectListItem {Text="Only Me", Value="Only Me" },
        //               new SelectListItem {Text="Friends", Value="Friends" }
                      
        //        };
        //    }
        //}




    }
}