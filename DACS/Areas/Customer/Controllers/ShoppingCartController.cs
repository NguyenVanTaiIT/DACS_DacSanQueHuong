using DACS.Models;
using DACS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using DACS.ViewModels;
namespace DACS.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class ShoppingCartController : Controller
	{
		private readonly DbDacSanQueHuongContext context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IVnPayService _vnPayservice;
	  
		public ShoppingCartController (DbDacSanQueHuongContext db, UserManager<IdentityUser> userManager, IVnPayService vnPayService)
		{
			context = db;
			_userManager = userManager;
			_vnPayservice = vnPayService;

		}
		public IActionResult Index()
		{
			var carts = GetCartItems();
			if(carts == null)
			{
				return NotFound();
			}
			ViewBag.TongTien = carts.Sum(p => p.Price * p.Quantity);
			ViewBag.TongSoLuong = carts.Sum(p => p.Quantity);
			return View(carts);

		   
		}

		
		List<CartItem>? GetCartItems()
		{
			string? jsoncart = HttpContext.Session.GetString("cart");
			if (jsoncart != null)
			{
				return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
			}
			return new List<CartItem>();
		}
		void SaveCartSession(List<CartItem> ls)
		{
			string jsoncart = JsonConvert.SerializeObject(ls);
			HttpContext.Session.SetString("cart", jsoncart);
		}
		public async Task<ActionResult> AddToCart(int id)
		{
			DacSan? itemProduct = await context.DacSans.FirstOrDefaultAsync(p => p.MaDacSan == id);
			if (itemProduct == null)
				return BadRequest("Sản phẩm không tồn tại");          
			var carts = GetCartItems();
			if (carts == null)
				return NotFound();
			var findCartItem = carts.FirstOrDefault(p => p.Id.Equals(id));
			if (findCartItem == null)
			{
				findCartItem = new CartItem()
				{
					Id = itemProduct.MaDacSan,
					Name = itemProduct.TenDacSan,
					Image = itemProduct.Anh1,
					Price = itemProduct.Gia,
					Quantity = 1
				};
				carts.Add(findCartItem);
			}
			else
				findCartItem.Quantity++;
			SaveCartSession(carts);
			return RedirectToAction("Index");
		}
		public ActionResult UpdateCart(int id, int quantity)
		{
			if (quantity < 1)
				quantity = 1;
			var carts = GetCartItems();
			if (carts == null)
				return NotFound();
			var findCartItem = carts.FirstOrDefault(p => p.Id == id);
			if (findCartItem == null)
				return NotFound();	
			else if (findCartItem != null)
			{
				findCartItem.Quantity = quantity;
				SaveCartSession(carts);
			}
			return RedirectToAction("Index");
		}
		public ActionResult DeleteCart(int id)
		{
			var carts = GetCartItems();
			if (carts == null)
				return NotFound();
			var findCartItem = carts.FirstOrDefault(p => p.Id == id);
			if (findCartItem != null)
			{
				carts.Remove(findCartItem);
				SaveCartSession(carts);
			}
			return RedirectToAction("Index");
		}

		public ActionResult ClearCart()
		{
			var carts = GetCartItems();
			if (carts == null)
				return NotFound();
			carts.Clear();
			SaveCartSession(carts);

			return RedirectToAction("Index");
		}




		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Order()
		{
			var carts = GetCartItems();
			if (carts == null||!carts.Any())
				return NotFound();
			var aspuser = await _userManager.GetUserAsync(User);
			if (aspuser == null)
				return NotFound();
			var user = await context.ApplicationUsers.FirstOrDefaultAsync(p => p.Id == aspuser.Id);
			if (user == null)
				return NotFound();
			ViewBag.TongTien = carts.Sum(p => p.Price * p.Quantity);
			ViewBag.TongSoLuong = carts.Sum(p => p.Quantity);
			CartViewModel cartViewModel = new CartViewModel
			{
				ListCart = carts,
				DonHang = new DonHang()
			};

			cartViewModel.DonHang.TenKhachHang = user.Name;
			cartViewModel.DonHang.SoDienThoai = user.PhoneNumber;
			cartViewModel.DonHang.DiaChi = user.Address;

			return View(cartViewModel);
			
		}


		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Order(CartViewModel cartViewModel, string payment)
		{
			ModelState.Remove("ListCart");
			ModelState.Remove("DonHang.MaDonHang");
			var carts = GetCartItems();
			if (carts == null)
				return NotFound();
			if (ModelState.IsValid)
			{
				var aspuser = await _userManager.GetUserAsync(User);
				if (aspuser == null)
					return NotFound();
				var user = await context.ApplicationUsers.FirstOrDefaultAsync(p => p.Id == aspuser.Id);
				if (user == null)
					return NotFound();
				int madonhangseq = context.GetDonHangSequence();

				var donhang = new DonHang
				{
					UserId = user.Id,
					MaDonHang = "MDH" + madonhangseq.ToString(),
					SoDonHang = madonhangseq,
					NgayNhanDon = DateTime.UtcNow,
					TongDonHang = carts.Sum(i => i.Price * i.Quantity),
					DiaChi = cartViewModel.DonHang.DiaChi,
					SoDienThoai = cartViewModel.DonHang.SoDienThoai,
					TenKhachHang = cartViewModel.DonHang.TenKhachHang,
					TinhTrangThanhToan = "Đang xác nhận"

				};

				await context.DonHangs.AddAsync(donhang);
				await context.SaveChangesAsync();
				foreach (var item in carts)
				{
					CtdonHang chitietdonhang = new CtdonHang()
					{
						MaCtdonHang = context.GetCTDonHangSequence(),
						MaDonHang = donhang.MaDonHang,
						MaDacSan = item.Id,
						SldacSan = item.Quantity,
						GiaDacSan = item.Price
					};

					await context.CtdonHangs.AddAsync(chitietdonhang);
					await context.SaveChangesAsync();
				}

				HttpContext.Session.Remove("cart");

				if (payment == "VNPAY")
				{

					var vnPayModel = new VnPaymentRequestModel
					{
						Amount = (double)donhang.TongDonHang * 100,
						CreateDate = DateTime.Now,
						Description = $"{donhang.SoDonHang}",
						FullName = donhang.TenKhachHang,
						OrderId = donhang.SoDonHang

					};

					return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
				}


				return View("OrderCompleted", donhang);
			}

			cartViewModel.ListCart = carts;
			ViewBag.TongTien = carts.Sum(p => p.Price * p.Quantity);
			ViewBag.TongSoLuong = carts.Sum(p => p.Quantity);
			return View(cartViewModel);
		}

		public IActionResult OrderCompleted()
		{

			return View();
		}



		[Authorize]
		public IActionResult PaymentFail()
		{
			return View();
		}

		public IActionResult PaymentSuccess()
		{
			return View();
		}




		[Authorize]
		public async Task<IActionResult> PaymentCallBack()
		{
			var response = _vnPayservice.PaymentExecute(Request.Query);
			if (response == null || response.VnPayResponseCode != "00")
			{
				var aspuser = await _userManager.GetUserAsync(User);
				if (aspuser == null)
					return NotFound();
				var donhang = await context.DonHangs.OrderByDescending(p=>p.SoDonHang).FirstOrDefaultAsync(p=>p.UserId==aspuser.Id);
				if (donhang == null)
					return BadRequest();
				donhang.TinhTrangThanhToan = "Lỗi thanh toán VNPay";
				await context.SaveChangesAsync();
				if(response != null)
					TempData["Message"] = $"Lỗi thanh toán VNPay: {response.VnPayResponseCode}";

				return RedirectToAction("PaymentFail");
			}
			if (response.OrderDescription == null)
				return BadRequest();
			int orderid = int.Parse(response.OrderDescription);
			DonHang? order = await context.DonHangs.FirstOrDefaultAsync(p => p.SoDonHang == orderid);
			if (order == null)
				return BadRequest();
			order.TinhTrangThanhToan = "Đã thanh toán VNPay";
			await context.SaveChangesAsync();

			return RedirectToAction("PaymentSuccess");
		}


	}

}
