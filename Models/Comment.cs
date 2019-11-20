using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Zilla.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        public virtual ApplicationUser Author { get; set; }

        [Required(ErrorMessage = "baga, ba, text")]
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual Assignment Assignment { get; set; }
    }
}