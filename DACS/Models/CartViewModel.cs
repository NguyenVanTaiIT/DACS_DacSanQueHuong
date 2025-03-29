namespace DACS.Models
{
	public class CartViewModel
	{
		public IEnumerable<CartItem> ListCart {  get; set; }=null!;
		public DonHang DonHang { get; set; } = null!;
	}

}
