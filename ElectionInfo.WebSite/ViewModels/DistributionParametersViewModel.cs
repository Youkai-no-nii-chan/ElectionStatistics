using System;
using System.Linq;
using System.Web.Mvc;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite
{
    public class DistributionParametersViewModel
    {
        public int? ElectionId { get; set; }

        public int[] SelectedIds { get; set; }
        public SelectListItem[] Items { get; protected set; }

        public void LoadData(ModelContext context)
        {
            if (ElectionId != null)
            {
                Items = Enum.GetValues(typeof(DistributionParameter))
                        .OfType<DistributionParameter>()
                        .Select(value => new SelectListItem
                        {
                            Text = value.GetDescription(),
                            Value = ((int) value).ToString()
                        })
                    .Concat(context.Candidates
                        .ByElection(context, ElectionId.Value)
                        .ToArray()
                        .Select(candidate => new SelectListItem
                        {
                            Text = "Процент за " + candidate.GenitiveName,
                            Value = candidate.Id.ToString()
                        }))
                    .ToArray();
            }
            else
            {
                Items = new SelectListItem[0];
            }
        }
    }
}