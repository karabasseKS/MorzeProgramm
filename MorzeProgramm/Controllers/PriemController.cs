using Microsoft.AspNetCore.Mvc;

namespace MorzeProgramm.Controllers
{
    public class PriemController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
