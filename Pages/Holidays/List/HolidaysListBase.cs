using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Holidays.Create;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Holidays.Edit;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Holidays.LoadMassive;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Holidays.List
{
    public class HolidaysListBase : ComponentBase
    {

        [Inject]
        public IHolidaysService _holidaysService { get; set; }


        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }



        public HolidaysListResponseDefinition _holidaysListResponseDefinition { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };


        [NotNull]
        public HolidaysCreate? HolidaysCreateRef { get; set; }
        [NotNull]
        public HolidaysEdit? HolidaysEditRef { get; set; }

        [NotNull]
        public HolidaysLoadMassive? HolidaysListMassiveRef { get; set; }




        [NotNull]
        public Table<HolidayDto>? Table { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }



        protected override async Task OnInitializedAsync()
        {


        }


        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<HolidayDto>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            var catalogInputDto = new HolidayInputDto()
            {
                SkipCount = (options.PageIndex - 1) * options.PageItems,
                MaxResultCount = options.PageItems,
            };

            var filters = options.ToFilter();

            if (filters != null && filters.Filters.Count > 0)
            {
                ApplyFiltersData(filters, catalogInputDto);
            }


            var response = await _holidaysService.GetHolidaysFilters(catalogInputDto);
            if (response != null && response.response.Success)
            {
                _holidaysListResponseDefinition = response.definition;
                IEnumerable<HolidayDto> items = _holidaysListResponseDefinition.items;

                var total = _holidaysListResponseDefinition.totalCount;
                _spinnerService.Hide();

                return new QueryData<HolidayDto>()
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
                return new QueryData<HolidayDto>()
                {
                    Items = null,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
        }

        private void ApplyFiltersData(FilterKeyValueAction filters, HolidayInputDto userFilterDto)
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
                            case "DateSelected":
                                userFilterDto.DateSelected = DateTime.Parse(itemDataFilter.FieldValue?.ToString());
                                break;

                            case "Description":
                                userFilterDto.Description = itemDataFilter.FieldValue?.ToString();
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
            await HolidaysListMassiveRef.OpenModal();
            _spinnerService.Hide();

        }
        #endregion


        #region goTo
        #region create

        public async Task goToCreate()
        {
            await HolidaysCreateRef.OpenModal();
        }

        protected async Task ActionComponentFather()
        {
            await Table.QueryAsync();
        }
        #endregion



        #region edit
        public async Task openEdit(TableColumnContext<HolidayDto, Guid?> item)
        {


            var holidayManagement = item.Row.ToJson().FromJson<HolidayManagementDto>();
            await HolidaysEditRef.OpenModal(holidayManagement);
        }


        #endregion

        #region delete
        public async Task deleteInfo(TableColumnContext<HolidayDto, Guid?> item)
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
                var data = await _holidaysService.DeleteHoliday(new HolidayDeleteDto()
                {
                    Id = item.Row.Id.Value
                });


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
