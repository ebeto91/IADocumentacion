using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Rol.Create
{
    public class RoleCreateBase : ComponentBase
    {

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }
        [Inject]
        public IRoleService _roleService { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "Create:Role";

        [SupplyParameterFromForm]
        public RoleDto ModelFirst { get; set; }
        public List<MenuModuleRolConfigDto> ModelSecond { get; set; }


        public List<SelectedItem> listIdentificationType { get; set; }
        public List<SelectedItem> listRoles { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }

        public Step? _stepper;
        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();

            ModelFirst ??= new();
            ModelSecond ??= new();

            var isValid = await _validationRouteService.HasAccessRoute(validScopes);

            if (!isValid)
            {
                await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                _navigation.NavigateTo("/login");
                _spinnerService.Hide();
                return;
            }


            RoleFilterRequestInput roleFilter = new RoleFilterRequestInput();
            var listRolesData = await _roleService.GetRolesFilterDataForConfig(roleFilter);

            if (listRolesData == null)
            {
                _spinnerService.Hide();
                await _toastService.Error("Ha ocurrido un error", "Ha ocurrido un error, por favor, inténtalo de nuevo", autoHide: true);
                goToList();
            }
            else
            {
                List<SelectedItem> listRolesSelect = new List<SelectedItem>();


                foreach (var item in listRolesData.items)
                {
                    listRolesSelect.Add(new SelectedItem()
                    {
                        Text = item.Name,
                        Value = item.Name,
                    });
                }

                listRoles = listRolesSelect;

                var listMenuScopesModel = await _roleService.GetScopesDataGroup();

                if (listMenuScopesModel != null && listMenuScopesModel.Count > 0)
                {
                    ModelSecond = listMenuScopesModel;
                }
                else
                {
                    _spinnerService.Hide();
                    await _toastService.Error("Ha ocurrido un error", "Ha ocurrido un error, por favor, inténtalo de nuevo", autoHide: true);
                    goToList();
                }
                _spinnerService.Hide();
            }

           




       
            //return base.OnInitializedAsync();
        }

        public async Task HandleValidSubmit()
        {
            _spinnerService.Show();
            await _stepper.Next();
            _spinnerService.Hide();
        }

        public async Task HandleValidSecondSubmit()
        {
            _spinnerService.Show();
            ModelFirst.Enabled = true;
            RoleCategoryScopeSave roleCategoryScopeSave = new RoleCategoryScopeSave()
            {
                role = ModelFirst,
                MenuModuleRolConfigDto = ModelSecond,
            };

            var response = await _roleService.CreateRole(roleCategoryScopeSave);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información agregada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                goToList();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }
        }
        public async Task backToFirstStep()
        {
            _spinnerService.Show();
            _stepper.Prev();
            _spinnerService.Hide();
        }

        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("/catalogo/roles");
        }
        #endregion
    }
}
