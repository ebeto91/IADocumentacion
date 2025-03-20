using BlazorSpinner;
using BootstrapBlazor.Components;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using NPOI.HPSF;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.SurveyVote;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CitizenManagement;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.SurveyVoteProgress
{
    public class SurveyVoteProgressBase:ComponentBase
    {
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

        public DotNetObjectReference<SurveyVoteProgressBase> self;

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

        public string value { get; set; } = "";
        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        public IExcelService _excelService { get; set; }


        protected async override Task OnInitializedAsync()
        {

            var userTokenData = await _customAuthService.GetClaims();
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
            module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
"./Pages/SurveyVoteProgress/SurveyVoteProgress.razor.js");
            ShowResultsInputDto input = new ShowResultsInputDto
            {
                SurveyId = Guid.Parse(SurveyId),
            };
            var data = await _SurveyService.ShowResultsSurvey(input);

            if(data != null && data.response.Success)
            {

                Modeldefinition = data.definition;
               // await module.InvokeVoidAsync("IsReady");
            }

            _spinnerService.Hide();
           
          //  await module.InvokeVoidAsync("IsReady");

        }

        public async void CheckSendToTask(QuestionComment item)
        {

            if (!string.IsNullOrEmpty(item.Title) && !string.IsNullOrEmpty(item.Descripcion) )
            {
                hangleManagementTaskBySurvey.SurveyQuestionOptionUserId = (Guid)item.SurveyQuestionOptionUserId;
                hangleManagementTaskBySurvey.Title = item.Title;
                hangleManagementTaskBySurvey.Description = item.Descripcion;
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
        public async Task SendToTask(bool send,string text,string title)
        {
            if(send)
            {
                hangleManagementTaskBySurvey.SurveyQuestionOptionUserId = Guid.Parse(idSendToTask);
                hangleManagementTaskBySurvey.Title = title;
                hangleManagementTaskBySurvey.Description = text;
                hangleManagementTaskBySurvey.Status = STATUSMANAGEMENT.NEW;
                hangleManagementTaskBySurvey.PrincipalTypeApplication = PRINCIPALTYPE.MANAGEMENT;
                var response = await _workTaskService.AssignCreateTaskForVoteSurvey(hangleManagementTaskBySurvey);
               if(response != null && response.response.Success)
                {
                    await _toastService.Warning("Enviar a tarea",response.response.Message, autoHide: true);
                    await module.InvokeVoidAsync("checkSendTask", idSendToTask);
                }
            }
            else
            {
                await _toastService.Warning("Campos Requeridos","Los campos título y descripción son requeridos",autoHide:true);
            }

        }

        public void OpenModal(List<QuestionComment> questionComments)
        {
            if (Comments.Count > 0)
            {
                Comments=new List<QuestionComment>();
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
            
                if (isUserInternal)
                {
                    _navigation.NavigateTo("/participacionMunicipio");
                }
                else
                {
                    _navigation.NavigateTo("/participacionCiudadana");
                }
           
        }

    }
}
