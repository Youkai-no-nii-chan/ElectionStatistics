using System.Web.Mvc;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite.Controllers
{
    public class CharacteristicsDistrubitionController : Controller
    {
        public ActionResult Index(CharacteristicsDistrubitionViewModel model)
        {
            using (var context = new ModelContext())
            {
                model.LoadData(context);
                return View(model);
            }
        }
    }
}
