using System.Web.Mvc;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite.Controllers
{
    public class CharacteristicsDistributionController : Controller
    {
        public ActionResult Index(CharacteristicsDistributionRequest request)
        {
            using (var context = new ModelContext())
            {
                var model = new CharacteristicsDistributionViewModel();
                model.LoadData(context, request);
                return View(model);
            }
        }

        public ActionResult Chart(CharacteristicsDistributionRequest request)
        {
            return PartialView(request);
        }

        public ActionResult ChartImage(CharacteristicsDistributionRequest request)
        {
            var builder = new ParametersDistributionChartBuilder(request);
            using (var context = new ModelContext())
            {
                builder.Build(context);
            }
            return File(builder.Image.ToArray(), "image/png");
        }
    }
}
