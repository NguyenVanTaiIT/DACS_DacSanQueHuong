using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage ="Vui lòng nhập tên ")]
        public string? Name { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập địa chỉ")]
        public string? Address { get; set; }

        
    }
}
