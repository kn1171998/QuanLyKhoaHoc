using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class UserVM
    {
        public int Id { get; set; }
        public string TypeUser { get; set; }
        public string FullName { get; set; }
        public bool? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
      
        public string Password { get; set; }
        [NotMapped] // Does not effect with your database
   
        public string RePassword { get; set; }
        public string Token { get; set; }
        public string Introduction { get; set; }
        public int? FacebookId { get; set; }
        public int? GoogleId { get; set; }
        public int? ImageUrl { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}