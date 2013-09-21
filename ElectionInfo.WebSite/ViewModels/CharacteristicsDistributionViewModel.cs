using System;
using System.Collections.Generic;
using System.Web;
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
            DistributionParameters = new DistributionParametersViewModel();
        }

        public CharacteristicsDistributionRequest Request { get; private set; }

        public ElectionSelectorViewModel Elections { get; private set; }
        public List<ElectoralDistrictSelectorViewModel> ElectoralDistricts { get; private set; }

        public DistributionValue DistributionValue { get; private set; }
        public ICollection<SelectListItem> DistributionValues { get; private set; }

        public DistributionParametersViewModel DistributionParameters { get; private set; }

        public double DistributionStepSize { get; set; }

        public int ChartWidth { get; set; }
        public int ChartHeight { get; set; }

        public void LoadData(ModelContext context, CharacteristicsDistributionRequest request)
        {
            Request = request;

            Elections.SelectedId = request.ElectionId;
            Elections.LoadData(context);

            var districtsViewModel = new ElectoralDistrictSelectorViewModel
            {
                ElectionId = Elections.SelectedId.Value,
            };
            districtsViewModel.LoadData(context);
            ElectoralDistricts.Add(districtsViewModel);

            int highestDistrictId = districtsViewModel.HigherDistrictId.Value;
            
            if (request.DistrictId != null)
            {
                var district = context.ElectoralDistricts.GetById(request.DistrictId.Value);
                while (district != null && district.HigherDistrictId != highestDistrictId)
                {
                    if (district.HigherDistrictId == null)
                        throw new HttpException(400, string.Format("Округ с Id={0} не участвовал в выборах с Id={1}", request.DistrictId, request.ElectionId));

                    districtsViewModel = new ElectoralDistrictSelectorViewModel
                    {
                        ElectionId = Elections.SelectedId.Value,
                        HigherDistrictId = district.HigherDistrictId
                    }; 
                    districtsViewModel.LoadData(context);
                    ElectoralDistricts.Insert(ElectoralDistricts.Count, districtsViewModel);

                    district = context.ElectoralDistricts.GetById(district.HigherDistrictId.Value);
                }
            }

            DistributionValue = request.DistributionValue;
            DistributionValues = Enum.GetValues(typeof (DistributionValue))
                .OfType<DistributionValue>()
                .Select(value => new SelectListItem
                {
                    Text = value.GetDescription(),
                    Value = value.ToString(),
                    Selected = DistributionValue == value
                })
                .ToArray();

            DistributionParameters.ElectionId = Elections.SelectedId;
            DistributionParameters.SelectedIds = request.DistributionParameters;
            DistributionParameters.LoadData(context);

            DistributionStepSize = request.DistributionStepSize;
            ChartWidth = request.ChartWidth;
            ChartHeight = request.ChartHeight;
        }
    }
}