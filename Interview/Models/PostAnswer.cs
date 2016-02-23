using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Interview.Models
{
    public class PostAnswer
    {
        public int PostAnswerID { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Answer { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        public int PostID { get; set; }

        public virtual Post Post { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}