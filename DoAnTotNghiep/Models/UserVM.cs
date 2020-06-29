using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnTotNghiep.Models
{
    public class LoginVM
    {

        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool IsRemember { get; set; }

        public string ErrorMessage { get; set; }
    }
    public class UserVM
    {
        public int Id { get; set; }
        public string TypeUser { get; set; }
        public string FullName { get; set; }
        public bool? Sex { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
      
        public string Password { get; set; }
      
   
        public string RePassword { get; set; }
        public string Token { get; set; }
        public string Introduction { get; set; }
        public long? FacebookId { get; set; }
        public long? GoogleId { get; set; }
        public int? ImageUrl { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}