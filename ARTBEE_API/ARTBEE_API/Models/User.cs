using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ARTBEE_API.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Products = new List<Product>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Profile Photo")]
        public byte[]? ProfilePhoto { get; set; }
        [Display(Name = "Date of Birth")]
        public DateTime? DoB { get; set; }
        public string? Country { get; set; }
        public string? Gender { get; set; }
        public string? Type { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
