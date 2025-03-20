using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Departments.ControlAssignUsers.ListControlAssignUsers
{
    public class DepartmentControlUserListBase : ComponentBase
    {
        [Inject]
        public IDepartmentService _departmentService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }


        public DepartmentResponseUsersDefinition departmentResponseUsersDefinition { get; set; }
        public IEnumerable<UserDepartmentDto> listUserDefinitionTable { get; set; }
        public List<Catalog> listCatalog { get; set; }

        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };

        [Parameter]
        public required string departmentId { get; set; }
        public Guid departmentIdModal { get; set; }
        public bool isLoadedData { get; set; }

        #region edit
        public UserDepartmentDto userDepartmentDtoForEdit { get; set; } = new UserDepartmentDto();
        [NotNull]
        public Modal? ModalEdit { get; set; }

        public IEnumerable<SelectedItem> items { get; set; }
        #endregion

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "AssingUser:Department";

        [NotNull]
        public Table<UserDepartmentDto>? Table { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        protected override async Task OnInitializedAsync()
        {
        
        }
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

            DepartmentIdInputDto departmentIdInputDto = new DepartmentIdInputDto()
            {
                Id = departmentId
            };

            var response = await _departmentService.GetDeparmentWithUsers(departmentIdInputDto);

            if (response != null && response.response.Success)
            {

                departmentResponseUsersDefinition = response.definition;

                foreach (var userDepartmentDto in departmentResponseUsersDefinition.UserDepartments)
                {
                    var itemPosition = listCatalog.FirstOrDefault(x => x.Code == userDepartmentDto.Position);
                    if (itemPosition != null)
                    {
                        userDepartmentDto.PositionDisplay = itemPosition.DisplayLabel;
                    }
                }
                isLoadedData = true;

                StateHasChanged();
            }
        }




        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<UserDepartmentDto>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            IEnumerable<UserDepartmentDto> items = departmentResponseUsersDefinition.UserDepartments;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();

            return new QueryData<UserDepartmentDto>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }


        #region goTo
        #region create

        public async Task goToCreate()
        {
            _navigation.NavigateTo($"/catalogo/departamentos/asignacion/asignar/{departmentId}");
        }


        #endregion



        #region edit
        public async Task openEdit(TableColumnContext<UserDepartmentDto, Guid> item)
        {
            _spinnerService.Show();
            var userAssigned = Utility.Clone(item.Row);
            userDepartmentDtoForEdit = userAssigned;
            departmentIdModal = Guid.Parse(departmentId);
            StateHasChanged();
            _spinnerService.Hide();
            ModalEdit.Toggle();

        }

        protected async Task ActionEditComponentFather()
        {
            ModalEdit.Toggle();
            await LoadDataTable();
            await Table.QueryAsync();
        }
        public async Task OnShownCallbackEditAsync()
        {
            _spinnerService.Hide();
            //userDepartmentDtoForEdit = new UserDepartmentDto();
            //await LoadDataTable();
            //await Table.QueryAsync();
            //return Task.CompletedTask;
        }
        #endregion

        #region delete
        public async Task deleteInfo(TableColumnContext<UserDepartmentDto, Guid> item)
        {

            // Do something with the form values
            SweetAlertResult result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Acción",
                Text = "¿Está seguro de que desea eliminar la información?",
                Icon = SweetAlertIcon.Warning,
                AllowEscapeKey = false,
                ShowCancelButton = true,
                CancelButtonColor = "#D9001B",
                ConfirmButtonText = "Eliminar",
                ConfirmButtonColor = "#0755A3",
                CancelButtonText = "Cancelar"
            });

            if (result != null && result.IsConfirmed)
            {
                _spinnerService.Show();


                AssingUserDepartmentDto assingUserDepartmentDto = new AssingUserDepartmentDto()
                {
                    Description = item.Row.Description,
                    Enabled = false,
                    Position = item.Row.Position,
                    UserId = item.Row.UserId
                };

                List<AssingUserDepartmentDto> listAssingUsers = new List<AssingUserDepartmentDto>();
                listAssingUsers.Add(assingUserDepartmentDto);

                AssingUserDepartmentInputDto assingUserDepartmentInputDto = new AssingUserDepartmentInputDto()
                {
                    DepartmentId = departmentId,
                    ListAssingUsers = listAssingUsers
                };

                var data = await _departmentService.PostAssingUsersDepartment(assingUserDepartmentInputDto);
                if (data != null && data.response != null && data.response.Success)
                {

                    await LoadDataTable();
                    await Table.QueryAsync();
                    await _toastService.Success("Acción", data.response.Message, autoHide: true);
                }
                else
                {
                    _spinnerService.Hide();
                    if (data != null && data.response != null)
                    {
                        Table.QueryAsync();
                        await _toastService.Error("Acción", data.response.Message, autoHide: true);
                    }
                    else
                    {
                        Table.QueryAsync();
                        await _toastService.Error("Acción", "Ha ocurrido un error, por favor inténtalo de nuevo", autoHide: true);
                    }
                }

            }

        }

        #endregion
        #region go back
        public async Task goToBack()
        {
            _navigation.NavigateTo($"/catalogo/departamentos");
        }
        #endregion
        #endregion
    }
}
