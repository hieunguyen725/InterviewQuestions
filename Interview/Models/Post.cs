using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interview.Models
{
    public class Post
    {
        public int PostID { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Question")]
        [Required]
        public string PostQuestion { get; set; }

        [Display(Name = "Category")]
        public String SelectedCategory { get; set; }

        public virtual ICollection<PostAnswer> PostAnswers { get; set; }

        public int ViewCount { get; set; }

        [Display(Name = "Created At")]
        [Column(TypeName = "datetime2")]
        public DateTime CreatedAt { get; set; }

        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }


    }
}