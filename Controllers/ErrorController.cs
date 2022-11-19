using Microsoft.AspNetCore.Mvc;

namespace WebSite.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("{controller}/{message}")]
        public IActionResult Index(string message)
        {
            ViewBag.message = message;
            return View();
        }
    }
}
