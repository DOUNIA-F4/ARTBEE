using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace ARTBEE_API.Models
{
    public class Product
    {
        public Product()
        {
         Images= new List<Image>();   
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
