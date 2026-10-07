using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence
{
    public static class Seed
    {
        public static void SeedData(ModelBuilder modelBuilder)
        {
            // Category
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "رمان"
                },
                new Category
                {
                    Id = 2,
                    Name = "علمی"
                },
                new Category
                {
                    Id = 3,
                    Name = "تاریخی"
                }
            );

            
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "شازده کوچولو",
                    AuthorId = 1,
                    CategoryId = 1,
                    Price = 250000,
                    Stock = 10
                },
                new Book
                {
                    Id = 2,
                    Title = "کیمیاگر",
                    AuthorId = 1,
                    CategoryId = 1,
                    Price = 300000,
                    Stock = 8
                },
                new Book
                {
                    Id = 3,
                    Title = "صد سال تنهایی",
                    AuthorId = 1,
                    CategoryId = 1,
                    Price = 450000,
                    Stock = 5
                }
            );
            modelBuilder.Entity<Author>().HasData(
    new Author
    {
        Id = 1,
        Name = "آنتوان دو سنت اگزوپری"
    }
);
        }
    }
}