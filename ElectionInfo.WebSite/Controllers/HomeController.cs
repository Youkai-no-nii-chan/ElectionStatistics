using System.Web.Mvc;

namespace ElectionInfo.WebSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contacts()
        {
            return View();
        }
    }
}
