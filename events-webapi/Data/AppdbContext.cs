using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using events_webapi.Models;
namespace events_webapi.Data 
{
    public class AppdbContext : DbContext
    {
        public AppdbContext (DbContextOptions<AppdbContext> options)
            : base(options)
        {
        }

        public DbSet<events_webapi.Models.Event> Event { get; set; } = default!;

        public DbSet<events_webapi.Models.EventType> EventType { get; set; } = default!;

        public DbSet<User> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);

            // User - Event relationship (1 user : many events)
            modelBuilder.Entity<Event>()
                .HasOne(e => e.User)
                .WithMany(u => u.Events)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // EventType - Event relationship
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Vrsta)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VrstaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}