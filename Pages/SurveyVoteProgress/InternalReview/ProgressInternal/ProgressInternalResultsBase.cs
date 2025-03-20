using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using System.Data;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVoteProgress.InternalReview.ProgressInternal
{
    public class ProgressInternalResultsBase : ComponentBase
    {
        //public CheckboxState chkInternal {  get; set; }= CheckboxState.UnChecked;
        //public CheckboxState chkExternal { get; set; } = CheckboxState.UnChecked;

        public bool allowOnlyIntern { get; set; }
        public bool allowOnlyExtern { get; set; }

        public CheckboxState chkOpen { get; set; } = CheckboxState.UnChecked;

        public CheckboxState chkShowResults { get; set; } = CheckboxState.UnChecked;
        public int characterCount { get; set; } = 0;
        public SurveyResponse ModelData = new SurveyResponse();

        [NotNull]
        public ImagePreviewer? ImagePreviewer { get; set; }
        public List<string> PreviewList { get; set; } = ["https://upload.wikimedia.org/wikipedia/commons/d/d3/Soccerball.svg"];

        public string idSendToTask { get; set; } = "";

        [NotNull]
        public Modal? SmallFullScreenModal { get; set; }

        public List<QuestionComment> Comments = new List<QuestionComment>();

        [Parameter]
        public string SurveyId { get; set; }

        [Parameter]
        public int BackReturn { get; set; } = 0;

        HangleManagementTaskBySurvey hangleManagementTaskBySurvey = new HangleManagementTaskBySurvey();

        public int _circleValue = 0;

        public bool isUserInternal { get; set; } = true;

        public void Add(int interval)
        {
            _circleValue += interval;
            _circleValue = Math.Min(100, Math.Max(0, _circleValue));
        }

        public DotNetObjectReference<ProgressInternalResultsBase> self;

        [Inject]
        public ISurveyService _SurveyService { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;

        public ShowResultsResponse Modeldefinition = new ShowResultsResponse();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public IWorkTaskService _workTaskService { get; set; }
        [Inject]
        public IExcelService _excelService { get; set; }

        public string value { get; set; } = "";

        //[CascadingParameter]
        //public Task<AuthenticationState>? authenticationState { get; set; }
        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        protected async override Task OnInitializedAsync()
        {

            var userTokenData = await _customAuthService.GetClaims();
            var userId = "";
            if (userTokenData != null)
            {

                var ROLE = userTokenData.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

                if (ROLE != null && ROLE.Value == ROLEAUDITORIA.Usuario)
                {
                    isUserInternal = false;
                }

            }


            self = DotNetObjectReference.Create(this);
            _spinnerService.Show();
            module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/SurveyVoteProgress/SurveyVoteProgress.razor.js");
            ShowResultsInputDto input = new ShowResultsInputDto
            {
                SurveyId = Guid.Parse(SurveyId),
            };
            var data = await _SurveyService.ShowResultsSurvey(input);
            var dataSurvey = await _SurveyService.GetSurveyForVoteById(SurveyId);
            if (dataSurvey != null && data.response.Success)
            {
                ModelData = dataSurvey.definition;
                characterCount = !string.IsNullOrEmpty(ModelData.Description) ? ModelData.Description.Length : 0;

                switch (ModelData.TypeCreation)
                {

                    //case "INTERNAL-SURVEY":
                    //    chkInternal = CheckboxState.Checked;
                    //    break;

                    //case "EXTERNAL-SURVEY":
                    //    chkExternal = CheckboxState.Checked;
                    //    break;
                    case "ALL-SURVEY":
                        chkOpen = CheckboxState.Checked;
                        break;
                }

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


                if (ModelData.ShowResultsAlways)
                {
                    chkShowResults = CheckboxState.Checked;
                }
            }

            if (data != null && data.response.Success)
            {

                Modeldefinition = data.definition;

                // await module.InvokeVoidAsync("IsReady");
            }

            _spinnerService.Hide();

            //  await module.InvokeVoidAsync("IsReady");

        }

        public async void CheckSendToTask(QuestionComment item)
        {

            if (!string.IsNullOrEmpty(item.Title) && !string.IsNullOrEmpty(item.Descripcion))
            {
                hangleManagementTaskBySurvey.SurveyQuestionOptionUserId = (Guid)item.SurveyQuestionOptionUserId;
                hangleManagementTaskBySurvey.Title = item.Title;
                hangleManagementTaskBySurvey.Description = item.Descripcion.Trim();
                hangleManagementTaskBySurvey.Status = STATUSMANAGEMENT.NEW;
                hangleManagementTaskBySurvey.PrincipalTypeApplication = PRINCIPALTYPE.MANAGEMENT;
                var response = await _workTaskService.AssignCreateTaskForVoteSurvey(hangleManagementTaskBySurvey);
                if (response != null && response.response.Success)
                {
                    await _toastService.Success("Enviar a tarea", response.response.Message, autoHide: true);
                    item.Title = "";
                    item.Descripcion = "";
                    item.Hide = true;
                    StateHasChanged();

                }
            }
            else
            {
                await _toastService.Warning("Campos Requeridos", "Los campos título y descripción son requeridos", autoHide: true);
            }
        }

        [JSInvokable]
        public async Task SendToTask(bool send, string text, string title)
        {
            if (send)
            {
                hangleManagementTaskBySurvey.SurveyQuestionOptionUserId = Guid.Parse(idSendToTask);
                hangleManagementTaskBySurvey.Title = title;
                hangleManagementTaskBySurvey.Description = text;
                hangleManagementTaskBySurvey.Status = STATUSMANAGEMENT.NEW;
                hangleManagementTaskBySurvey.PrincipalTypeApplication = PRINCIPALTYPE.MANAGEMENT;
                var response = await _workTaskService.AssignCreateTaskForVoteSurvey(hangleManagementTaskBySurvey);
                if (response != null && response.response.Success)
                {
                    await _toastService.Warning("Enviar a tarea", response.response.Message, autoHide: true);
                    await module.InvokeVoidAsync("checkSendTask", idSendToTask);
                }
            }
            else
            {
                await _toastService.Warning("Campos Requeridos", "Los campos título y descripción son requeridos", autoHide: true);
            }

        }

        public void OpenModal(List<QuestionComment> questionComments)
        {
            if (Comments.Count > 0)
            {
                Comments = new List<QuestionComment>();
                StateHasChanged();
            }

            Comments = questionComments;


            SmallFullScreenModal.Show();
        }


        public async void test()
        {
            //  await module.InvokeVoidAsync("GetCircles");
        }

        public Task View(string url)
        {
            if (PreviewList.Count == 0)
            {
                PreviewList.Add(url);
            }
            else
            {
                PreviewList.Clear();

                PreviewList.Add(url);

            }
            StateHasChanged();
            ImagePreviewer.Show();

            return Task.CompletedTask;
        }

        public void Back()
        {

            // _navigation.NavigateTo("/encuestas");
            _navigation.NavigateTo("javascript:history.back()");


        }





        public async Task GenerateFileExcel()
        {
            _spinnerService.Show();
            try
            {
                var resultToNotify = true;

                // Obtener los resultados de la encuesta
                ShowResultsInputDto input = new ShowResultsInputDto
                {
                    SurveyId = Guid.Parse(SurveyId),
                };

                if (Modeldefinition != null)
                {
                    // Convertir los resultados de la encuesta a DataTable
                    DataTable surveyResults = new DataTable("Resultados de la Encuesta");
                    surveyResults.Columns.Add("Pregunta");
                    surveyResults.Columns.Add("Respuesta");
                    surveyResults.Columns.Add("Porcentaje");
                    surveyResults.Columns.Add("Votos");



                    // Convertir los comentarios a DataTable
                    DataTable commentsTable = new DataTable("Comentarios");
                    commentsTable.Columns.Add("Título");
                    commentsTable.Columns.Add("Descripción");

                    foreach (var question in Modeldefinition.SurveyQuestions)
                    {
                        foreach (var answer in question.SurveyQuestionOptions)
                        {
                            DataRow row = surveyResults.NewRow();
                            row["Pregunta"] = question.Title;
                            row["Respuesta"] = answer.Description;
                            row["Porcentaje"] = answer.TotalPercentage;
                            row["Votos"] = answer.Total;
                            surveyResults.Rows.Add(row);

                            if (answer.QuestionOptionsComments != null && answer.QuestionOptionsComments.Count > 0)
                            {
                                foreach (var comment in answer.QuestionOptionsComments)
                                {
                                    DataRow row2 = commentsTable.NewRow();
                                    row2["Título"] = answer.Description;
                                    row2["Descripción"] = comment.CommentValue;
                                    commentsTable.Rows.Add(row2);
                                }
                            }

                        }
                    }

                    foreach (var comment in Comments)
                    {
                        DataRow row = commentsTable.NewRow();
                        row["Título"] = comment.Title;
                        row["Descripción"] = comment.Descripcion;
                        commentsTable.Rows.Add(row);
                    }

                    // Generar el archivo Excel con los resultados de la encuesta y los comentarios
                    var excelStream = _excelService.GetExcelStreamOpenXML(surveyResults, out resultToNotify, secondPageData: commentsTable);

                    var arrayData = excelStream.ToArray();
                    await _jsRuntime.InvokeAsync<object>("saveAsFile", "ResultadosEncuestaOVotacion.xlsx", Convert.ToBase64String(arrayData));
                    _spinnerService.Hide();
                    await _toastService.Success("¡Descarga completada!", "Los resultados de la encuesta se han descargado correctamente", autoHide: true);
                }
                else
                {
                    _spinnerService.Hide();
                    await _toastService.Error("¡Error al obtener resultados!", "No se pudieron obtener los resultados de la encuesta", autoHide: true);
                }
            }
            catch (Exception ex)
            {
                _spinnerService.Hide();
                await _toastService.Error("¡Descarga incorrecta!", "Intenta descargar de nuevo el archivo, por favor", autoHide: true);
            }
        }

    }
}
