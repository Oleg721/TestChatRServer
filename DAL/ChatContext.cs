using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DAL
{
    public class ChatContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ChatContext(DbContextOptions options)
            : base(options) 
        {
            Database.EnsureCreated();
            Console.WriteLine("DB Created");
        }

        internal DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {});

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.GroupName).HasDefaultValue(null);
            });

        }
    }
}
