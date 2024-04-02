using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace diplom_api.Models
{
    public partial class kursach_diplomContext : DbContext
    {
        public kursach_diplomContext()
        {
        }

        public kursach_diplomContext(DbContextOptions<kursach_diplomContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<Logerin> Logs { get; set; } = null!;
        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
        public virtual DbSet<Tour> Tours { get; set; } = null!;
        public virtual DbSet<TouristRoute> TouristRoutes { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.IdBooking);

                entity.ToTable("Booking");

                entity.Property(e => e.IdBooking).HasColumnName("ID_Booking");

                entity.Property(e => e.BookingDate)
                    .HasColumnType("date")
                    .HasColumnName("Booking_date");

                entity.Property(e => e.BookingStatus)
                    .HasMaxLength(7)
                    .IsUnicode(false)
                    .HasColumnName("Booking_status");

                entity.Property(e => e.NumberOfBooked)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Number_of_booked");

                entity.Property(e => e.ReservationNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Reservation_number");

                entity.Property(e => e.StarteDate)
                    .HasColumnType("date")
                    .HasColumnName("Starte_date");

                entity.Property(e => e.ToursId).HasColumnName("Tours_ID");

                entity.Property(e => e.UsersId).HasColumnName("Users_ID");


            });

            modelBuilder.Entity<Logerin>(entity =>
            {
                entity.HasKey(e => e.IdLogs);

                entity.Property(e => e.IdLogs).HasColumnName("ID_Logs");

                entity.Property(e => e.Exception).IsUnicode(false);

                entity.Property(e => e.Levels).IsUnicode(false);

                entity.Property(e => e.Message).IsUnicode(false);

                entity.Property(e => e.TimeDate)
                    .HasColumnType("date")
                    .HasColumnName("Time_date");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.HasKey(e => e.IdReviews);

                entity.Property(e => e.IdReviews).HasColumnName("ID_Reviews");

                entity.Property(e => e.DateReviews)
                    .HasColumnType("date")
                    .HasColumnName("Date_Reviews");

                entity.Property(e => e.Grade)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MailUsers)
                    .IsUnicode(false)
                    .HasColumnName("Mail_Users");

                entity.Property(e => e.ReviewDescription)
                    .IsUnicode(false)
                    .HasColumnName("Review_Description");

                entity.Property(e => e.UsersId).HasColumnName("Users_ID");


            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.ToTable("Role");

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.NameRole)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Name_Role");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.HasKey(e => e.IdToken);

                entity.ToTable("Token");

                entity.Property(e => e.IdToken).HasColumnName("ID_Token");

                entity.Property(e => e.Token1).IsUnicode(false);

                entity.Property(e => e.TokenDateTime).HasColumnType("date");
            });

            modelBuilder.Entity<Tour>(entity =>
            {
                entity.HasKey(e => e.IdTours);

                entity.Property(e => e.IdTours).HasColumnName("ID_Tours");

                entity.Property(e => e.DescriptionTours)
                    .IsUnicode(false)
                    .HasColumnName("Description_Tours");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("expiration_date");

                entity.Property(e => e.NameTours)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Name_Tours");

                entity.Property(e => e.NumberAvailable)
                    .IsUnicode(false)
                    .HasColumnName("Number_available");

                entity.Property(e => e.Photo).IsUnicode(false);

                entity.Property(e => e.Place).IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewsId).HasColumnName("Reviews_ID");

                entity.Property(e => e.StarteDate)
                    .HasColumnType("date")
                    .HasColumnName("Starte_date");

                entity.Property(e => e.TourType)
                    .IsUnicode(false)
                    .HasColumnName("Tour_type");

                entity.Property(e => e.TouristRoutesId).HasColumnName("Tourist_routes_ID");


            });

            modelBuilder.Entity<TouristRoute>(entity =>
            {
                entity.HasKey(e => e.IdTouristRoutes);

                entity.ToTable("Tourist_routes");

                entity.Property(e => e.IdTouristRoutes).HasColumnName("ID_Tourist_routes");

                entity.Property(e => e.Attractions)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Duration)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PlacesVisited)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Places_visited");

                entity.Property(e => e.RouteDescription)
                    .IsUnicode(false)
                    .HasColumnName("Route_description");

                entity.Property(e => e.TokenDateTime).HasColumnType("date");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUsers);

                entity.Property(e => e.IdUsers).HasColumnName("ID_Users");

                entity.Property(e => e.FirstNameUsers)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("First_Name_Users");

                entity.Property(e => e.LoginUsers)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("Login_Users");

                entity.Property(e => e.MailUsers)
                    .IsUnicode(false)
                    .HasColumnName("Mail_Users");

                entity.Property(e => e.MiddleNameUsers)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Middle_Name_Users")
                    .HasDefaultValueSql("('-')");

                entity.Property(e => e.PasswordUsers)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("Password_Users");

                entity.Property(e => e.RoleId).HasColumnName("Role_ID");

                entity.Property(e => e.Salt)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.SecondNameUsers)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("Second_Name_Users");


            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
