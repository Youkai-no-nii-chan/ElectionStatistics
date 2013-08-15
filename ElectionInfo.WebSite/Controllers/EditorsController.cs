using System.Web.Mvc;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite.Controllers
{
    public class EditorsController : Controller
    {
        public ActionResult ElectoralDistrictSelector(ElectoralDistrictSelectorViewModel model)
        {
            using (var context = new ModelContext())
            {
                model.LoadData(context);
                return PartialView("~/Views/Shared/EditorTemplates/ElectoralDistrictSelector.cshtml", model);
            }
        }
    }
}
