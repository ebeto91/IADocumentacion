﻿using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Diagnostics.CodeAnalysis;
using Microsoft.PowerBI.Api.Models;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ControlInternalSurveyEditProcess.SecondStepSurveyEditItem
{
    public class SecondStepSurveyEditBase : ComponentBase
    {

        public HandleSurveyConfig ModelData = new HandleSurveyConfig();
        public SurveyResponse ModelDataConsult = new SurveyResponse();

        public bool allowOnlyIntern { get; set; }
        public bool allowOnlyExtern { get; set; }


        private const long MaxFileSize = 10240000L; // 500 KB

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
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        public bool isReadyPage { get; set; }
        [Parameter]
        public EventCallback ActionChild { get; set; }

        public async Task UpdateData(HandleSurveyConfig _modelData, SurveyResponse _surveyResponse, bool _allowOnlyIntern, bool _allowOnlyExtern)
        {
            isReadyPage = true;
            ModelData = _modelData;
            ModelDataConsult = _surveyResponse;

            allowOnlyIntern = _allowOnlyIntern;
            allowOnlyExtern = _allowOnlyExtern;
            StateHasChanged();
        }


        #region add principalImage
        public async Task OnInputFilePrincipalImageChange(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            var format = "image/png";
            foreach (var file in imageFiles)
            {
                if (file.Size > MaxFileSize)
                {
                    // Mostrar mensaje de error
                    await _toastService.Error($"Ha ocurrido un error con la imagen {file.Name}", $"El archivo excede el tamaño máximo permitido de 10 Mb.", autoHide: true);

                }
                else
                {
                    var resizeImageFile = await file.RequestImageFileAsync(format, 250, 250);
                    var buffer = new byte[resizeImageFile.Size];
                    await resizeImageFile.OpenReadStream(maxAllowedSize: MaxFileSize).ReadAsync(buffer);

                    var imageDataUrlLink = $"data:{format};base64,{Convert.ToBase64String(buffer)}";


                    ModelData.UrlPrincipal = imageDataUrlLink;
                    ModelData.NewPrincipalImage = file;
                }
            }
            StateHasChanged();
        }

        #region delete
        public async void HandleDeleteImage()
        {

            ModelData.UrlPrincipal = null;
            if (ModelData.NewPrincipalImage != null)
            {
                ModelData.NewPrincipalImage = null;
            }


            StateHasChanged();
        }

        #endregion

        #endregion

        #region table

        [NotNull]
        public Table<SurveyAttachedDocumentDto>? Table { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<SurveyAttachedDocumentDto>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<SurveyAttachedDocumentDto> items = new List<SurveyAttachedDocumentDto>();
            if (ModelData.SurveyAttachedDocuments != null)
            {
                items = ModelData.SurveyAttachedDocuments.Where(x => !x.ToDeleted);

                return new QueryData<SurveyAttachedDocumentDto>()
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
                return new QueryData<SurveyAttachedDocumentDto>()
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


        #region add new files

        public async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();

            foreach (var file in imageFiles)
            {
                if (file.Size > MaxFileSize)
                {
                    // Mostrar mensaje de error
                    await _toastService.Error($"Ha ocurrido un error con la imagen {file.Name}", $"El archivo excede el tamaño máximo permitido de 10 MB.", autoHide: true);

                }
                else
                {

                    var format = file.ContentType;
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream(MaxFileSize).ReadAsync(buffer);

                    var imageDataUrlLink = await _jsRuntime.InvokeAsync<string>("generateLink", file.Name, format, buffer);
                    var newId = Guid.NewGuid();
                    SurveyAttachedDocumentDto evidenciaDto = new SurveyAttachedDocumentDto()
                    {
                        Id = newId,
                        FilePath = imageDataUrlLink,
                        FileName = file.Name,
                        ToDeleted = false,
                        MimeType = format,
                        BrowserFile = file
                    };

                    if (ModelData.SurveyAttachedDocuments == null)
                    {
                        ModelData.SurveyAttachedDocuments = new List<SurveyAttachedDocumentDto>();
                    }

                    ModelData.SurveyAttachedDocuments.Add(evidenciaDto);
                    await Table.QueryAsync();
                }
            }
            StateHasChanged();
        }

        #region delete
        public async Task deleteInfo(TableColumnContext<SurveyAttachedDocumentDto, Guid> item)
        {
            _spinnerService.Show();

            var itemIndex = ModelData.SurveyAttachedDocuments.FindIndex(x => x.Id == item.Row.Id);
            if (itemIndex >= 0)
            {
                var itemFound = ModelData.SurveyAttachedDocuments[itemIndex];
                itemFound.ToDeleted = true;
                ModelData.SurveyAttachedDocuments[itemIndex] = itemFound;
                await _toastService.Success($"Borrado", $"Archivo borrado temporalmente", autoHide: true);
            }

            await Table.QueryAsync();
            _spinnerService.Hide();
        }

        #endregion

        #endregion






        #region add image to item
        #region imageFile 

        public async Task OnInputFileItemChangeFromItemList(InputFileChangeEventArgs e, SurveyQuestionOptionDto surveyQuestionOptionDto)
        {

            var imageFiles = e.GetMultipleFiles();
            var format = "image/png";
            foreach (var image in imageFiles)
            {
                if (image.Size > MaxFileSize)
                {
                    // Mostrar mensaje de error
                    await _toastService.Error($"Ha ocurrido un error con la imagen {image.Name}", $"El archivo excede el tamaño máximo permitido de 10 Mb.", autoHide: true);

                }
                else
                {
                    var resizeImageFile = await image.RequestImageFileAsync(format, 250, 250);
                    var buffer = new byte[resizeImageFile.Size];
                    await resizeImageFile.OpenReadStream(maxAllowedSize: MaxFileSize).ReadAsync(buffer);

                    var imageDataUrlLink = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
                    surveyQuestionOptionDto.Url = imageDataUrlLink;
                    surveyQuestionOptionDto.NewFileQuestion = image;
                }
            }
            StateHasChanged();
        }


        #region delete
        public async void HandleDeleteImageFromItemList(SurveyQuestionOptionDto surveyQuestionOptionDto)
        {

            surveyQuestionOptionDto.Url = null;
            if (surveyQuestionOptionDto.NewFileQuestion != null)
            {
                surveyQuestionOptionDto.NewFileQuestion = null;
            }
            if (surveyQuestionOptionDto.SurveyQuestionOptionDocuments != null && surveyQuestionOptionDto.SurveyQuestionOptionDocuments.ToDeleted == false)
            {
                surveyQuestionOptionDto.SurveyQuestionOptionDocuments.ToDeleted = true;
            }

            StateHasChanged();
        }

        #endregion
        #endregion

        #endregion


        #region save and send to revision


        public async Task sendToSave(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;


            var model = ModelData;

            if (allowOnlyIntern && allowOnlyExtern)
            {
                model.TypeCreation = TYPE_PROCCESS_SURVEY.ALL_SURVEY;
            }
            else
            {
                if (allowOnlyIntern)
                {
                    model.TypeCreation = TYPE_PROCCESS_SURVEY.INTERNAL_SURVEY;
                }
                else
                {
                    if (allowOnlyExtern)
                    {
                        model.TypeCreation = TYPE_PROCCESS_SURVEY.EXTERNAL_SURVEY;
                    }
                    else
                    {
                        await _toastService.Warning($"Información", $"Debe seleccionar para quien va dirigida la encuesta, para usuarios internos, ciudadanos o inclusive ambos", autoHide: true);
                        return;
                    }
                }
            }



            var isEmptyTitles = ModelData.SurveyQuestions.Where(x => string.IsNullOrEmpty(x.Title.Trim()));

            if (isEmptyTitles.Count() > 0)
            {
                await _toastService.Warning($"Información", $"Los títulos de preguntas son requeridos, inténtalo de nuevo por favor", autoHide: true);
                return;
            }

            _spinnerService.Show();

            //marcar el archivo principal como borrado
            if (model.PrincipalImage != null)
            {
                if (model.NewPrincipalImage != null)
                {
                    model.PrincipalImage.ToDeleted = true;
                }
            }

            //marcar los documentos como borrado
            if (model.SurveyAttachedDocuments != null)
            {
                var findNewFiles = model.SurveyAttachedDocuments.Where(x => x.BrowserFile != null).Select(x => x.BrowserFile);

                if (findNewFiles != null && findNewFiles.Count() > 0)
                {
                    model.AttachedNewFiles = findNewFiles.ToList();


                    var onlyFilesSaved = model.SurveyAttachedDocuments.Where(x => x.BrowserFile == null);
                    model.SurveyAttachedDocuments = onlyFilesSaved.ToList();
                }
            }

            //saber cuales preguntas fueron eliminadas de la parte visual
            var detectChangesToDelete = ModelDataConsult.SurveyQuestions.Where(a => !model.SurveyQuestions.Any(x => x.Id == a.Id)).ToList();
            //var detectChangesToDelete2 =  model.SurveyQuestions.Where(a => !model.SurveyQuestions.Any(x => x.Id == a.Id)).ToList();

            //revisar quien no está de verdad para marcarlo como eliminado
            foreach (var question in detectChangesToDelete)
            {
                var itemExistInConsult = ModelDataConsult.SurveyQuestions.FirstOrDefault(x => question.Id == x.Id);
                //como se que existe lo voy eliminar
                if (itemExistInConsult != null)
                {
                    var itemConvertQuestion = itemExistInConsult.ToJson().FromJson<SurveyQuestionDto>();
                    itemConvertQuestion.ToDeleted = true;
                    model.SurveyQuestions.Add(itemConvertQuestion);
                }
            }
            //recorrer las preguntas
            foreach (var item in model.SurveyQuestions)
            {
                ////recorrer para saber si hay un nuevo archivo para estos
                //foreach (var questionOption in item.SurveyQuestionOptions)
                //{
                //    if (questionOption.SurveyQuestionOptionDocuments != null)
                //    {
                //        if (model.AttachedNewFiles != null)
                //        {
                //           questionOption.ToDeleted = true;
                //        }
                //    }
                //}
                //preguntar si hay un item en la consulta general, con el objetivo de ver quienes son los que hay que borrar

                if (!item.AllowJustification)
                {
                    item.JustificationTitle = "";
                }

                var findItemQuestion = ModelDataConsult.SurveyQuestions.FirstOrDefault(x => x.Id == item.Id);

                if (findItemQuestion != null)
                {

                    var findMissingQuestionOptionsToDelete = findItemQuestion.SurveyQuestionOptions.Where(a => !item.SurveyQuestionOptions.Any(x => x.Id == a.Id)).ToList();
                    foreach (var questionOptionItem in findMissingQuestionOptionsToDelete)
                    {
                        var itemConvertQuestionOption = questionOptionItem.ToJson().FromJson<SurveyQuestionOptionDto>();
                        if (itemConvertQuestionOption.SurveyQuestionOptionDocuments != null)
                        {
                            itemConvertQuestionOption.SurveyQuestionOptionDocuments.ToDeleted = true;
                        }
                        itemConvertQuestionOption.ToDeleted = true;
                        item.SurveyQuestionOptions.Add(itemConvertQuestionOption);
                    }
                }

            }

            //mandar api
            var response = await _surveyService.PutSurvey(model);
            if (response != null && response.response != null && response.response.Success)
            {
                _spinnerService.Hide();
                await _toastService.Success("Información editada", "Encuesta editada con éxito");
                await goToList();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }
            _spinnerService.Hide();
        }



        #endregion


        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("/encuestas");
        }

        public async Task backToFirstStep()
        {
            isReadyPage = false;
            await ActionChild.InvokeAsync(null);
        }
        #endregion

        public string GetIdItem(string type, Guid? id)
        {
            return type + "-" + id;
        }

        public string GetTypeToShow(string item)
        {
            if (item == SURVEYQUESTIONTYPE.SINGLE)
            {
                return "(Selección Única)";
            }
            else if (item == SURVEYQUESTIONTYPE.MULTIPLE)
            {
                return "(Selección Múltiple)";
            }
            else
            {
                return "(Respuesta Breve)";
            }
        }
    }
}
