using System;
using System.Collections.Generic;

namespace WebData.Models
{
    public partial class Users
    {
        public Users()
        {
            Cart = new HashSet<Cart>();
            Courses = new HashSet<Courses>();
            LessonComments = new HashSet<LessonComments>();
            Orders = new HashSet<Orders>();
        }

        public int Id { get; set; }
        public string TypeUser { get; set; }
        public string FullName { get; set; }
        public bool? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Introduction { get; set; }
        public decimal? FacebookId { get; set; }
        public decimal? GoogleId { get; set; }
        public string ImageUrl { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }

        public ICollection<Cart> Cart { get; set; }
        public ICollection<Courses> Courses { get; set; }
        public ICollection<LessonComments> LessonComments { get; set; }
        public ICollection<Orders> Orders { get; set; }
    }
}
