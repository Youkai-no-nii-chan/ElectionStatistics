using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite
{
    public class CharacteristicsDistributionViewModel
    {
        public CharacteristicsDistributionViewModel()
        {
            Elections = new ElectionSelectorViewModel();
            ElectoralDistricts = new List<ElectoralDistrictSelectorViewModel>();
            DistributionValue = DistributionValue.VotersCount;
        }

        public ElectionSelectorViewModel Elections { get; set; }
        public List<ElectoralDistrictSelectorViewModel> ElectoralDistricts { get; set; }

        public DistributionValue DistributionValue { get; set; }
        public ICollection<SelectListItem> DistributionValues { get; set; }

        public void LoadData(ModelContext context)
        {
            Elections.LoadData(context);

            if (ElectoralDistricts.Count == 0)
            {
                ElectoralDistricts.Add(new ElectoralDistrictSelectorViewModel
                {
                    ElectionId = Elections.SelectedId.Value
                });
            }

            foreach (var model in ElectoralDistricts)
            {
                model.LoadData(context);
            }

            DistributionValues = Enum.GetValues(typeof (DistributionValue))
                .OfType<DistributionValue>()
                .Select(value => new SelectListItem
                {
                    Text = value.GetDescription(),
                    Value = value.ToString(),
                    Selected = DistributionValue == value
                })
                .ToArray();
        }
    }
}