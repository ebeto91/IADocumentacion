using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Rol.List
{
    public class RoleListBase : ComponentBase
    {
        [Inject]
        public IRoleService _roleService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }


        [NotNull]
        public Table<RoleDto>? Table { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }


        public RolListResponseDefinition _rolListResponseDefinition { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };


        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }



        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<RoleDto>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            var userFilterDto = new RoleFilterRequest()
            {
                SkipCount = (options.PageIndex - 1) * options.PageItems,
                MaxResultCount = options.PageItems,
            };

            var filters = options.ToFilter();

            if (filters != null && filters.Filters.Count > 0)
            {
                ApplyFiltersData(filters, userFilterDto);
            }


            var response = await _roleService.GetRolesFilterData(userFilterDto);
            if (response != null && response.response.Success)
            {
                _rolListResponseDefinition = response.definition;
                IEnumerable<RoleDto> items = _rolListResponseDefinition.items;

                var total = _rolListResponseDefinition.totalCount;
                _spinnerService.Hide();

                return new QueryData<RoleDto>()
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
                return new QueryData<RoleDto>()
                {
                    Items = null,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }




        }

        private void ApplyFiltersData(FilterKeyValueAction filters, RoleFilterRequest roleFilterRequest)
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
                                roleFilterRequest.Name = itemDataFilter.FieldValue?.ToString()?.Trim();
                                break;

                            //case "Identification":
                            //    roleFilterRequest.Identification = itemDataFilter.FieldValue?.ToString()?.Trim();
                            //    break;
                            default:
                                break;
                        }
                    }

                    //var key2 = itemDataFilter.FieldValue;
                }
            }
        }

        #region goTo
        public async Task goToCreate()
        {
            _navigation.NavigateTo("/catalogo/roles/crear");
        }
        #endregion

        #region edit
        public async Task openEdit(TableColumnContext<RoleDto, int> item)
        {
            _spinnerService.Show();
            _navigation.NavigateTo($"/catalogo/roles/editar/{item.Value}");

        }
        #endregion
        #region active / inactive
        public async Task inactiveRole(TableColumnContext<RoleDto, int> item)
        {

            SweetAlertResult result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Acción",
                Text = "¿Está seguro de que desea inactivar el rol?",
                Icon = SweetAlertIcon.Warning,
                AllowEscapeKey = false,
                ShowCancelButton = true,
                CancelButtonColor = "#D9001B",
                ConfirmButtonText = "Inactivar",
                ConfirmButtonColor = "#0755A3",
                CancelButtonText = "Cancelar"
            });

            if (result != null && result.IsConfirmed)
            {
                _spinnerService.Show();

                RoleFilter roleFilter = new RoleFilter()
                {
                    IdRole = item.Row.Id,
                };

                var data = await _roleService.InactiveRole(roleFilter);
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
        public async Task activeRole(TableColumnContext<RoleDto, int> item)
        {
            SweetAlertResult result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Acción",
                Text = "¿Está seguro de que desea activar el rol?",
                Icon = SweetAlertIcon.Warning,
                AllowEscapeKey = false,
                ShowCancelButton = true,
                CancelButtonColor = "#D9001B",
                ConfirmButtonText = "Activar",
                ConfirmButtonColor = "#0755A3",
                CancelButtonText = "Cancelar"
            });

            if (result != null && result.IsConfirmed)
            {
                _spinnerService.Show();

                RoleFilter roleFilter = new RoleFilter()
                {
                    IdRole = item.Row.Id,
                };

                var data = await _roleService.ActiveRole(roleFilter);
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

    }
}
