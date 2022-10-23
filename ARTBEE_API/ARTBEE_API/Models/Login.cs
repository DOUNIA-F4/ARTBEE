using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ARTBEE_API.Models
{
    public class Login
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password), Required, JsonIgnore]
        public string Password { get; set; }

    }
}
