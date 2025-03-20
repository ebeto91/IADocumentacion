using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Departments.ControlAssignUsers.EditControlAssignUsers
{
    public class EditControlAssignUsersBase : ComponentBase
    {
        [Inject]
        public IDepartmentService _departmentService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Parameter]
        public UserDepartmentDto userDepartmentDtoForEdit { get; set; } = new UserDepartmentDto();
        [Parameter]
        public IEnumerable<SelectedItem> items { get; set; }
        [Parameter]
        public Guid departmentId { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }


        protected override async Task OnInitializedAsync()
        {

        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {

        }

        public async Task HandleFormValid()
        {
            _spinnerService.Show();


            AssingUserDepartmentDto assingUserDepartmentDto = new AssingUserDepartmentDto()
            {
                Description = userDepartmentDtoForEdit.Description,
                Enabled = userDepartmentDtoForEdit.Enabled,
                Position = userDepartmentDtoForEdit.Position,
                UserId = userDepartmentDtoForEdit.UserId
            };

            List<AssingUserDepartmentDto> listAssingUsers = new List<AssingUserDepartmentDto>();
            listAssingUsers.Add(assingUserDepartmentDto);

            AssingUserDepartmentInputDto assingUserDepartmentInputDto = new AssingUserDepartmentInputDto()
            {
                DepartmentId = departmentId.ToString(),
                ListAssingUsers = listAssingUsers
            };

            var response = await _departmentService.PostAssingUsersDepartment(assingUserDepartmentInputDto);
            if (response != null && response.response != null && response.response.Success)
            {
                var message = response != null && response.response != null ? response.response.Message : "Información editada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                userDepartmentDtoForEdit = new UserDepartmentDto();
                await ActionChild.InvokeAsync(null);
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }



        }
       


        public async Task CloseModal()
        {
            userDepartmentDtoForEdit = new UserDepartmentDto();
            await ActionChild.InvokeAsync(null);
        }
    }
}
