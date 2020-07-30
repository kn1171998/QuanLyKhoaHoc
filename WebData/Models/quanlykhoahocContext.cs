using System;
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

        public virtual DbSet<CourseCategories> CourseCategories { get; set; }
        public virtual DbSet<CourseLessons> CourseLessons { get; set; }
        public virtual DbSet<Courses> Courses { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<Discount> Discount { get; set; }
        public virtual DbSet<DiscountCourse> DiscountCourse { get; set; }
        public virtual DbSet<LessonComments> LessonComments { get; set; }
        public virtual DbSet<MasterList> MasterList { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Sysdiagrams> Sysdiagrams { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=SQL5050.site4now.net;Initial Catalog=DB_A63596_quanlykhoahoc;persist security info=True;User id=DB_A63596_quanlykhoahoc_admin; Password=pqpqPQ111");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CourseCategories>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);
            });

            modelBuilder.Entity<CourseLessons>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.SlidePath).HasMaxLength(250);

                entity.Property(e => e.TypeDocument).HasMaxLength(250);

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

                entity.Property(e => e.FromDate).HasColumnType("datetime");

                entity.Property(e => e.ToDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DiscountCourse>(entity =>
            {
                entity.HasKey(e => new { e.Iddiscount, e.Idcourse });

                entity.Property(e => e.Iddiscount).HasColumnName("IDDiscount");

                entity.Property(e => e.Idcourse).HasColumnName("IDCourse");

                entity.HasOne(d => d.IdcourseNavigation)
                    .WithMany(p => p.DiscountCourse)
                    .HasForeignKey(d => d.Idcourse)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountCourse_Courses");

                entity.HasOne(d => d.IddiscountNavigation)
                    .WithMany(p => p.DiscountCourse)
                    .HasForeignKey(d => d.Iddiscount)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiscountCourse_Discount");
            });

            modelBuilder.Entity<LessonComments>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LessonId });

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.DateComment).HasColumnType("datetime");

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

            modelBuilder.Entity<MasterList>(entity =>
            {
                entity.HasKey(e => e.MasterListCode);

                entity.Property(e => e.MasterListCode)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.MasterListDefault)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MasterListGroup)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MasterListValue1)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MasterListValue2)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MasterListValue3)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.MasterListValue4)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OrderDetails>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.CourseId });

                entity.Property(e => e.OrderId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Courses");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("FK_OrderDetails_Discount");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.PayMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RequestId)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Signature)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<Sysdiagrams>(entity =>
            {
                entity.HasKey(e => e.DiagramId);

                entity.ToTable("sysdiagrams");

                entity.Property(e => e.DiagramId).HasColumnName("diagram_id");

                entity.Property(e => e.Definition).HasColumnName("definition");

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CreatedDate).HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(300);

                entity.Property(e => e.FacebookId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.FullName).HasMaxLength(200);

                entity.Property(e => e.GoogleId).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.ImageUrl).HasMaxLength(2000);

                entity.Property(e => e.Introduction).HasMaxLength(2000);

                entity.Property(e => e.Password).HasMaxLength(300);

                entity.Property(e => e.Token).HasMaxLength(2000);

                entity.Property(e => e.TypeUser)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });
        }
    }
}
