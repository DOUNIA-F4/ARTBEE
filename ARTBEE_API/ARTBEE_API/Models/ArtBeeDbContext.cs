using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ARTBEE_API.Models
{
    public class ArtBeeDbContext : IdentityDbContext<User>
    {
        public ArtBeeDbContext(DbContextOptions<ArtBeeDbContext> options): base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Relationship between User and their products (One-to-Many)
            builder.Entity<Product>()
                .HasOne(x => x.User)
                .WithMany(y => y.Products)
                .HasForeignKey(z => z.UserId);

            // Relationship between product and their images (One-to-Many)
            builder.Entity<Image>()
                .HasOne(x => x.Product)
                .WithMany(y => y.Images)
                .HasForeignKey(z => z.ProductId);

            base.OnModelCreating(builder);
        }
    }
}
