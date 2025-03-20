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
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Departments.List
{
    public class DepartmentListBase : ComponentBase
    {
        [Inject]
        public IDepartmentService _departmentService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        public DepartmentListResponseDefinition deparmentListDefinition { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };

        #region create
        [NotNull]
        public Modal? ModalCreate { get; set; }
        [NotNull]
        public ManagementDepartmentDto managementDepartmentDto { get; set; } = new ManagementDepartmentDto();

        #endregion

        [NotNull]
        public UserResponse principalUserSelected { get; set; } = new UserResponse();
        [Parameter]
        public IEnumerable<UserResponse> listUserToAssign { get; set; }

        public IEnumerable<DepartmenstExcelDto> listDepartamentLoadedExcel { get; set; } = Enumerable.Empty<DepartmenstExcelDto>();
        [NotNull]
        public Modal? ModalLoadMassive { get; set; }

        #region edit
        [NotNull]
        public Modal? ModalEdit { get; set; }
        [NotNull]
        public ManagementDepartmentDto? managementDepartmentDtoForEdit { get; set; } = new ManagementDepartmentDto();
        #endregion


        [NotNull]
        public Table<DepartmentResponse>? Table { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Departments/List/DepartmentList.razor.js");
        
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var response = await _departmentService.GetUsersForAssign();

            if (response != null && response.response != null && response.response.Success)
            {
                listUserToAssign = response.definition;
            }
        }


        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<DepartmentResponse>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            var departmentInputDto = new DepartmentInputDto()
            {
                SkipCount = (options.PageIndex - 1) * options.PageItems,
                MaxResultCount = options.PageItems,
            };

            var filters = options.ToFilter();

            if (filters != null && filters.Filters.Count > 0)
            {
                ApplyFiltersData(filters, departmentInputDto);
            }


            var response = await _departmentService.GetListFilters(departmentInputDto);
            if (response != null && response.response.Success)
            {
                deparmentListDefinition = response.definition;
                IEnumerable<DepartmentResponse> items = deparmentListDefinition.items;

                var total = deparmentListDefinition.totalCount;
                _spinnerService.Hide();

                return new QueryData<DepartmentResponse>()
                {
                    Items = items,
                    TotalCount = total,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
            else
            {
                //TODO VALIDAR 
                _spinnerService.Hide();
                return new QueryData<DepartmentResponse>()
                {
                    Items = null,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
        }

        private void ApplyFiltersData(FilterKeyValueAction filters, DepartmentInputDto userFilterDto)
        {
            foreach (var item in filters.Filters)
            {
                var itemFilter = item.Filters;
                foreach (var itemDataFilter in itemFilter)
                {
                    if (itemDataFilter != null && !string.IsNullOrEmpty(itemDataFilter.FieldKey) && itemDataFilter.FieldValue != null)
                    {
                        var key = itemDataFilter.FieldKey;
                        switch (key)
                        {
                            case "Name":
                                userFilterDto.Name = itemDataFilter.FieldValue?.ToString()?.Trim();
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }



        #region massive upload
        public async Task openModalMassiveLoad()
        {
            _spinnerService.Show();
            listDepartamentLoadedExcel = new List<DepartmenstExcelDto>();

            ModalLoadMassive.Toggle();
            _spinnerService.Hide();

        }

        public Task OnShownCallbackModalLoadAsync()
        {
            _spinnerService.Hide();
            return Task.CompletedTask;
        }
        protected async Task ActionModalAssignComponentFather()
        {
            ModalLoadMassive.Toggle();
            listDepartamentLoadedExcel = new List<DepartmenstExcelDto>();
            Table.QueryAsync();
        }

        #endregion


        #region goTo
        #region create

        public async Task goToCreate()
        {
            _spinnerService.Show();
            var response = await _departmentService.GetUsersForAssign();

            if (response != null && response.response != null && response.response.Success)
            {
                _spinnerService.Hide();
                listUserToAssign = response.definition;
                await module.InvokeVoidAsync("preventEnterKey", "addFormDepartment");
                ModalCreate.Toggle();
            }
            else
            {
                _spinnerService.Hide();
                await _toastService.Error("Ha ocurrido un error", "Ha ocurrido un error, inténtalo de nuevo por favor", autoHide: true);
            }

          
        }

        public Task OnShownCallbackAsync()
        {
            _spinnerService.Hide();
            managementDepartmentDto = new ManagementDepartmentDto();
            principalUserSelected = new UserResponse();
            return Task.CompletedTask;
        }

        protected async Task ActionComponentFather()
        {
            ModalCreate.Toggle();
            Table.QueryAsync();
        }
        #endregion



        #region edit
        public bool IsEditable { get; set; }
        public async Task openEdit(TableColumnContext<DepartmentResponse, Guid> item)
        {
            _spinnerService.Show();

            var departmentData = Utility.Clone(item.Row); 

            var inputDto = new DepartmentIdInputDto()
            {
                Id = departmentData.IdLeader
            };

            var response = await _departmentService.GetUsersForAssignForEdit(inputDto);

            if (response != null && response.response != null && response.response.Success)
            {
                

                _spinnerService.Hide();
                listUserToAssign = response.definition;

                var findUser = listUserToAssign.FirstOrDefault(x => x.Id == departmentData.IdLeader);

                if (findUser != null)
                {
                    principalUserSelected = findUser;
                }

                var itemParse = departmentData.ToJson().FromJson<ManagementDepartmentDto>();

                managementDepartmentDtoForEdit = itemParse;
                managementDepartmentDtoForEdit.IdUserLeadershipDepartment = departmentData.IdLeader;
                managementDepartmentDtoForEdit.BeforeChangeName = itemParse.Name;
                managementDepartmentDtoForEdit.IdUserLeadershipDepartmentBefore = departmentData.IdLeader;
                managementDepartmentDtoForEdit.IsEditable = itemParse.IsEditable;

                IsEditable = itemParse.IsEditable != null? itemParse.IsEditable.Value : true;


                ModalEdit.Toggle();
                await module.InvokeVoidAsync("preventEnterKey", "editFormDepartmentData");
            }
            else
            {
                _spinnerService.Hide();
                await _toastService.Error("Ha ocurrido un error", "Ha ocurrido un error, inténtalo de nuevo por favor", autoHide: true);
            }

            //_spinnerService.Show();
            //var response = await _departmentService.GetUsersForAssign();

            //if (response != null && response.response != null && response.response.Success)
            //{
            //    _spinnerService.Hide();
            //    listUserToAssign = response.definition;
            //    await module.InvokeVoidAsync("preventEnterKey", "editFormDepartmentData");
            //    ModalEdit.Toggle();
            //}
            //else
            //{
            //    _spinnerService.Hide();
            //    await _toastService.Error("Ha ocurrido un error", "Ha ocurrido un error, inténtalo de nuevo por favor", autoHide: true);
            //}


        }

        public Task OnShownCallbackEditAsync()
        {
            _spinnerService.Hide();
            managementDepartmentDtoForEdit = new ManagementDepartmentDto();
            principalUserSelected = new UserResponse();
            return Task.CompletedTask;
        }

        protected async Task ActionEditComponentFather()
        {
            ModalEdit.Toggle();
            Table.QueryAsync();
        }
        #endregion

        #region delete
        public async Task deleteInfo(TableColumnContext<DepartmentResponse, Guid> item)
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
                var data = await _departmentService.DeleteDepartment(item.Value);
                if (data != null && data.response != null && data.response.Success)
                {
                    _spinnerService.Hide();
                    Table.QueryAsync();
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
        #region control user assing
        public async Task openControlUserAssign(TableColumnContext<DepartmentResponse, Guid> item)
        {
            _spinnerService.Show();

            var inputData = new AssociationIdInputDto()
            {
                Id = item.Row.Id,
            };

            _navigation.NavigateTo($"/catalogo/departamentos/asignacion/{item.Row.Id}");
            //var dataService = await _departamentService.GetUsersForJoinAsociation(inputData);
            //if (dataService != null)
            //{
            //    if (dataService.response != null && dataService.response.Success)
            //    {
            //        associationListUsersResponse = dataService.definition;
            //        principalUserSelected = dataService.definition.UserRelated != null ? dataService.definition.UserRelated : new UserResponse();
            //        associationId = item.Row.Id;

            //        ModalControlAssign.Toggle();
            //    }
            //    else
            //    {
            //        _spinnerService.Hide();
            //        await _toastService.Error("Acción", dataService.response.Message, autoHide: true);
            //    }
            //}
            //else
            //{
            //    _spinnerService.Hide();
            //    await _toastService.Error("Acción", "Ha ocurrido un error, por favor inténtalo de nuevo", autoHide: true);

            //}

        }

        #endregion
        #endregion
    }
}
