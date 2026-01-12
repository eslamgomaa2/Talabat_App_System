using System.ComponentModel.DataAnnotations;

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
