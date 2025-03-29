using DACS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DonHangController : Controller
	{
		private readonly DbDacSanQueHuongContext _db;
		public DonHangController(DbDacSanQueHuongContext db)
		{
			_db = db;
		}
		public async Task<IActionResult> Index()
		{
			IEnumerable<DonHang> hoadon = await _db.DonHangs.ToListAsync();
			if (hoadon == null)
				return NotFound();
			return View(hoadon);
		}

		public async Task<IActionResult> Detail(int iddh)
		{
			var donhang = await _db.DonHangs.FirstOrDefaultAsync(p => p.SoDonHang == iddh);
			if (donhang == null)
				return NotFound();			
			ViewBag.MaHoaDon = donhang.MaDonHang;
			ViewBag.TenKH = donhang.TenKhachHang;
			ViewBag.TongTien = donhang.TongDonHang;
			var ctdonhang = await _db.CtdonHangs.Include("MaDonHangNavigation").Include("MaDacSanNavigation").Where(p => p.MaDonHang == donhang.MaDonHang).ToListAsync();
			if (ctdonhang == null)
				return NotFound();
			return View(ctdonhang);
		}

		[HttpGet]
		public async Task<IActionResult> EditStatus(int iddh)
		{
			DonHang? donhang = await _db.DonHangs.FirstOrDefaultAsync(p => p.SoDonHang == iddh);
			if (donhang == null)
				return NotFound();

			return View(donhang);
		}

		[HttpPost]
		public async Task<IActionResult> EditStatus(DonHang? donhang,int iddh)
		{
			if(ModelState.IsValid)
			{
				_db.DonHangs.Update(donhang);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}	
			
			return View(donhang);
		}



	}
}
