using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class NotFoundController : Controller
    {
      
        public IActionResult Index()
        {
            return View();
        }
        
    }
}