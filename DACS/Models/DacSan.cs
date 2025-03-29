using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models;

public partial class DacSan
{
    public int MaDacSan { get; set; }

    [StringLength(50,ErrorMessage ="Tên đặc sản chỉ được sử dụng tối đa 50 ký tự")]
    public string TenDacSan { get; set; }=null!;
    [StringLength(450, ErrorMessage = "Mô tả chỉ được sử dụng tối đa 450 ký tự")]
    public string? MoTaDacSan { get; set; }
    [StringLength(500, ErrorMessage = "Tên ảnh 1 chỉ được tối đa 500 ký tự")]
    public string? Anh1 { get; set; }
    [StringLength(500, ErrorMessage = "Tên ảnh 2 chỉ được tối đa 500 ký tự")]
    public string? Anh2 { get; set; }
    [StringLength(500, ErrorMessage = "Tên ảnh 3 chỉ được tối đa 500 ký tự")]
    public string? Anh3 { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Giá phải lớn hơn 1")]
    [Required(ErrorMessage = "Vui lòng nhập giá")]
    public int Gia { get; set; }

    public int? Kho { get; set; }
    [Required(ErrorMessage ="Vui lòng chọn loại")]
    public int MaLoai { get; set; }

    [StringLength(50, ErrorMessage = "Quê hương chỉ được sử dụng tối đa 50 ký tự")]
    [Required(ErrorMessage = "Vui lòng nhập quê hương")]
    public string QueHuong { get; set; } = null!;

    public virtual ICollection<CtdonHang> CtdonHangs { get; set; } = new List<CtdonHang>();
    [ValidateNever]
    public virtual LoaiDacSan MaLoaiNavigation { get; set; } = null!;
}
