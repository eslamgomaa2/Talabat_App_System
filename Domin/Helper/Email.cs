using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domin.Helper
{
    public class Email
    {
        [Required]
        public string? Body { get; set; }
        [Required]
        public string? MailTo { get; set; }
        [Required]
        public string? Subject { get; set; } 
        
    }
}
