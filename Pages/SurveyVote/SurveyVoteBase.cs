using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.List;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVote.List;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVote
{
    public class SurveyVoteBase : ComponentBase
    {

        public SurveyInputDto surveyVoteInputFilterDto { get; set; } = new SurveyInputDto();

        public List<Catalog> listCatalogData = new List<Catalog>();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelectType = new List<SelectedItem>();

        [NotNull]
        public ListSurveyVote listSurveyVote { get; set; }
        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();

            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["SURVEY-TYPE"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;

            if (listAllDataCatalog != null && listAllDataCatalog.Count > 0)
            {
                var listStatus = listCatalogData.Where(x => x.Collection == "SURVEY-TYPE");


                foreach (var item in listStatus)
                {
                    listSelectType.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });
                }

                listSelectType.Insert(0, (new SelectedItem { Text = "Ambos", Value = "" }));
            }

            _spinnerService.Hide();

        }

        public async Task OnItemChanged(SelectedItem item)
        {
            surveyVoteInputFilterDto.TypeCreation = TYPE_PROCCESS_SURVEY.EXTERNAL_SURVEY;
            await listSurveyVote.UpdateData(surveyVoteInputFilterDto);

            StateHasChanged();
            // return Task.CompletedTask;
        }

    }
}
