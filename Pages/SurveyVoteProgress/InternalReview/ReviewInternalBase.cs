using BlazorSpinner;
using BootstrapBlazor.Components;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Components;
using NPOI.Util;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVoteProgress.InternalReview
{
    public class ReviewInternalBase : ComponentBase
    {

        [Parameter]
        public int totalAcountPages { get; set; }
        public int actualPageIndex { get; set; } = 1;
        [Parameter]
        public GetSurveyListAdminstratorDefinition? listSurvey { get; set; } = new GetSurveyListAdminstratorDefinition();

        public SurveyInputDto surveyInputDto { get; set; } = new SurveyInputDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();



        public List<Catalog> listCatalogData = new List<Catalog>();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ISurveyService _surveyService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }


        [Inject]
        [NotNull]
        private ClipboardService? _clipboardService { get; set; }

        #region goTo
        public async Task goToList()
        {
            //_navigation.NavigateTo("/encuestas");
            _navigation.NavigateTo("javascript:history.back()");


        }
        #endregion
        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();




            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["SURVEY-TYPE", "STATUS-CONFIG-APPROVAL", "TYPE-PROCCESS-SURVEY"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;



            //
            var listStatus = listCatalogData.Where(x => x.Collection == "STATUS-CONFIG-APPROVAL");

            foreach (var item in listStatus)
            {
                listSelectStatus.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectStatus.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            //




            //
            var listSubtype = listCatalogData.Where(x => x.Collection == "SURVEY-TYPE");
            foreach (var item in listSubtype)
            {
                listSelectType.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectType.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            //


            await searchData();
            _spinnerService.Hide();
        }
        #region display

        public string getDisplayData(string valueCatalog)
        {
            if (!string.IsNullOrEmpty(valueCatalog))
            {
                var itemSelect = listCatalogData.FirstOrDefault(x => x.Code == valueCatalog);
                if (itemSelect != null)
                {
                    return itemSelect.DisplayLabel;
                }
            }

            return "Sin asignar";
        }

        public string getDateFormatDisplay(DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }
        #endregion


        #region search
        public async Task searchData()
        {
            _spinnerService.Show();
            actualPageIndex = 1;
            await SearchData();

            _spinnerService.Hide();
        }

        public async Task OnPageClick(int pageIndex)
        {

            _spinnerService.Show();
            actualPageIndex = pageIndex;

            await SearchData();

            _spinnerService.Hide();
        }

        private async Task SearchData()
        {
            surveyInputDto.SkipCount = (actualPageIndex - 1) * 10;
            surveyInputDto.MaxResultCount = 10;


            var responseListSurvey = await _surveyService.GetSurveyAdminstratorFilter(surveyInputDto);
            if (responseListSurvey != null && responseListSurvey.response != null && responseListSurvey.response.Success)

            {
                // carga la data
                listSurvey = responseListSurvey.definition;
                var celling = Math.Ceiling((decimal)responseListSurvey.definition.totalCount / 10);
                totalAcountPages = (int)celling;
                StateHasChanged();
            }
            else
            {
                //error
                var message = responseListSurvey != null && responseListSurvey.response != null ?
                    responseListSurvey.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Error", message, autoHide: true);
            }
        }
        #endregion


        #region create new 

        public async Task createNewInternal()
        {

            _spinnerService.Show();
            _navigation.NavigateTo("/encuestas/crear");
            _spinnerService.Hide();
        }
        #endregion


        #region go to
        public void goToProcessEdit(SurveyResponse surveyResponse)
        {
            _spinnerService.Show();
            _navigation.NavigateTo($"/encuestas/revisar/{surveyResponse.Id}");
            _spinnerService.Hide();
        }

        public void goToProcessDetail(SurveyResponse surveyResponse)
        {
            _navigation.NavigateTo($"/encuestas/detalle/{surveyResponse.Id}");
        }

        public async Task changeStatusPublication(SurveyResponse surveyResponse, string status)
        {
            _spinnerService.Show();

            ChangeStatusSurveyDto changeStatusSurveyDto = new ChangeStatusSurveyDto()
            {
                Id = surveyResponse.Id,
                Status = status,
            };
            var response = await _surveyService.PostSurveyChangeStatus(changeStatusSurveyDto);

            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información de la encuesta actualizada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                await searchData();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }
        }


        public void SurveyOrVoteStats(SurveyResponse item)
        {

            //el ultimo 1 es para el boton de volver para que vuelva a encuesta.
            _navigation.NavigateTo($"/resultados/opinion/{item.Id}");

        }

        public async Task copyLink(SurveyResponse surveyResponse)
        {
            try
            {
                var url = _navigation.Uri;
                var urlCorrect = url.Replace("encuestas", "encuesta");

                await _clipboardService.Copy($"{urlCorrect}/anonima/{surveyResponse.Id}");
                await _toastService.Success("Información", "Enlace copiado para enviar", autoHide: true);

            }
            catch (Exception ex)
            {

            }

        }
        #endregion
    }
}
