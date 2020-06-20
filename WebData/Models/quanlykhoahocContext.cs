﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebData.Models
{
    public partial class quanlykhoahocContext : DbContext
    {
        public quanlykhoahocContext()
        {
        }

        public quanlykhoahocContext(DbContextOptions<quanlykhoahocContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CourseCategories> CourseCategories { get; set; }
        public virtual DbSet<CourseLessons> CourseLessons { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<LessonComments> LessonComments { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<WareHouse> WareHouse { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=quanlykhoahoc;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CourseId });

                entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cart_Courses");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cart_Users");
            });

            modelBuilder.Entity<CourseCategories>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.SeoAlias)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SeoMetaDescription).HasMaxLength(158);

                entity.Property(e => e.SeoMetaKeywords).HasMaxLength(158);

                entity.Property(e => e.SeoTitle).HasMaxLength(250);
            });

            modelBuilder.Entity<CourseLessons>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Attachment).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.SlidePath).HasMaxLength(250);

                entity.Property(e => e.VideoPath).HasMaxLength(250);

                entity.HasOne(d => d.Chapter)
                    .WithMany(p => p.CourseLessons)
                    .HasForeignKey(d => d.ChapterId)
                    .HasConstraintName("FK_CourseLessons_Chapter");
            });

            modelBuilder.Entity<Courses>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Courses_CourseCategories");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Courses_Users");
            });

            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NameChapter).HasMaxLength(200);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Chapter)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK_Chapter_Courses");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CodeDiscount).HasMaxLength(50);

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.DiscountPercent).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.ToDate).HasColumnType("datetime");

                entity.HasOne(d => d.Coures)
                    .WithMany(p => p.Discount)
                    .HasForeignKey(d => d.CouresId)
                    .HasConstraintName("FK_Discount_Courses");
            });

            modelBuilder.Entity<LessonComments>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LessonId });

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.LessonComments)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonComments_CourseLessons");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LessonComments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LessonComments_Users");
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.CourseId });

                entity.Property(e => e.OrderId).ValueGeneratedOnAdd();

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Quantity).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Courses");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK_OrderDetails_Discount");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PayMethod).HasMaxLength(50);

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.Introduction).HasMaxLength(10);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.Token).HasMaxLength(2000);

                entity.Property(e => e.TypeUser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<WareHouse>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.NameFile).HasMaxLength(2000);

                entity.Property(e => e.NameFolder).HasMaxLength(2000);

                entity.Property(e => e.NamePath).HasMaxLength(2000);
            });
        }
    }
}