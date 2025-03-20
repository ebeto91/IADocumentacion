using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Departments.Create
{
    public class DepartmentCreateBase : ComponentBase
    {
        [Inject]
        public IDepartmentService _departmentService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Parameter]
        public ManagementDepartmentDto managementDepartmentDto { get; set; } = new ManagementDepartmentDto();

        [Parameter]
        public IEnumerable<UserResponse> listUserToAssign { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }

        [NotNull]
        [Parameter]
        public UserResponse principalUserSelected { get; set; } = new UserResponse();

 

        protected override async Task OnInitializedAsync()
        {
        
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {

        }

        public async Task HandleFormValid()
        {
            _spinnerService.Show();


            managementDepartmentDto.CodeDepartment = managementDepartmentDto.Name.Replace(" ", "").ToUpper();
            managementDepartmentDto.IsEditable = true;

            //var itemListSelected = listCatalog.FirstOrDefault(x => x.Code == managementDepartmentDto.Code);

            //managementDepartmentDto.Collection = itemListSelected != null ? itemListSelected.Collection : "";
            managementDepartmentDto.Name = managementDepartmentDto.Name.Trim();
            var response = await _departmentService.PostDepartment(managementDepartmentDto);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información agregada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                managementDepartmentDto = new ManagementDepartmentDto();
                await ActionChild.InvokeAsync(null);
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }

        }

        public static string OnGetDisplayText(UserResponse foo) => foo.Identification ?? "";
        public Task OnSelectedItemChanged(UserResponse foo)
        {
            principalUserSelected = Utility.Clone(foo);
            StateHasChanged();
            managementDepartmentDto.IdUserLeadershipDepartment = principalUserSelected.Id;

            StateHasChanged();
            return Task.CompletedTask;
        }


        public async Task CloseModal()
        {
            managementDepartmentDto = new ManagementDepartmentDto();
            await ActionChild.InvokeAsync(null);
        }



   
    }
}
