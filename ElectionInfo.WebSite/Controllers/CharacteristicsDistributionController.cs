using System.Web.Mvc;
using ElectionInfo.Model;
using System.Linq;

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

        public ActionResult Chart(CharacteristicsDistributionViewModel model)
        {
            return PartialView(model);
        }

        public ActionResult ChartImage(CharacteristicsDistributionViewModel model)
        {
            return null;
        }
    }
}
