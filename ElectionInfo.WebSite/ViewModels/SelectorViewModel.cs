using System.Web.Mvc;
using ElectionInfo.Model;

namespace ElectionInfo.WebSite
{
    public abstract class SelectorViewModel
    {
        public int? SelectedId { get; set; }
        public SelectListItem[] Items { get; protected set; }

        public abstract void LoadData(ModelContext context);
    }
}