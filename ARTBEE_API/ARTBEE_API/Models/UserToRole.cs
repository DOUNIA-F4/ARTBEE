using System.ComponentModel.DataAnnotations;

namespace ARTBEE_API.Models
{
    public class UserToRole
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string roleName { get; set; }
    }
}
