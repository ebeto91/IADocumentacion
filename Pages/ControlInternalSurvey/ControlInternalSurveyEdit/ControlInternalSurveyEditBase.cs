using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ModalOnlyView;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using Microsoft.JSInterop;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ControlInternalSurveyEdit
{

    public class ControlInternalSurveyEditBase : ComponentBase
    {
        public SurveyResponse ModelData = new SurveyResponse();

        public ChangeStatusSurveyDto changeStatusSurveyDtoItem = new ChangeStatusSurveyDto();



        public List<SurveyQuestionOptionDto> ListQuestionOptions = new List<SurveyQuestionOptionDto>();
        public List<Catalog> listCatalogData = new List<Catalog>();
        public List<SelectedItem> listSelectType = new List<SelectedItem>();


        private const long MaxFileSize = 10240000L; // 500 KB
        public bool isPressedAddButton { get; set; }
        public bool isReadyPage { get; set; }

        [Parameter]
        public string surveyId { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ISurveyService _surveyService { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "Edit:Survey";

        public bool allowOnlyIntern { get; set; }
        public bool allowOnlyExtern { get; set; }
        public bool changeOrderEdit { get; set; }
        public int permissionsForUser { get; set; }

        public IEnumerable<SelectedItem> ListItemTipesSelection { get; set; } = new List<SelectedItem>();


        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();


            var isValid = await _validationRouteService.HasAccessRoute(validScopes);

            if (!isValid)
            {
                await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                _navigation.NavigateTo("/login");
                _spinnerService.Hide();
                return;
            }

            string scopes = "Create:Survey,Process:Survey";
            permissionsForUser = await _validationRouteService.HowManyPermissionsHave(scopes);

            var response = await _surveyService.GetSurveyById(surveyId);
            if (response != null && response.response != null && response.response.Success)
            {
                CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
                {
                    Collections = ["SURVEY-TYPE"]
                };

                var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
                listCatalogData = listAllDataCatalog;


                var listItemTipesSelection = new List<SelectedItem>
                {
                    new SelectedItem()
                    {
                        Text = "Selección única",
                        Value = SURVEYQUESTIONTYPE.SINGLE
                    },
                    new SelectedItem()
                    {
                        Text = "Selección múltiple",
                        Value = SURVEYQUESTIONTYPE.MULTIPLE
                    },
                    new SelectedItem()
                    {
                        Text = "Respuesta Breve",
                        Value = SURVEYQUESTIONTYPE.SHORT_RESPONSE
                    }
                };

                ListItemTipesSelection = listItemTipesSelection;

                var model = response.definition;
                ModelData = model;

                if (ModelData.TypeCreation == TYPE_PROCCESS_SURVEY.ALL_SURVEY)
                {
                    allowOnlyIntern = true;
                    allowOnlyExtern = true;
                }

                if (ModelData.TypeCreation == TYPE_PROCCESS_SURVEY.EXTERNAL_SURVEY)
                {
                    allowOnlyExtern = true;
                }
                if (ModelData.TypeCreation == TYPE_PROCCESS_SURVEY.INTERNAL_SURVEY)
                {
                    allowOnlyIntern = true;
                }



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




                isReadyPage = true;

                StateHasChanged();

                _spinnerService.Hide();

            }
            else
            {

                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                goToList();
            }



        }

        #region goTo
        public async Task goToList()
        {
            if (permissionsForUser == 1)
            {
                _navigation.NavigateTo("/participacionMunicipio");
            }
            else
            {
                _navigation.NavigateTo("/encuestas");
            }

        }
        #endregion




        #region open modal upload photo
        [NotNull]
        public ModalOnlyViewItem? ModalOnlyView { get; set; }
        protected async Task ActionComponentFather()
        {
            //await Table.QueryAsync();
        }

        public async Task openModalUploadPhoto(string item)
        {
            await ModalOnlyView.OpenModal(item);
            //todo aquí se guardan las preguntas
            StateHasChanged();
        }
        #endregion

        public string GetIdItem(string type, Guid? id)
        {
            return type + "-" + id;
        }


        #region table

        [NotNull]
        public Table<SurveyAttachedDocumentResponse>? Table { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<SurveyAttachedDocumentResponse>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<SurveyAttachedDocumentResponse> items = new List<SurveyAttachedDocumentResponse>();
            if (ModelData.SurveyAttachedDocuments != null)
            {
                items = ModelData.SurveyAttachedDocuments;

                return new QueryData<SurveyAttachedDocumentResponse>()
                {
                    Items = items,
                    TotalCount = items.Count(),
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
            else
            {
                return new QueryData<SurveyAttachedDocumentResponse>()
                {
                    Items = items,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };

            }





        }

        #endregion

        public async Task sendToAprove()
        {
            _spinnerService.Show();

            ChangeStatusSurveyDto changeStatusSurveyDto = new ChangeStatusSurveyDto()
            {
                Id = ModelData.Id,
                Status = CONFIG_APROVE.PUBLISHED,
            };
            var response = await _surveyService.PostSurveyChangeStatus(changeStatusSurveyDto);

            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información de la encuesta actualizada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                //handleAssociationConfig = new HandleAssociationConfig();
                await goToList();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }
        }
        #region modal
        [NotNull]
        public Modal? ModalRef { get; set; }

        //public async Task CloseModalSend()
        //{
        //    changeStatusSurveyDtoItem = new ChangeStatusSurveyDto();
        //    Thread.Sleep(1000);
        //    await ModalRef.Close();

        //}

        public async Task<bool> CloseModal()
        {

            return true;
        }

        #endregion
        public async Task openModalDenied()
        {
            await ModalRef.Show();
        }

        public async Task sendToDenied()
        {

            _spinnerService.Show();
            StateHasChanged();
            ChangeStatusSurveyDto changeStatusSurveyDto = new ChangeStatusSurveyDto()
            {
                Id = ModelData.Id,
                Status = CONFIG_APROVE.DENIED,
                ReasonDenied = changeStatusSurveyDtoItem.ReasonDenied
            };
            await ModalRef.Close();
            Thread.Sleep(1000);

            var response = await _surveyService.PostSurveyChangeStatus(changeStatusSurveyDto);

            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información de la encuesta actualizada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                //handleAssociationConfig = new HandleAssociationConfig();
                await goToList();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }
        }



        public async Task HandleSelectionChange(IEnumerable<SelectedItem> values, string val)
        {
            var value = values.FirstOrDefault();
            if (value.Value == SURVEYQUESTIONTYPE.SHORT_RESPONSE)
            {
                await _jsRuntime.InvokeAsync<object>("toggleOptionsVisibility", false);
                //question.AllowJustification = true;
                //foreach (var option in question.SurveyQuestionOptions)
                //{
                //    option.Description = question.Title;
                //    option.IsOtherValue = true; // Ocultar la opción
                //}
            }
            else
            {
                await _jsRuntime.InvokeAsync<object>("toggleOptionsVisibility", true);
                //question.AllowJustification = false;
                //foreach (var option in question.SurveyQuestionOptions)
                //{
                //    option.IsOtherValue = false; // Mostrar la opción
                //}
            }
            StateHasChanged();
        }

    }
}
