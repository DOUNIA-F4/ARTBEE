using System.ComponentModel.DataAnnotations.Schema;

namespace ARTBEE_API.Models
{
    public class Dashboard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        //public ICollection<Product> Products { get; set; }
    }
}
