using System.Web.Mvc;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite.Controllers
{
    public class CharacteristicsDistributionController : Controller
    {
        public ActionResult Index(CharacteristicsDistributionViewModel model)
        {
            using (var context = new ModelContext())
            {
                model.LoadData(context);
                return View(model);
            }
        }
    }
}
