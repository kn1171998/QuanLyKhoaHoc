using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Courses
    {
        public Courses()
        {
            Cart = new HashSet<Cart>();
            Chapter = new HashSet<Chapter>();
            DiscountCourse = new HashSet<DiscountCourse>();
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Content { get; set; }
        public long Price { get; set; }
        public long? PromotionPrice { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? Status { get; set; }
        public int? CategoryId { get; set; }
        public int? UserId { get; set; }
        public bool? IsFree { get; set; }

        public CourseCategories Category { get; set; }
        public Users User { get; set; }
        public ICollection<Cart> Cart { get; set; }
        public ICollection<Chapter> Chapter { get; set; }
        public ICollection<DiscountCourse> DiscountCourse { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
