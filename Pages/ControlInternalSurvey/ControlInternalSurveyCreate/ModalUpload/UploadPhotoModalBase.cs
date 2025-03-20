using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Survey;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalSurvey.ControlInternalSurveyCreate.ModalUpload
{
    public class UploadPhotoModalBase : ComponentBase
    {

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        private const long MaxFileSize = 10240000L; // 500 KB
        [NotNull]
        public Modal? ModalRef { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }

        public SurveyQuestionOptionDto _SurveyQuestionOptionDto { get; set; } = new SurveyQuestionOptionDto();

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public async Task HandleFormValid()
        {
            _spinnerService.Show();


    

        }
        public async Task CloseModal()
        {
            await ModalRef.Close();
            await ActionChild.InvokeAsync(null);
        }

        public async Task CloseModalSend()
        {
            await ModalRef.Close();
            await ActionChild.InvokeAsync(null);
        }
        public async Task OpenModal(SurveyQuestionOptionDto surveyQuestionOptionDto)
        {
            _SurveyQuestionOptionDto = surveyQuestionOptionDto;
            StateHasChanged();
            await ModalRef.Show();
        }



        #region imageFile 

        public async Task OnInputFileItemChange(InputFileChangeEventArgs e)
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

                    //EvidenciaDto evidenciaDto = new EvidenciaDto()
                    //{
                    //    imageUrl = imageDataUrlLink,
                    //    Identificador = Guid.NewGuid().ToString(),
                    //    Delete = false,
                    //    FileImage = image

                    //};
                    _SurveyQuestionOptionDto.Url = imageDataUrlLink;
                    _SurveyQuestionOptionDto.NewFileQuestion = image;
                }
            }

        }


        #region delete
        public async void HandleDeleteImage()
        {

            _SurveyQuestionOptionDto.Url = null;
            _SurveyQuestionOptionDto.NewFileQuestion = null;
            StateHasChanged();
        }

        #endregion
        #endregion

    }
}
