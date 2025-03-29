using DACS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ChiTietSanPhamController : Controller
    {
        private readonly DbDacSanQueHuongContext _context;
        public ChiTietSanPhamController(DbDacSanQueHuongContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(int? id)
        {
            var dacSan = await _context.DacSans
                .Include(d => d.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaDacSan == id);
            if (dacSan == null)
            {
                return NotFound();
            }

            return View(dacSan);
        }
    }
}
