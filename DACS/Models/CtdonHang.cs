using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace DACS.Models;

public partial class CtdonHang
{
    public int MaCtdonHang { get; set; }

    public int? MaDacSan { get; set; }

    public int? SldacSan { get; set; }

    public int? GiaDacSan { get; set; }

    public string MaDonHang { get; set; } = null!;



    [ValidateNever]
    public virtual DacSan MaDacSanNavigation { get; set; } = null!;

    public virtual DonHang MaDonHangNavigation { get; set; } = null!;
}
