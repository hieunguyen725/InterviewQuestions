using Interview.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Interview.ViewModels
{
    public class PostAnswerViewModel
    {

        /* Post's related fields*/
        public int PostID { get; set; }

        public DateTime?  CreatedAt { get; set; }

        public ApplicationUser User { get; set; }

        public string PostName { get; set; }

        public ICollection<PostAnswer> PostAnswers { get; set; }

        public string PostQuestion { get; set; }

        /* PostAnswer's related fields */
        public int PostAnswerID { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Answer { get; set; }
        

    }
}