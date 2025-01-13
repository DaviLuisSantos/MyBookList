using Microsoft.EntityFrameworkCore;
using MyBookList.Models;
using System.Collections.Generic;
using System.Reflection.Emit;


namespace MyBookList.Data
{
        public class ReadingTrackerContext : DbContext
        {
            public ReadingTrackerContext(DbContextOptions<ReadingTrackerContext> options) : base(options)
            {
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Book> Books { get; set; }
            public DbSet<Library> Libraries { get; set; }
            public DbSet<UserBook> UserBooks { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configurações adicionais do modelo podem vir aqui

                modelBuilder.Entity<User>()
                   .HasOne(u => u.Library)
                   .WithOne()
                   .HasForeignKey<User>(u => u.LibraryId)
                   .OnDelete(DeleteBehavior.Cascade);

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
            }
        }
    }
