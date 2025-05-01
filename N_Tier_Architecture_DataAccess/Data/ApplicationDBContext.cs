using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using N_Tier_Architecture_Models;

namespace N_Tier_Architecture_DataAccess.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) : base(opt)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        //Data Seeding means Insert Data into Database using Migration Command and this below method
        //used for Data Seeding.
        //This below method used for customization in your Entities i.e. create field and insert
        //customize data in it.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Story" },
                new Category { Id = 3, Name = "Fiction" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Action Game 1",
                    Description = "An exciting action-packed game.",
                    Price = 50,
                    ImageUrl = "imageUrl1.jpg",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Action Game 2",
                    Description = "Another action-packed game.",
                    Price = 60,
                    ImageUrl = "imageUrl2.jpg",
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Name = "Story Book 1",
                    Description = "A compelling storybook for children.",
                    Price = 20,
                    ImageUrl = "imageUrl3.jpg",
                    CategoryId = 2
                },
                new Product
                {
                    Id = 4,
                    Name = "Fiction Novel 1",
                    Description = "A thrilling fiction novel.",
                    Price = 30,
                    ImageUrl = "imageUrl4.jpg",
                    CategoryId = 3
                }
            );
        }
    }
}
