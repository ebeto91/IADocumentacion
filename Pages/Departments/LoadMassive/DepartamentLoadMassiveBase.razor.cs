using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Departments.LoadMassive
{
    public partial class DepartamentLoadMassiveBase : ComponentBase
    {
        [Inject]
        public IDepartmentService _departamentService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public IExcelService _excelService { get; set; }
        [Parameter]

        public IEnumerable<UserResponse> _listUserToAssign { get; set; }
        [Parameter]
        public IEnumerable<SelectedItem> itemsCatalogSelect { get; set; }
        [Parameter]
        public IEnumerable<DepartmenstExcelDto> listDepartamentLoadedExcel { get; set; }

        public Table<DepartmenstExcelDto>? Table { get; set; }

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

                List<DepartmenstExcelDto> handleDepartamentConfigExcels = new List<DepartmenstExcelDto>();


                DataTable listData = new DataTable();
                listData = handleDepartamentConfigExcels.ToDataTable();
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

                foreach (var item in _listUserToAssign)
                {
                    dataExcelShows.Add(new DataExcelShow()
                    {
                        Name = item.EmailAddress,
                        Value = item.Id.ToString()
                    });
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

                var data = _excelService.GetExcelStreamOpenXML(listData, out resultToNotify, secondPageData: secondPageData, secondPageName: "Información Usuarios");


                var arrayData = data.ToArray();
                await _jsRuntime.InvokeAsync<object>("saveAsFile", "Plantilla Departamentos.xlsx", Convert.ToBase64String(arrayData));
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
                    listDepartamentLoadedExcel = data.ToListof<DepartmenstExcelDto>();
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
            var response = await _departamentService.PostMassiveDepartament(listDepartamentLoadedExcel.ToList());

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
        public async Task<QueryData<DepartmenstExcelDto>> OnQueryAsync(QueryPageOptions options)
        {
            _spinnerService.Show();

            IEnumerable<DepartmenstExcelDto> items = listDepartamentLoadedExcel;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();

            return new QueryData<DepartmenstExcelDto>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }
        public string GetUserByEmail(TableColumnContext<DepartmenstExcelDto, string> item)
        {
            var catalog = _listUserToAssign.FirstOrDefault(x => x.Id == item.Value);
            if (catalog != null)
            {
                return catalog.EmailAddress;
            }
            else
            {
                return "Desconocido";
            }


        }
        #endregion


    }
}

