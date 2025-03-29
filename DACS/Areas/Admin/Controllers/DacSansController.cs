using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DACS.Models;
using Microsoft.AspNetCore.Hosting;


namespace DACS.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class DacSansController : Controller
	{
		private readonly DbDacSanQueHuongContext _context;
		private readonly IWebHostEnvironment webHostEnvironment;
		public DacSansController(DbDacSanQueHuongContext context, IWebHostEnvironment webHost)
		{
			_context = context;
			webHostEnvironment = webHost;
		}



		public async Task<IActionResult> Index()
		{
			var dbDacSanQueHuongContext = _context.DacSans.Include(d => d.MaLoaiNavigation);
			return View(await dbDacSanQueHuongContext.ToListAsync());
		}



		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.DacSans == null)
			{
				return NotFound();
			}

			var dacSan = await _context.DacSans
				.Include(d => d.MaLoaiNavigation)
				.FirstOrDefaultAsync(m => m.MaDacSan == id);
			if (dacSan == null)
			{
				return NotFound();
			}

			return View(dacSan);
		}



		public IActionResult Create()
		{
			ViewData["MaLoai"] = new SelectList(_context.LoaiDacSans, "MaLoai", "TenLoai");
			return View();
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("MaDacSan,TenDacSan,MoTaDacSan,Anh1,Anh2,Anh3,Gia,Kho,MaLoai,QueHuong")] DacSan dacSan, IFormFile Anh1, IFormFile Anh2, IFormFile Anh3)
		{
			if (ModelState.IsValid)
			{
				dacSan.MaDacSan = _context.GetDacSanSequence();
				if (Anh1 != null && Anh2 != null && Anh3 != null)
				{
					
					dacSan.Anh1 = await SaveImage(Anh1);
					dacSan.Anh2 = await SaveImage(Anh2);
					dacSan.Anh3 = await SaveImage(Anh3);
				}
				_context.Add(dacSan);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["MaLoai"] = new SelectList(_context.LoaiDacSans, "MaLoai", "TenLoai", dacSan.MaLoai);
			return View(dacSan);

		}

		private async Task<string> SaveImage(IFormFile image)
		{
			var savePath = Path.Combine("wwwroot/image", image.FileName); 
			
			using (var fileStream = new FileStream(savePath, FileMode.Create))
			{
				await image.CopyToAsync(fileStream);
			}
			return "/image/" + image.FileName; 
		}

		private void DeleteImage(string urlimage)
		{
			string deletePath = webHostEnvironment.WebRootPath + urlimage;
			if (System.IO.File.Exists(deletePath))
				System.IO.File.Delete(deletePath);
		}




		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.DacSans == null)
			{
				return NotFound();
			}

			var dacSan = await _context.DacSans.FindAsync(id);
			if (dacSan == null)
			{
				return NotFound();
			}
			ViewData["MaLoai"] = new SelectList(_context.LoaiDacSans, "MaLoai", "TenLoai", dacSan.MaLoai);
			return View(dacSan);
		}



		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("MaDacSan,TenDacSan,MoTaDacSan,Anh1,Anh2,Anh3,Gia,Kho,MaLoai,QueHuong")] DacSan dacSan, IFormFile Anh1, IFormFile Anh2, IFormFile Anh3)
		{
			if (id != dacSan.MaDacSan)
			{
				return NotFound();
			}

			ModelState.Remove("Anh1");
			ModelState.Remove("Anh2");
			ModelState.Remove("Anh3");

			if (ModelState.IsValid)
			{
				try
				{
					var existingdacsan = await _context.DacSans.FindAsync(id);
					if (existingdacsan == null)
						return NotFound();
					if (Anh1 != null)
					{
						DeleteImage(existingdacsan.Anh1);
						dacSan.Anh1 = await SaveImage(Anh1);
					}
					else
						dacSan.Anh1 = existingdacsan.Anh1;
					if (Anh2 != null)
					{
						DeleteImage(existingdacsan.Anh2);
						dacSan.Anh2 = await SaveImage(Anh2);
					}
					else
						dacSan.Anh2 = existingdacsan.Anh2;
					if (Anh3 != null)
					{
						DeleteImage(existingdacsan.Anh3);
						dacSan.Anh3 = await SaveImage(Anh3);
					}
					else
						dacSan.Anh3 = existingdacsan.Anh3;
					existingdacsan.TenDacSan = dacSan.TenDacSan;
					existingdacsan.MoTaDacSan = dacSan.MoTaDacSan;
					existingdacsan.Anh1 = dacSan.Anh1;
					existingdacsan.Anh2 = dacSan.Anh2;
					existingdacsan.Anh3 = dacSan.Anh3;
					existingdacsan.Gia = dacSan.Gia;
					existingdacsan.Kho = dacSan.Kho;
					existingdacsan.MaDacSan = dacSan.MaDacSan;
					_context.Update(existingdacsan);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!DacSanExists(dacSan.MaDacSan))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["MaLoai"] = new SelectList(_context.LoaiDacSans, "MaLoai", "TenLoai", dacSan.MaLoai);
			return View(dacSan);
		}



		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.DacSans == null)
			{
				return NotFound();
			}

			var dacSan = await _context.DacSans
				.Include(d => d.MaLoaiNavigation)
				.FirstOrDefaultAsync(m => m.MaDacSan == id);
			if (dacSan == null)
			{
				return NotFound();
			}

			return View(dacSan);
		}




		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.DacSans == null)
			{
				return Problem("Tập thực thể 'DbDacSanQueHuongContext.DacSans' là null.");
			}
			var dacSan = await _context.DacSans.FindAsync(id);
			if (dacSan == null)
			{
				return NotFound();
			}
			_context.DacSans.Remove(dacSan);
			await _context.SaveChangesAsync();
			DeleteImage(dacSan.Anh1);
			DeleteImage(dacSan.Anh2);
			DeleteImage(dacSan.Anh3);
			return RedirectToAction(nameof(Index));
		}

		private bool DacSanExists(int id)
		{
			return (_context.DacSans?.Any(e => e.MaDacSan == id)).GetValueOrDefault();
		}
	}
}
