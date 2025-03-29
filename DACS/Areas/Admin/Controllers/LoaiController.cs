using DACS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class LoaiController : Controller
	{
		private readonly DbDacSanQueHuongContext _db;
		private readonly IWebHostEnvironment webHostEnvironment;

		public LoaiController(DbDacSanQueHuongContext db, IWebHostEnvironment webHost)
		{
			_db = db;
			webHostEnvironment = webHost;
		}


		public async Task<IActionResult> Index()
		{
			IEnumerable<LoaiDacSan> loai = await _db.LoaiDacSans.ToListAsync();
			if (loai == null)
				return NotFound();
			return View(loai);
		}

		[HttpGet]

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]

		public async Task<IActionResult> Create(LoaiDacSan theloai, IFormFile AnhLoai)
		{
			if (ModelState.IsValid)
			{
				theloai.MaLoai = _db.GetLoaiSequence();
				if (AnhLoai != null)
					theloai.AnhLoai = await SaveImage(AnhLoai);
				await _db.LoaiDacSans.AddAsync(theloai);
				await _db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View();

		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			if (id == 0 || await _db.LoaiDacSans.FindAsync(id) == null)
			{
				return NotFound();
			}
			LoaiDacSan? theloai = await _db.LoaiDacSans.FindAsync(id);
			return View(theloai);

		}

		[HttpPost]

		public async Task<IActionResult> Edit(int id,LoaiDacSan theloai, IFormFile AnhLoai)
		{
			ModelState.Remove("AnhLoai");
			if (id != theloai.MaLoai)
				return NotFound();
			if (ModelState.IsValid)
			{
				var existingLoai = await _db.LoaiDacSans.FindAsync(id);
				if (existingLoai == null)
					return NotFound();
				if (AnhLoai != null)
				{
					DeleteImage(existingLoai.AnhLoai);
					theloai.AnhLoai = await SaveImage(AnhLoai);
				}					
				else
					theloai.AnhLoai = existingLoai.AnhLoai;
				existingLoai.TenLoai = theloai.TenLoai;
				existingLoai.AnhLoai = theloai.AnhLoai;
				existingLoai.MoTaLoai = theloai.MoTaLoai;
				_db.LoaiDacSans.Update(existingLoai);
				await _db.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			return View(theloai);
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
			if(System.IO.File.Exists(deletePath))
				System.IO.File.Delete(deletePath);
		}


		[HttpGet]
		public async Task<IActionResult> DeleteConfirm(int id)
		{
			if (id == 0 || await _db.LoaiDacSans.FindAsync(id)==null)
			{
				return NotFound();
			}

			var theloai = await _db.LoaiDacSans.FindAsync(id);
			return View(theloai);
		}


		[HttpPost]

		public async Task<IActionResult> Delete(int id)
		{
			var theloai = await _db.LoaiDacSans.FindAsync(id);
			if (theloai == null)
			{
				return NotFound();
			}
			_db.LoaiDacSans.Remove(theloai);
			await _db.SaveChangesAsync();
			DeleteImage(theloai.AnhLoai);
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Details(int id)
		{
			if (id == 0 || await _db.LoaiDacSans.FindAsync(id)==null)
			{
				return NotFound();
			}

			var theloai = await _db.LoaiDacSans.FindAsync(id);


			return View(theloai);
		}



	}
}
