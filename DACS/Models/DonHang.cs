using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models;

public partial class DonHang
{
    public string MaDonHang { get; set; } = null!;

    public string? UserId {  get; set; }
    public int SoDonHang { get; set; }

    public DateTime? NgayNhanDon { get; set; }

    public int TongDonHang { get; set; }
    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    [Phone(ErrorMessage ="Định dạng số điện thoại không đúng")]
    public string? SoDienThoai {  get; set; }
    [Required(ErrorMessage ="Địa chỉ không được để trống")]
    [StringLength(500, ErrorMessage = "Địa chỉ được sử dụng tối đa 500 ký tự")]
    public string? DiaChi {  get; set; }
    [Required(ErrorMessage = "Tên không được để trống")]
    [StringLength(250, ErrorMessage = "Tên được sử dụng tối đa 250 ký tự")]
    public string? TenKhachHang {  get; set; }
    public DateTime? NgayGiao { get; set; }
    public string? TinhTrangThanhToan { get; set; }
    
    public IdentityUser? User { get; set; }

    public virtual ICollection<CtdonHang> CtdonHangs { get; set; } = new List<CtdonHang>();

   
}
