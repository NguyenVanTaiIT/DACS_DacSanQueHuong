using DACS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly DbDacSanQueHuongContext _context;

		public HomeController(DbDacSanQueHuongContext context)
		{
			_context = context;
		}


		public async Task<IActionResult> Index(string tk, string dm1)
		{
			IEnumerable<DacSan> dacsan = await _context.DacSans.ToListAsync();
			if (dacsan == null)
				return NotFound();
			ViewBag.DacSan = dacsan;
			var loaidacsan = await _context.LoaiDacSans.ToListAsync();
			if (loaidacsan == null)
				return NotFound();
			ViewBag.LoaiDacSan = loaidacsan;
			ViewBag.TitleTK = "Tất cả đặc sản";
			if (!string.IsNullOrEmpty(tk))
			{
				dacsan = await _context.DacSans.Where(p => p.TenDacSan.Contains(tk)||p.QueHuong.Contains(tk)).ToListAsync();
				if(dacsan.Count()==0)
				{
					ViewBag.TitleTK = "Không tìm thấy đặc sản " + tk;
				}
				else
					ViewBag.TitleTK = "Đặc sản " + tk;
				ViewBag.DacSan = dacsan;
			}
			else if (!string.IsNullOrEmpty(dm1))
			{
				LoaiDacSan? loai = await _context.LoaiDacSans.FirstOrDefaultAsync(p => p.TenLoai == dm1);
				if (loai == null)
					return NotFound();
				dacsan = await _context.DacSans.Where(p => p.MaLoai == loai.MaLoai).ToListAsync();
				if(dacsan.Count()==0)
					ViewBag.TitleTK = "Không tìm thấy đặc sản theo các loại yêu cầu";
				else
					ViewBag.TitleTK = dm1 + " các loại";
				ViewBag.DacSan = dacsan;
			}


			return View(dacsan);
		}

		
	}
}
