using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DACS.Models;

public partial class LoaiDacSan
{
    public int MaLoai { get; set; }
    [Required(ErrorMessage ="Vui lòng nhập tên loại")]
    [StringLength(50, ErrorMessage = "Chỉ được sử dụng tối đa 50 ký tự")]
    public string? TenLoai { get; set; }

    [StringLength(450, ErrorMessage = "Chỉ được sử dụng tối đa 450 ký tự")]
    public string? AnhLoai { get; set; }

    [StringLength(50, ErrorMessage = "Chỉ được sử dụng tối đa 50 ký tự")]
    public string? MoTaLoai { get; set; }

    public virtual ICollection<DacSan> DacSans { get; set; } = new List<DacSan>();
}
