using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Departments.ControlAssignUsers.CreateControlAssignUsers
{
    public class CreateControlAssignUsersBase : ComponentBase
    {
        [Inject]
        public IDepartmentService _departmentService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }


        public List<UserResponseAssing> userListToAssing { get; set; }
        [NotNull]
        public Table<UserResponseAssing>? Table { get; set; }
        [NotNull]
        public List<UserResponseAssing>? SelectedItems { get; set; } = new List<UserResponseAssing>();
        public IEnumerable<UserDepartmentDto> listUserDefinitionTable { get; set; }
        public List<Catalog> listCatalog { get; set; }
        public IEnumerable<SelectedItem> items { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "AssingUser:Department";

        [Parameter]
        public required string departmentId { get; set; }
        public string nameDepartment { get; set; }
        public bool isLoadedData { get; set; }




        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            var isValid = await _validationRouteService.HasAccessRoute(validScopes);

            if (!isValid)
            {
                await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                _navigation.NavigateTo("/login");
                _spinnerService.Hide();
                return;
            }

            if (firstRender)
            {
                _spinnerService.Show();
                var responseCatalog = await _catalogService.GetCatalogByCollection("USERDEPARTMENT-POSITION");

                listCatalog = responseCatalog;

                var listCatalogSelect = new List<SelectedItem>();
                foreach (var catalog in listCatalog)
                {
                    if (catalog.Code != USERDEPARTMENT_POSITION.LEADERSHIP)
                    {
                        listCatalogSelect.Add(new SelectedItem()
                        {
                            Text = catalog.DisplayLabel,
                            Value = catalog.Code
                        });
                    }

                }
                listCatalogSelect.Insert(0, (new SelectedItem { Text = "Seleccione puesto", Value = "" }));
                items = listCatalogSelect;



                await LoadDataTable();
                _spinnerService.Hide();
            }
        }

        private async Task LoadDataTable()
        {


            var response = await _departmentService.GetUsersForAssign();
            var responseDepartment = await _departmentService.GetDeparmentById(departmentId);

            if (response != null && response.response.Success)
            {

                userListToAssing = response.definition;
                if (responseDepartment != null && responseDepartment.response.Success)
                {
                    nameDepartment = responseDepartment.definition.Name;
                }
                isLoadedData = true;

                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {

        }


        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<UserResponseAssing>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            IEnumerable<UserResponseAssing> items = userListToAssing;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();
            foreach (var item in items)
            {
                var itemSelected = SelectedItems.FirstOrDefault(x => x.Id == item.Id);
                if (itemSelected != null)
                {
                    item.Selected = true;
                }
            }
            StateHasChanged();
            return new QueryData<UserResponseAssing>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }
        public void CheckboxChanged(ChangeEventArgs e, TableColumnContext<UserResponseAssing, string> userResponseAssing)
        {
            // get the checkbox state
            bool? x = e.Value as bool?;
            if (x != null)
            {
                if (x.Value)
                {
                    SelectedItems.Add(userResponseAssing.Row);
                    userResponseAssing.Row.Selected = true;
                }
                else
                {
                    SelectedItems.Remove(userResponseAssing.Row);
                    userResponseAssing.Row.Selected = false;
                }
                StateHasChanged();
            }
        }



        #region go back
        public async Task goToBack()
        {
            //_navigation.NavigateTo($"/catalogo/departamentos");
            _navigation.NavigateTo($"/catalogo/departamentos/asignacion/{departmentId}");
        }

        public async Task assignUsersToAdd()
        {
            _spinnerService.Show();
            var dontHavePositionYet = SelectedItems.Where(x => string.IsNullOrEmpty(x.Position)).ToList();
            if (SelectedItems.Count > 0)
            {
                if (dontHavePositionYet.Count == 0)
                {
                    List<AssingUserDepartmentDto> listAssingUsers = new List<AssingUserDepartmentDto>();
                    foreach (var userDepartmentDtoForEdit in SelectedItems)
                    {

                        var userIdParse = Guid.NewGuid();
                        var parse = Guid.TryParse(userDepartmentDtoForEdit.Id, out userIdParse);

                        if (parse)
                        {
                            AssingUserDepartmentDto assingUserDepartmentDto = new AssingUserDepartmentDto()
                            {
                                Description = "",
                                Enabled = userDepartmentDtoForEdit.Enabled,
                                Position = userDepartmentDtoForEdit.Position,
                                UserId = userIdParse
                            };
                            listAssingUsers.Add(assingUserDepartmentDto);
                        }
                        else
                        {
                            await _toastService.Error("Ha ocurrido un error", "Ha ocurrido un error, inténtalo de nuevo por favor", autoHide: true);
                            break;
                        }

                    }
                    AssingUserDepartmentInputDto assingUserDepartmentInputDto = new AssingUserDepartmentInputDto()
                    {
                        DepartmentId = departmentId.ToString(),
                        ListAssingUsers = listAssingUsers
                    };
                    var response = await _departmentService.PostAssingUsersDepartment(assingUserDepartmentInputDto);
                    if (response != null && response.response != null && response.response.Success)
                    {
                        _spinnerService.Hide();
                        var message = response != null && response.response != null ? response.response.Message : "Información agregada con éxito";
                        await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                       await goToBack();
                    }
                    else
                    {
                        _spinnerService.Hide();
                        var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                        await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                    }
                }
                else
                {
                    var message = "Un colaborador seleccionado no posee rol asignado: <br>";
                    foreach (var item in dontHavePositionYet) { message += "-" + item.UserName + "<br>"; }
                    _spinnerService.Hide();
                    await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                }
            }
            else
            {
                _spinnerService.Hide();
                await _toastService.Error("Ha ocurrido un error", "Selecciona un colaborador para poder agregar", autoHide: true);
            }

        }
        #endregion
    }
}
