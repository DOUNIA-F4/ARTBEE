using Microsoft.AspNetCore.Identity;

namespace ARTBEE_API.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            Products = new List<Product>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string Gendre { get; set; }
        public string Type { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
