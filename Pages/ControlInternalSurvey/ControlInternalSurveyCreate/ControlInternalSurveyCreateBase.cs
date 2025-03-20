using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office.SpreadSheetML.Y2023.MsForms;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ControlInternalSurveyCreate.ModalUpload;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ControlInternalSurveyCreate.SecondStepSurveyCreateItem;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Holidays.Create;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ControlInternalSurveyCreate
{
    public class ControlInternalSurveyCreateBase : ComponentBase
    {
        public HandleSurveyConfig ModelData = new HandleSurveyConfig();


        public SurveyQuestionDto ModelAddQuestion = new SurveyQuestionDto();

        public SurveyQuestionOptionDto ModelAddSurveyQuestionOptionDto = new SurveyQuestionOptionDto();


        public List<SurveyQuestionOptionDto> ListQuestionOptions = new List<SurveyQuestionOptionDto>();
        public List<Catalog> listCatalogData = new List<Catalog>();
        public List<SelectedItem> listSelectType = new List<SelectedItem>();



        public bool isPressedAddButton { get; set; }
        public bool isReadyPage { get; set; }


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
        private string validScopes { get; } = "Create:Survey";

        public Step? _stepper;

        [NotNull]
        public SecondStepSurveyCreate? secondStepSurveyCreate;
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
            ModelData = new HandleSurveyConfig();

            ModelData.SurveyQuestions = new List<SurveyQuestionDto>();

            var newGuid = Guid.NewGuid();

            var itemQuestion = new SurveyQuestionDto()
            {
                Order = 1,
                Title = "",
                Note = "",
                OrderString = "1",
                Id = newGuid,
                IsForEditable = false,
                IsDefault = true,
            };

            itemQuestion.SurveyQuestionOptions =
            [
                new SurveyQuestionOptionDto()
                {
                    Order = 1,
                    Description = "",
                    OrderString = "1",
                    Id = Guid.NewGuid(),
                },

            ];

            ModelData.SurveyQuestions.Add(itemQuestion);

            ModelData.StartDate = DateTime.Today;
            ModelData.DueRate = DateTime.Today.AddDays(1);

            isReadyPage = true;

            StateHasChanged();

            _spinnerService.Hide();
        }

        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("javascript:history.back()");
        }
        #endregion


        #region add or delete option question
        public async Task addItemOptionQuestion(SurveyQuestionDto context, SurveyQuestionOptionDto itemSurvey)
        {
            var newGuid = Guid.NewGuid();


            var findIndex = context.SurveyQuestionOptions.IndexOf(itemSurvey);

            var itemToAdd = new SurveyQuestionOptionDto()
            {
                Description = "",
                OrderString = "",
                Id = Guid.NewGuid(),
            };

            context.SurveyQuestionOptions.Insert(findIndex + 1, itemToAdd);


            for (int i = 0; i < context.SurveyQuestionOptions.Count; i++)
            {
                var item = context.SurveyQuestionOptions[i];
                item.Order = i + 1;
                item.OrderString = item.Order.ToString();
                context.SurveyQuestionOptions[i] = item;
            }
            StateHasChanged();
        }
        public async Task deleteItemOptionQuestion(SurveyQuestionDto context, SurveyQuestionOptionDto itemSurvey)
        {

            var findIndex = context.SurveyQuestionOptions.IndexOf(itemSurvey);
            if (context.SurveyQuestionOptions.Count == 1)
            {
                await _toastService.Warning("¡Advertencia!", "No puede eliminar las respuestas si solo queda una respuesta de la pregunta", autoHide: true);
                return;
            }


            context.SurveyQuestionOptions.Remove(itemSurvey);
            var format = "image/png";

            for (int i = 0; i < context.SurveyQuestionOptions.Count; i++)
            {
                var item = context.SurveyQuestionOptions[i];



                //var resizeImageFile = await item.NewFileQuestion.RequestImageFileAsync(format, 250, 250);
                //var buffer = new byte[resizeImageFile.Size];
                //await resizeImageFile.OpenReadStream(maxAllowedSize: MaxFileSize).ReadAsync(buffer);

                //var imageDataUrlLink = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                item.Url = item.Url;
                item.NewFileQuestion = item.NewFileQuestion;


                item.Order = i + 1;
                item.OrderString = item.Order.ToString();
                context.SurveyQuestionOptions[i] = item;
            }
            StateHasChanged();

        }

        #endregion



        #region editItem
        public async Task startToEditItem(SurveyQuestionDto item)
        {
            item.IsForEditable = true;
            StateHasChanged();
        }
        public async Task deleteItem(SurveyQuestionDto itemToRemove)
        {
            itemToRemove.IsForEditable = false;

            if (ModelData.SurveyQuestions.Count == 1)
            {
                await _toastService.Warning("¡Advertencia!", "No puede eliminar la pregunta si solo queda una pregunta para la encuesta", autoHide: true);
                return;
            }


            ModelData.SurveyQuestions.Remove(itemToRemove);


            for (int i = 0; i < ModelData.SurveyQuestions.Count; i++)
            {
                var item = ModelData.SurveyQuestions[i];
                item.Order = i + 1;
                item.OrderString = item.Order.ToString();
                ModelData.SurveyQuestions[i] = item;
            }

            StateHasChanged();
        }
        public async Task saveItemAndQuestion(EditContext formContext, SurveyQuestionDto item)
        {
            var validationContext = new ValidationContext(item);
            var validationResults = new List<ValidationResult>();

            //Seteo la primer opción como pregunta y habilito justificación
            if (item.Type == SURVEYQUESTIONTYPE.SHORT_RESPONSE)
            {
                item.AllowJustification = true;
                // Eliminar todas las opciones excepto la primera
                if (item.SurveyQuestionOptions.Count > 1)
                {
                    item.SurveyQuestionOptions = item.SurveyQuestionOptions.Take(1).ToList();
                }
                foreach (var option in item.SurveyQuestionOptions)
                {
                    option.Description = item.Title;
                    option.IsOtherValue = false; // Ocultar la opción
                }
            }

            bool itemIsValid = Validator.TryValidateObject(item, validationContext, validationResults, true);


            if (!itemIsValid)
            {
                return;
                // Aquí puedes manejar los errores de validación si lo necesitas
            }

            if (item.Type != SURVEYQUESTIONTYPE.SHORT_RESPONSE && item.SurveyQuestionOptions.Count <= 1 && !item.AllowOtherValue)
            {
                await _toastService.Warning("¡Advertencia!", "Debe haber al menos más de una opción de respuesta para la pregunta para poder guardar", autoHide: true);
                return;
            }

            if (item.AllowOtherValue && (item.DescriptionOtherValue == null || string.IsNullOrEmpty(item.DescriptionOtherValue.Trim())))
            {
                await _toastService.Warning("¡Advertencia!", "Debe colocar una descripción a la respuesta abierta valor", autoHide: true);
                return;
            }

            foreach (var itemQuestion in item.SurveyQuestionOptions)
            {
                var validationContextQuestion = new ValidationContext(itemQuestion);
                var validationResultsQuestion = new List<ValidationResult>();
                bool itemIsValidQuestion = Validator.TryValidateObject(itemQuestion, validationContextQuestion, validationResultsQuestion, true);

                if (!itemIsValidQuestion)
                {
                    return;
                    // Aquí puedes manejar los errores de validación si lo necesitas
                }
            }


            item.IsForEditable = false;
            StateHasChanged();
        }

        public async Task addItemNewQuestion(SurveyQuestionDto item)
        {
            //item.IsForEditable = false;

            if (item.IsForEditable)
            {
                await _toastService.Warning("¡Advertencia!", "Guarda la pregunta para poder continuar con una nueva", autoHide: true);
                return;
            }
            var findIndex = ModelData.SurveyQuestions.IndexOf(item);



            var newGuid = Guid.NewGuid();
            var itemQuestion = new SurveyQuestionDto()
            {
                Order = 1,
                Title = "",
                OrderString = "1",
                Note = "",
                Id = newGuid,
                IsForEditable = false,
                IsDefault = true,
            };

            itemQuestion.SurveyQuestionOptions =
            [
                new SurveyQuestionOptionDto()
                {
                    Order = 1,
                    Description = "",
                    OrderString = "",
                    Id = Guid.NewGuid(),
                },
            ];

            ModelData.SurveyQuestions.Insert(findIndex + 1, itemQuestion);

            for (int i = 0; i < ModelData.SurveyQuestions.Count; i++)
            {
                var itemQuestionInternal = ModelData.SurveyQuestions[i];
                itemQuestionInternal.Order = i + 1;
                itemQuestionInternal.OrderString = itemQuestionInternal.Order.ToString();
                ModelData.SurveyQuestions[i] = itemQuestionInternal;
            }

            //todo aquí se guardan las preguntas
            StateHasChanged();
        }
        #endregion

        #region change Order questions
        public async Task changeOrderQuestions()
        {
            changeOrderEdit = true;
            await _toastService.Information($"Información", $"Selecciona del campo 'Número de orden' " +
                $"para cambiar el orden de las preguntas y después presiona el botón de 'Guardar Orden'", autoHide: true);
        }

        public async Task saveOrderQuestions()
        {
            changeOrderEdit = false;
            try
            {
                ModelData.SurveyQuestions = ModelData.SurveyQuestions.OrderBy(x => int.Parse(x.OrderString)).ToList();
            }
            catch (Exception)
            {
                await _toastService.Information($"Información", $"Por favor colocar los valores sin punto o coma y que sean solo números, por favor", autoHide: true);
                return;
            }



            for (int i = 0; i < ModelData.SurveyQuestions.Count; i++)
            {
                var itemQuestionInternal = ModelData.SurveyQuestions[i];
                itemQuestionInternal.Order = i + 1;
                itemQuestionInternal.OrderString = itemQuestionInternal.Order.ToString();
                ModelData.SurveyQuestions[i] = itemQuestionInternal;
            }
            await _toastService.Success($"Información", $"Orden actualizado con éxito", autoHide: true);
            //todo aquí se guardan las preguntas
            StateHasChanged();
        }
        #endregion



        public string GetIdItem(string type, Guid? id)
        {
            return type + "-" + id;
        }

        public async Task HandleValidSubmit()
        {

            var isSomeEdit = ModelData.SurveyQuestions.Where(x => x.IsForEditable == true).ToList();

            if (isSomeEdit != null && isSomeEdit.Count > 0)
            {
                await _toastService.Warning("¡Advertencia!", "Guarda la pregunta para poder continuar", autoHide: true);
                return;
            }


            if (ModelData.StartDate == ModelData.DueRate)
            {
                await _toastService.Warning("¡Advertencia!", "Las fechas de inicio y finalización no pueden ser iguales, se debe agregar al menos un día", autoHide: true);
                return;
            }

            _spinnerService.Show();
            await _stepper.Next();




            await secondStepSurveyCreate.UpdateData(permissionsForUser, ModelData, allowOnlyIntern, allowOnlyExtern);

            _spinnerService.Hide();
        }

        public async Task ActionFatherBack()
        {
            _spinnerService.Show();
            ModelData.NewPrincipalImage = null;
            ModelData.UrlPrincipal = null;
            ModelData.SurveyAttachedDocuments = new List<SurveyAttachedDocumentDto>();
            foreach (var item in ModelData.SurveyQuestions)
            {
                foreach (var questionOption in item.SurveyQuestionOptions)
                {
                    questionOption.NewFileQuestion = null;
                    questionOption.Url = null;
                }
            }

             _stepper.Prev();

            _spinnerService.Hide();
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
