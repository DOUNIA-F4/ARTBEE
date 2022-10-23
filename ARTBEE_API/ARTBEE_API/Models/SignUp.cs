using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ARTBEE_API.Models
{
    public class SignUp
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
        
        public string? Phone { get; set; }

        [Required, JsonIgnore]
        public string Password { get; set; }

        
        public string? Address { get; set; }

        public string? Country { get; set; }

        [Display(Name = "Date of Birth")]
        public DateTime? DoB { get; set; }


        [Display(Name = "Profile photo")]
        public byte[]? Photo { get; set; }
    }
}
