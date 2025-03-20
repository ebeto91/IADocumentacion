using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CatalogResponse.List
{
    public class CatalogResponseListBase : ComponentBase
    {

        [Inject]
        public ICatalogAutomaticResponseService _catalogResponseService { get; set; }

        [Inject]
        public ICatalogService _catalogService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

 

        public CatalogAutomaticResponseDefinition _catalogAutomaticResponse { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };

        public List<Catalog> listCatalog { get; set; } = new List<Catalog>();
        public IEnumerable<SelectedItem> itemsCatalogSelect;

        [NotNull]
        public Modal? ModalCreate { get; set; }
        [NotNull]
        public ManagementCatalogResponse managementCatalogResponse { get; set; } = new ManagementCatalogResponse();

        [NotNull]
        public Modal? ModalEdit { get; set; }
        [NotNull]
        public ManagementCatalogResponse? managementCatalogResponseForEdit { get; set; } = new ManagementCatalogResponse();


        [NotNull]
        public Table<CatalogAutomaticResponseDto>? Table { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }


        public Modal? ModalLoadMassive { get; set; }

        public List<CatalogAutomaticExcel> listAutomaticResponseLoadedExcel { get; set; } = new List<CatalogAutomaticExcel>();

        protected override async Task OnInitializedAsync()
        {

            _spinnerService.Show();

            List<SelectedItem> listSelect = new List<SelectedItem>();
            var itemInputCatalog = new CatalogInputCollectionDto()
            {
                Collections = ["STATUS-MANAGEMENT"],
                Ref1 = "1"
            };

            listCatalog = await _catalogService.GetCatalogByFilters(itemInputCatalog);

            foreach (var item in listCatalog)
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
        public async Task<QueryData<CatalogAutomaticResponseDto>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            var catalogInputDto = new CatalogResponseInputDto()
            {
                SkipCount = (options.PageIndex - 1) * options.PageItems,
                MaxResultCount = options.PageItems,
            };

            var filters = options.ToFilter();

            if (filters != null && filters.Filters.Count > 0)
            {
                ApplyFiltersData(filters, catalogInputDto);
            }


            var response = await _catalogResponseService.GetListFilters(catalogInputDto);
            if (response != null && response.response.Success)
            {
                _catalogAutomaticResponse = response.definition;
                IEnumerable<CatalogAutomaticResponseDto> items = _catalogAutomaticResponse.items;

                var total = _catalogAutomaticResponse.totalCount;
                _spinnerService.Hide();

                return new QueryData<CatalogAutomaticResponseDto>()
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
                return new QueryData<CatalogAutomaticResponseDto>()
                {
                    Items = null,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
        }

        private void ApplyFiltersData(FilterKeyValueAction filters, CatalogResponseInputDto userFilterDto)
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
                            case "DisplayLabel":
                                userFilterDto.DisplayLabel = itemDataFilter.FieldValue?.ToString()?.Trim();
                                break;
                            case "Description":
                                userFilterDto.Description = itemDataFilter.FieldValue?.ToString()?.Trim();
                                break;
                            case "Enabled":
                                bool flag;
                                if (Boolean.TryParse(itemDataFilter.FieldValue.ToString(), out flag))
                                {
                                    userFilterDto.Enabled = flag;
                                }
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
            listAutomaticResponseLoadedExcel = new List<CatalogAutomaticExcel>();

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
            listAutomaticResponseLoadedExcel = new List<CatalogAutomaticExcel>();
            Table.QueryAsync();
        }

        #endregion


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
            managementCatalogResponse = new ManagementCatalogResponse();
            return Task.CompletedTask;
        }

        protected async Task ActionComponentFather()
        {
            ModalCreate.Toggle();
            Table.QueryAsync();
        }
        #endregion



        #region edit
        public async Task openEdit(TableColumnContext<CatalogAutomaticResponseDto, Guid> item)
        {
            _spinnerService.Show();
   
            managementCatalogResponseForEdit = new ManagementCatalogResponse()
            {
                Code = item.Row.Code,
                Collection = item.Row.Collection,
                Description = item.Row.Description, 
                DisplayLabel = item.Row.DisplayLabel,   
                Enabled = item.Row.Enabled,
                Id = item.Row.Id,
            };
            ModalEdit.Toggle();
        }

        public Task OnShownCallbackEditAsync()
        {
            _spinnerService.Hide();
            managementCatalogResponseForEdit = new ManagementCatalogResponse();
            return Task.CompletedTask;
        }

        protected async Task ActionEditComponentFather()
        {
            ModalEdit.Toggle();
            Table.QueryAsync();
        }
        #endregion

        #region delete
        public async Task deleteInfo(TableColumnContext<CatalogAutomaticResponseDto, Guid> item)
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
                var data = await _catalogResponseService.DeleteCatalogResponse(item.Value);
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

        #endregion
    }
}
