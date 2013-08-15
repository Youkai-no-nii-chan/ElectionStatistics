using System.Collections.Generic;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite
{
    public class CharacteristicsDistrubitionViewModel
    {
        public CharacteristicsDistrubitionViewModel()
        {
            Elections = new ElectionSelectorViewModel();
            ElectoralDistricts = new List<ElectoralDistrictSelectorViewModel>();
        }

        public ElectionSelectorViewModel Elections { get; set; }
        public List<ElectoralDistrictSelectorViewModel> ElectoralDistricts { get; set; }

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
        }
    }
}