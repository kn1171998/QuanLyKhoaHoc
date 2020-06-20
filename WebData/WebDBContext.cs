//using Microsoft.EntityFrameworkCore;
//using WebModel;

//namespace WebData
//{
//    public class WebDBContext : DbContext
//    {
//        public WebDBContext(DbContextOptions<WebDBContext> options) : base(options)
//        {
//        }

//        public DbSet<MasterList> MasterLists { get; set; }
//        public DbSet<User> Users { get; set; }
//        public DbSet<CourseCategory> CourseCategories { get; set; }
//        public DbSet<Course> Courses { get; set; }
//        public DbSet<CourseLesson> CourseLessons { get; set; }
//        public DbSet<Order> Orders { get; set; }
//        public DbSet<OrderDetail> OrderDetails { get; set; }
//        public DbSet<Cart> Carts { get; set; }

//        public DbSet<Discount> Discounts { get; set; }

//        public DbSet<LessonComment> LessonComments { get; set; }

//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);
//            builder.Entity<Cart>()
//             .HasKey(c => new { c.UserId, c.CourseId });
//            builder.Entity<LessonComment>()
//            .HasKey(c => new { c.UserId, c.CourseId });
//            builder.Entity<OrderDetail>()
//            .HasKey(c => new { c.CourseId, c.OrderId });            
//        }
//    }
//}