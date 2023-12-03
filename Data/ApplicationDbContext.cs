﻿using BookingTourWebApp_MVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingTourWebApp_MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        #region DbSet
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Plane> Planes { get; set; }
        #endregion

        #region FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Flight
            modelBuilder.Entity<Flight>(entity =>
            {
                entity.ToTable("Flight")
                    .HasKey(f => f.Id);

                entity.Property(f => f.BusinessPrice)
                    .HasColumnType("decimal(18, 2)");

                entity.Property(f => f.EconomyPrice)
                    .HasColumnType("decimal(18, 2)");
            });

            // Plane
            modelBuilder.Entity<Plane>(entity =>
            {
                entity.ToTable("Plane")
                    .HasKey(p => p.Id);

                // 1-n Plane-Flight
                entity.HasMany(p => p.Flights) // Một máy bay có nhiều chuyến bay
                    .WithOne(f => f.Plane) // Mỗi chuyến bay thuộc về một máy bay
                    .HasForeignKey(f => f.PlaneId); // Khóa ngoại trong bảng chuyến bay tham chiếu đến khóa chính trong bảng máy bay
            });                

            // Booking n-n AppUser - Flight
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Booking")
                    .HasKey(b => b.Id);
                entity.Property(b => b.TotalPrice)
                    .HasColumnType("decimal(18, 2)");

                entity.HasOne(b => b.AppUser)
                    .WithMany(a => a.Bookings)
                    .HasForeignKey(b => b.AppUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(b => b.Flight)
                    .WithMany(f => f.Bookings)
                    .HasForeignKey(b => b.FlightId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
        #endregion
    }
}
