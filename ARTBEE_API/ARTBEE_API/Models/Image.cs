using System.ComponentModel.DataAnnotations.Schema;

namespace ARTBEE_API.Models
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public byte[] ProductImage { get; set; }
        public Product Product { get; set; }
        public string ProductId { get; set; }
    }
}
