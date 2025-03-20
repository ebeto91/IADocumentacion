using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskComments;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalComplaint.ControlInternalComplaintFollowing.Tabs.Details.Comments
{
    public class EditComplaintDetailsCommentsBase : ComponentBase
    {
        public UserProfileResponse UserProfile { get; set; }
        public HandleWorkTaskCommentDto ModelComment { get; set; } = new HandleWorkTaskCommentDto();    
        public ListWorkTaskCommentResponse listWorkTaskCommentResponse { get; set; }

        public WorkTaskCommentsGroupResponse listComments { get; set; }
        public string WorkTaskId { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IWorkTaskService _workTaskService { get; set; }
        [Inject]
        public IWorkTaskCommentService _workTaskCommentService { get; set; }



        [Inject]
        public ToastService _toastService { get; set; }

        [NotNull]
        public WorkTaskInputEditTrackableDto workTaskResponseDetailConsult { get; set; }

        public async Task UpdateData(WorkTaskInputEditTrackableDto _workTaskResponseDetailConsult, UserProfileResponse userProfile, string workTaskId)
        {
            workTaskResponseDetailConsult = _workTaskResponseDetailConsult;
            UserProfile = userProfile;
            WorkTaskId = workTaskId;
            await updateListComments(workTaskId);
            StateHasChanged();
        }
        public async Task updateListComments(string workTaskId)
        {
            var response = await _workTaskCommentService.GetWorkTaskCommentsById(workTaskId);
            if (response != null && response.Response != null && response.Response.Success)
            {
                UpdateDataListAssign(response);
            }
          
        }

        private void UpdateDataListAssign(ListWorkTaskCommentResponse response)
        {
            listWorkTaskCommentResponse = response;
            listComments = response.Definition.FirstOrDefault(x => x.Type == "COMMENT");
            StateHasChanged();
        }

        public async Task saveComment(EditContext formContext)
        {

            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;

            _spinnerService.Show();
            ModelComment.Type = TypesComments.COMMENT;
            ModelComment.UserId = UserProfile.Id;
            ModelComment.WorkTaskId = WorkTaskId;


            var response = await _workTaskCommentService.CreateCommentForWorkTask(ModelComment);
            if (response != null && response.Response != null && response.Response.Success)
            {
                UpdateDataListAssign(response);
                ModelComment = new HandleWorkTaskCommentDto();
                await _toastService.Success("¡Comentario agregado!", response.Response.Message, autoHide: true);
            }
            else
            {
                var message = response != null && response.Response != null ? response.Response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("¡Ha ocurrido un error!", message, autoHide: true);

            }


            _spinnerService.Hide();
        }

        public string getUserProfile(string image)
        {
            var imageData = ImageProfile.ImageDefault;
            if (!string.IsNullOrEmpty(image))
            {
                imageData = image;
                return imageData;
            }
            else
            {
                return imageData;
            }
        }


        public async Task updateButton()
        {
            _spinnerService.Show();
            await updateListComments(WorkTaskId);
            _spinnerService.Hide();
        }
    }
}
