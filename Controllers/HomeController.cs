using Microsoft.AspNetCore.Mvc;

namespace CryptoDepositApp.Controllers
{
    public class HomeController : Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return File("~/index.html", "text/html");
        }
    }
}