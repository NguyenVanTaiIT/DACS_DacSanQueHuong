using Microsoft.AspNetCore.Mvc;

namespace DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return LocalRedirect("~/Admin/DacSans/Index");
        }
    }
}
