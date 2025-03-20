using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Associations.List
{
    public class AssociationListBase : ComponentBase
    {
        [Inject]
        public IAssociationService _associationService { get; set; }

        [Inject]
        public IDistrictService _districtService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [NotNull]
        public Guid associationId { get; set; }


        public AssociationDefinition _getAssociationResponse { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };

        public List<DistrictNeighborhoodsDefinition> listDistricts { get; set; } = new List<DistrictNeighborhoodsDefinition>();

        public IEnumerable<SelectedItem> itemsCatalogSelect;




        [NotNull]
        public Modal? ModalCreate { get; set; }
        [NotNull]
        public HandleAssociationConfig? handleAssociationConfig { get; set; } = new HandleAssociationConfig();

        #region edit

        [NotNull]
        public Modal? ModalEdit { get; set; }

        [NotNull]
        public HandleAssociationConfig? managementAssociationEditConfig { get; set; } = new HandleAssociationConfig();

        public IEnumerable<SelectedItem> itemsCatalogNeighborhoodSelect { get; set; } = new List<SelectedItem>();

        #endregion
        #region assign control user

        [NotNull]
        public Modal? ModalControlAssign { get; set; }
        public AssociationListUsersResponse associationListUsersResponse { get; set; } = new AssociationListUsersResponse();
        [NotNull]
        public UserResponse principalUserSelected { get; set; } = new UserResponse();
        #endregion
        #region control massive load
        [NotNull]
        public Modal? ModalLoadMassive { get; set; }

        public IEnumerable<HandleAssociationConfig> listAssociationsLoadedExcel { get; set; } = Enumerable.Empty<HandleAssociationConfig>();
        #endregion

        [NotNull]
        public Table<AssociationResponse>? Table { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }



        protected override async Task OnInitializedAsync()
        {

            _spinnerService.Show();

            List<SelectedItem> listSelect = new List<SelectedItem>();

            listDistricts = await _districtService.GetDistricts();

            foreach (var item in listDistricts)
            {
                listSelect.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });


            }
            listSelect.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));

            itemsCatalogSelect = listSelect;

            _spinnerService.Hide();
        }


        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<AssociationResponse>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            var catalogInputDto = new AssociationInputDto()
            {
                SkipCount = (options.PageIndex - 1) * options.PageItems,
                MaxResultCount = options.PageItems,
            };

            var filters = options.ToFilter();

            if (filters != null && filters.Filters.Count > 0)
            {
                ApplyFiltersData(filters, catalogInputDto);
            }


            var response = await _associationService.GetListFilters(catalogInputDto);
            if (response != null && response.response.Success)
            {
                _getAssociationResponse = response.definition;
                IEnumerable<AssociationResponse> items = _getAssociationResponse.items;

                var total = _getAssociationResponse.totalCount;
                _spinnerService.Hide();

                return new QueryData<AssociationResponse>()
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
                return new QueryData<AssociationResponse>()
                {
                    Items = null,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
        }

        private void ApplyFiltersData(FilterKeyValueAction filters, AssociationInputDto userFilterDto)
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
                            case "DistrictCode":
                                userFilterDto.DistrictCode = itemDataFilter.FieldValue?.ToString()?.Trim();
                                break;
                            case "Enabled":

                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        #region goTo
        #region create

        public async Task goToCreate()
        {
            _spinnerService.Show();
            ModalCreate.Toggle();
        }

        public Task OnShownCallbackAsync()
        {
            _spinnerService.Hide();
            handleAssociationConfig = new HandleAssociationConfig();


            itemsCatalogNeighborhoodSelect = new List<SelectedItem>();
            return Task.CompletedTask;
        }

        protected async Task ActionComponentFather()
        {
            ModalCreate.Toggle();
            Table.QueryAsync();
        }
        #endregion



        #region edit
        public async Task openEdit(TableColumnContext<AssociationResponse, Guid> item)
        {
            _spinnerService.Show();

            var itemSelected = listDistricts.FirstOrDefault(x => x.Code == item.Row.DistrictCode);
            if (itemSelected != null)
            {
                var list = new List<SelectedItem>();
                var neighborhoodList = itemSelected.NeighborhoodList;

                list.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                foreach (var itemNeighborhood in neighborhoodList)
                {
                    list.Add(new SelectedItem()
                    {
                        Text = itemNeighborhood.DisplayLabel,
                        Value = itemNeighborhood.Code
                    });
                }
                itemsCatalogNeighborhoodSelect = list;
            }
          

            managementAssociationEditConfig = new HandleAssociationConfig()
            {
                Id = item.Row.Id,
                BeforeChangeName = item.Row.Name,
                Description = item.Row.Description,
                DistrictCode = item.Row.DistrictCode,
                NeighbordCode = item.Row.NeighbordCode,
                Name = item.Row.Name,
            };
            ModalEdit.Toggle();
        }

        public Task OnShownCallbackEditAsync()
        {
            _spinnerService.Hide();
            managementAssociationEditConfig = new HandleAssociationConfig();
            return Task.CompletedTask;
        }

        protected async Task ActionEditComponentFather()
        {
            ModalEdit.Toggle();
            Table.QueryAsync();
        }
        #endregion

        #region delete
        public async Task deleteInfo(TableColumnContext<AssociationResponse, Guid> item)
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
                var data = await _associationService.DeleteAssociation(item.Value);
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

        #region control assign users

        public async Task openControlUserAssign(TableColumnContext<AssociationResponse, Guid> item)
        {
            _spinnerService.Show();

            var inputData = new AssociationIdInputDto()
            {
                Id = item.Row.Id,
            };
            var dataService = await _associationService.GetUsersForJoinAsociation(inputData);
            if (dataService != null)
            {
                if (dataService.response != null && dataService.response.Success)
                {
                    associationListUsersResponse = dataService.definition;
                    principalUserSelected = dataService.definition.UserRelated != null? dataService.definition.UserRelated : new UserResponse();
                    associationId = item.Row.Id;

                    ModalControlAssign.Toggle();
                }
                else
                {
                    _spinnerService.Hide();
                    await _toastService.Error("Acción", dataService.response.Message, autoHide: true);
                }
            }
            else
            {
                _spinnerService.Hide();
                await _toastService.Error("Acción", "Ha ocurrido un error, por favor inténtalo de nuevo", autoHide: true);

            }

        }

        public Task OnShownCallbackControlAssignAsync()
        {
            _spinnerService.Hide();
            return Task.CompletedTask;
        }
        protected async Task ActionControlAssignComponentFather()
        {
            ModalControlAssign.Toggle();
            Table.QueryAsync();
        }
        #endregion


        #region massive upload
        public async Task openModalMassiveLoad()
        {
            _spinnerService.Show();
            listAssociationsLoadedExcel = new List<HandleAssociationConfig>();

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
            listAssociationsLoadedExcel = new List<HandleAssociationConfig>();
            Table.QueryAsync();
        }

        #endregion
        #endregion
    }
}
