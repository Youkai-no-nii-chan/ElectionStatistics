using System.Linq;
using System.Web.Mvc;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite
{
    public class ElectionSelectorViewModel : SelectorViewModel
    {
        public override void LoadData(ModelContext context)
        {
            Items = context.Elections
                .OrderByDescending(election => election.Date)
                .ToArray()
                .Select(election => new SelectListItem
                {
                    Text = election.Name,
                    Value = election.Id.ToString(),
                    Selected = SelectedId == election.Id
                })
                .ToArray();

            if (SelectedId == null)
            {
                var firstItem = Items.First();
                firstItem.Selected = true;
                SelectedId = int.Parse(firstItem.Value);
            }
        }
    }
}