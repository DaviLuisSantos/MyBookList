using Microsoft.EntityFrameworkCore;
using MyBookList.Models;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace MyBookList.Data
{
        public class AppDbContext : DbContext
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Book> Books { get; set; }
            //public DbSet<Library> Libraries { get; set; }
            public DbSet<UserBook> UserBooks { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configurações adicionais do modelo podem vir aqui

                modelBuilder.Entity<UserBook>()
                   .HasKey(ub => ub.UserBookId);

                modelBuilder.Entity<UserBook>()
                    .HasOne(ub => ub.User)
                    .WithMany()
                    .HasForeignKey(ub => ub.UserId)
                     .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<UserBook>()
                    .HasOne(ub => ub.Book)
                    .WithMany()
                    .HasForeignKey(ub => ub.BookId)
                     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasData(new User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Email="admin@gmail.com",
                    Activated = true
                }
                );


            }
        }
    }
