using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Associations.ControlAssign
{
    public class AssociationControlAssignBase : ComponentBase
    {
        [Inject]
        public IAssociationService _associationService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        [Parameter]
        public AssociationListUsersResponse associationListUsersResponse { get; set; } = new AssociationListUsersResponse();
        [NotNull]
        [Parameter]
        public UserResponse principalUserSelected { get; set; } = new UserResponse();
        [Parameter]
        public Guid associationId { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public async Task HandleFormUnAssingValid()
        {
            _spinnerService.Show();

            HandleAssociationJoinUser handleAssociationJoinUser = new HandleAssociationJoinUser()
            {
                UserId = principalUserSelected.Id,
                AssociationId = associationId,

            };
            var response = await _associationService.PostAssociationUnjoinUser(handleAssociationJoinUser);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información agregada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                //handleAssociationConfig = new HandleAssociationConfig();
                associationListUsersResponse = new AssociationListUsersResponse();
                await ActionChild.InvokeAsync(null);
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                associationListUsersResponse = new AssociationListUsersResponse();
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }


        }


        public async Task HandleFormAssingValid()
        {
            _spinnerService.Show();

            if (principalUserSelected.Id == null)
            {
                await _toastService.Error("Seleccionar usuario", "Selecciona primero un usuario para poder asignar a la asociación", autoHide: true);
                _spinnerService.Hide();
                return;
            }

            HandleAssociationJoinUser handleAssociationJoinUser = new HandleAssociationJoinUser()
            {
                UserId = principalUserSelected.Id,
                AssociationId = associationId,

            };
            var response = await _associationService.PostAssociationJoinUser(handleAssociationJoinUser);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información agregada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                //handleAssociationConfig = new HandleAssociationConfig();
                associationListUsersResponse = new AssociationListUsersResponse();
                await ActionChild.InvokeAsync(null);
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                associationListUsersResponse = new AssociationListUsersResponse();
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }

        }

        public static string OnGetDisplayText(UserResponse foo) => foo.UserName ?? "";
        public Task OnSelectedItemChanged(UserResponse foo)
        {
            principalUserSelected = Utility.Clone(foo);
            StateHasChanged();
            return Task.CompletedTask;
        }

        public async Task CloseModal()
        {
            await ActionChild.InvokeAsync(null);
        }
    }
}
