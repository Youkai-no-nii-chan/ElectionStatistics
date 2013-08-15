using System.Web.Mvc;
using ElectionInfo.Model;
using System.Linq;

namespace ElectionInfo.WebSite
{
    public class ElectoralDistrictSelectorViewModel : SelectorViewModel
    {
        public int ElectionId { get; set; }
        public int? HigherDistrictId { get; set; }

        public override void LoadData(ModelContext context)
        {
            if (HigherDistrictId == null)
            {
                HigherDistrictId = context.Elections.GetById(ElectionId).ElectoralDistrictId;
            }

            var electoralDistricts = context.ElectoralDistricts
                .ByElection(context, ElectionId)
                .ByHigherDistrict(HigherDistrictId.Value)
                .OrderBy(district => district.Name)
                .ToArray()
                .Select(district => new SelectListItem
                {
                    Text = district.Name,
                    Value = district.Id.ToString(),
                    Selected = SelectedId == district.Id
                });

            Items = new []
                {
                    new SelectListItem
                    {
                        Text = "Все",
                        Value = string.Empty,
                        Selected = SelectedId == null
                    }
                }
                .Concat(electoralDistricts)
                .ToArray();
        }
    }
}