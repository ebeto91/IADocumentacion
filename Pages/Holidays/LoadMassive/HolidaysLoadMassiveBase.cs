using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Data;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Holidays.LoadMassive
{
    public class HolidaysLoadMassiveBase : ComponentBase
    {
        [Inject]
        public IHolidaysService _holidayService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public IExcelService _excelService { get; set; }

        public IEnumerable<HolidayExcel> listHolidays { get; set; } = new List<HolidayExcel>();

        public Table<HolidayExcel>? Table { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }



        [NotNull]
        public Modal? ModalRef { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }


        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };
        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }


        public async Task GenerateFileExcel()
        {
            _spinnerService.Show();
            try
            {
                var resultToNotify = true;

                List<HolidayExcel> handleAutomaticResponseConfigExcels = new List<HolidayExcel>();


                DataTable listData = new DataTable();
                listData = handleAutomaticResponseConfigExcels.ToDataTable();
                var attributes = listData.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                foreach (var item in attributes)
                {
                    try
                    {
                        listData.Columns["Name"].ColumnName = item;
                    }
                    catch (Exception ex)
                    {
                    }
                }




                var data = _excelService.GetExcelStreamOpenXML(listData, out resultToNotify);


                var arrayData = data.ToArray();
                await _jsRuntime.InvokeAsync<object>("saveAsFile", "Plantilla Fechas.xlsx", Convert.ToBase64String(arrayData));
                _spinnerService.Hide();
                await _toastService.Success("¡Descarga completada!", "Completa la plantilla para poder cargar la información correctamente", autoHide: true);
            }
            catch (Exception ex)
            {
                _spinnerService.Hide();
                await _toastService.Error("¡Descarga incorrecta!", "Intenta descargar de nuevo el archivo, por favor", autoHide: true);
            }


        }
        public async Task OnFileSelected(InputFileChangeEventArgs e)
        {
            try
            {
                if (e != null && e.File != null)
                {
                    _spinnerService.Show();
                    var file = e.File;
                    var data = await _excelService.ImportExcelFile(e);
                    listHolidays = data.ToListof<HolidayExcel>();
                    StateHasChanged();
                    await Table.QueryAsync();
                    _spinnerService.Hide();
                }

            }
            catch (Exception ex)
            {

                _spinnerService.Hide();
                await _toastService.Error("¡Error con el archivo!", "Intenta subir de nuevo el archivo, por favor", autoHide: true);
            }

        }

        public async Task UploadDataMassive()
        {
            _spinnerService.Show();



            var today = DateTime.Today;



            var isValidDatePeriod = listHolidays.Where(x => x.Date < today).ToList();


            if (isValidDatePeriod.Count > 0)
            {
                await _toastService.Error("Acción", "La fecha de cierre no puede ser menor a la fecha de hoy, por favor revisar", autoHide: true);
                _spinnerService.Hide();
                return;
            }

            var requestData = listHolidays.Select(item => new HolidayDto
            {
               DateSelected = item.Date,
               Month = item.Date.Month,
               Day = item.Date.Day,
               Year = item.Date.Year,
               Description = item.Description,
            }).ToList();

            var response = await _holidayService.PostMassiveHolidays(requestData);

            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información agregada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);

                await CloseModal();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }
        }

        #region table
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<HolidayExcel>> OnQueryAsync(QueryPageOptions options)
        {
            _spinnerService.Show();

            IEnumerable<HolidayExcel> items = listHolidays;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();

            return new QueryData<HolidayExcel>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }



        #endregion
        public async Task OpenModal()
        {
            listHolidays = new List<HolidayExcel>();
            await ModalRef.Show();

        }

        public async Task CloseModal()
        {
            await ModalRef.Close();
            await ActionChild.InvokeAsync(null);
        }
        public async Task CloseModalSend()
        {
            await ActionChild.InvokeAsync(null);
        }
    }
}
