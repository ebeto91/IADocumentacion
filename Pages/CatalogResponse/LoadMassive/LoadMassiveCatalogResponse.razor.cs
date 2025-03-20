using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CatalogResponse.LoadMassive
{
    public partial class LoadMassiveCatalogResponse : ComponentBase
    {

        [Inject]
        public ICatalogAutomaticResponseService _automaticResponseService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public IExcelService _excelService { get; set; }
        [Parameter]
        public IEnumerable<SelectedItem> itemsCatalogSelect { get; set; }
        [Parameter]
        public IEnumerable<CatalogAutomaticExcel> listCatalogAutomaticResponses { get; set; }

        public Table<CatalogAutomaticExcel>? Table { get; set; }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }



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

        public async Task CloseModal()
        {
            await ActionChild.InvokeAsync(null);
        }

        public async Task GenerateFileExcel()
        {
            _spinnerService.Show();
            try
            {
                var resultToNotify = true;

                List<CatalogAutomaticExcel> handleAutomaticResponseConfigExcels = new List<CatalogAutomaticExcel>();


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

                DataTable secondPageData = new DataTable();


                List<DataExcelShow> dataExcelShows = new List<DataExcelShow>();

                foreach (var item in itemsCatalogSelect)
                {
                    if (!string.IsNullOrEmpty(item.Value))
                    {
                        dataExcelShows.Add(new DataExcelShow()
                        {
                            Name = item.Text,
                            Value = item.Value
                        });
                    }
                }

                secondPageData = dataExcelShows.ToDataTable();
                var attributesSecondPageData = listData.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                foreach (var item in attributesSecondPageData)
                {
                    try
                    {
                        secondPageData.Columns["Name"].ColumnName = item;
                    }
                    catch (Exception ex)
                    {
                    }
                }

                var data = _excelService.GetExcelStreamOpenXML(listData, out resultToNotify, secondPageData: secondPageData, secondPageName: "Información Tipos");


                var arrayData = data.ToArray();
                await _jsRuntime.InvokeAsync<object>("saveAsFile", "Plantilla Respuestas Automaticas.xlsx", Convert.ToBase64String(arrayData));
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
                    listCatalogAutomaticResponses = data.ToListof<CatalogAutomaticExcel>();
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

            var requestData = listCatalogAutomaticResponses.Select(item => new CatalogAutomaticResponseDto
            {
                Id = Guid.NewGuid(),
                Code = item.Code,
                Collection = "STATUS-MANAGEMENT",
                Description = item.Description,
                DisplayLabel = item.DisplayLabel,
                Enabled = true
            }).ToList();
            var response = await _automaticResponseService.PostMassiveAutomaticResponse(requestData);

            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información agregada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);

                await ActionChild.InvokeAsync(null);
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
        public async Task<QueryData<CatalogAutomaticExcel>> OnQueryAsync(QueryPageOptions options)
        {
            _spinnerService.Show();

            IEnumerable<CatalogAutomaticExcel> items = listCatalogAutomaticResponses;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();

            return new QueryData<CatalogAutomaticExcel>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }
        public string GetTypeByCode(TableColumnContext<CatalogAutomaticExcel, string> item)
        {
            var catalog = itemsCatalogSelect.FirstOrDefault(x => x.Value == item.Value);
            if (catalog != null)
            {
                return catalog.Text;
            }
            else
            {
                return "Desconocido";
            }


        }
        #endregion
    }
}