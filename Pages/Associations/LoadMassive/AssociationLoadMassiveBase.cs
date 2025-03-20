using BlazorSpinner;
using BootstrapBlazor.Components;
using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Reflection;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Associations.LoadMassive
{
    public class AssociationLoadMassiveBase : ComponentBase
    {
        [Inject]
        public IAssociationService _associationService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public IExcelService _excelService { get; set; }
        [Parameter]

        public List<DistrictNeighborhoodsDefinition> listDistrict { get; set; }
        [Parameter]
        public IEnumerable<SelectedItem> itemsCatalogSelect { get; set; }
        [Parameter]
        public IEnumerable<HandleAssociationConfig> listAssociationsLoadedExcel { get; set; }

        public Table<HandleAssociationConfig>? Table { get; set; }

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

                List<HandleAssociationConfigExcel> handleAssociationConfigExcels = new List<HandleAssociationConfigExcel>();


                DataTable listData = new DataTable();
                listData = handleAssociationConfigExcels.ToDataTable();
                var attributes = listData.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                foreach (var item in attributes)
                {
                    try
                    {
                        //MemberInfo property = typeof(HandleAssociationConfigExcel).GetProperty(item);

                        //var attribute = property.GetCustomAttributes(typeof(DisplayAttribute), true)
                        //                .Cast<DisplayAttribute>().Single();

                        //string displayName = _stringLocalizer.GetString(attribute.Name).Value;
                        listData.Columns["Name"].ColumnName = item;
                    }
                    catch (Exception ex)
                    {
                    }
                }
                #region second page 
            
                DataTable secondPageData = new DataTable();


                List<DataExcelShow> dataExcelShows = new List<DataExcelShow>();

                foreach (var item in listDistrict)
                {
                    dataExcelShows.Add(new DataExcelShow()
                    {
                        Name = item.DisplayLabel,
                        Value = item.Code
                    });
                }

                secondPageData = dataExcelShows.ToDataTable();
                var attributesSecondPageData = secondPageData.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

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
                #endregion



                #region third page 
                DataTable thirdPageData = new DataTable();


                List<DataExcelDepencyShow> dataExcelShowsThirdPage = new List<DataExcelDepencyShow>();

                var listNeighborhoods = listDistrict.SelectMany(x => x.NeighborhoodList);

                foreach (var item in listNeighborhoods)
                {
                    var district = listDistrict.FirstOrDefault(x => x.Ref1 == item.Ref2);
                    dataExcelShowsThirdPage.Add(new DataExcelDepencyShow()
                    {
                        Name = item.DisplayLabel,
                        RelatedValue = district != null ? district.DisplayLabel : "Sin Distrito",
                        Value = item.Code
                    });
                }

                thirdPageData = dataExcelShowsThirdPage.ToDataTable();
                var attributesThirdPageData = thirdPageData.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                foreach (var item in attributesThirdPageData)
                {
                    try
                    {
                        thirdPageData.Columns["Name"].ColumnName = item;
                    }
                    catch (Exception ex)
                    {
                    }
                }


                #endregion
                var data = _excelService.GetExcelStreamOpenXML(listData, out resultToNotify, 
                    secondPageData: secondPageData, secondPageName: "Información distritos",
                    thirdPageData: thirdPageData, thirdPageName : "Información barrios");


                var arrayData = data.ToArray();
                await _jsRuntime.InvokeAsync<object>("saveAsFile", "Plantilla Asociaciones.xlsx", Convert.ToBase64String(arrayData));
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
                    listAssociationsLoadedExcel = data.ToListof<HandleAssociationConfig>();
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
            var response = await _associationService.PostMassiveAssociation(listAssociationsLoadedExcel.ToList());

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
        public async Task<QueryData<HandleAssociationConfig>> OnQueryAsync(QueryPageOptions options)
        {
            _spinnerService.Show();

            IEnumerable<HandleAssociationConfig> items = listAssociationsLoadedExcel;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();

            return new QueryData<HandleAssociationConfig>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }
        public string GetDistrictByCode(TableColumnContext<HandleAssociationConfig, string> item)
        {
            var catalog = listDistrict.FirstOrDefault(x => x.Code == item.Value);
            if (catalog != null)
            {
                return catalog.DisplayLabel;
            }
            else
            {
                return "Desconocido";
            }


        }


        public string GetDistrictByNighborhood(TableColumnContext<HandleAssociationConfig, string> item)
        {
            var listNeighborhoods = listDistrict.SelectMany(x => x.NeighborhoodList);

            var catalog = listNeighborhoods.FirstOrDefault(x => x.Code == item.Value);

            if (catalog != null)
            {
                return catalog.DisplayLabel;
            }
            else
            {
                return "Sin Barrio";
            }


        }
        #endregion


    }
}
