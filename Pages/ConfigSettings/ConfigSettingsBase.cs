using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.ConfigSetting;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ConfigSettings
{
    public class ConfigSettingsBase : ComponentBase
    {
        [Inject]
        public IConfigSettingService _configSettingService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        //public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };
        public List<ConfigSettingModel> deparmentListDefinition { get; set; }
        [NotNull]
        public Table<ConfigSettingModel>? Table { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }




        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<ConfigSettingModel>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            //var departmentInputDto = new DepartmentInputDto()
            //{
            //    SkipCount = (options.PageIndex - 1) * options.PageItems,
            //    MaxResultCount = options.PageItems,
            //};


            var response = await _configSettingService.GetSettings();
            if (response != null && response.response.Success)
            {
                deparmentListDefinition = response.definition;
                IEnumerable<ConfigSettingModel> items = deparmentListDefinition;

                var total = deparmentListDefinition.Count;
                _spinnerService.Hide();

                return new QueryData<ConfigSettingModel>()
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
                return new QueryData<ConfigSettingModel>()
                {
                    Items = null,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }

        }

        public async Task goToSave()
        {
            _spinnerService.Show();
            var data = Table.Rows.ToList();
            var response = await _configSettingService.PostSettings(data);

            if (response != null && response.response != null && response.response.Success)
            {
                _spinnerService.Hide();
                await _toastService.Success("Acción", response.response.Message, autoHide: true);
                await Table.QueryAsync();
            }
            else
            {
                _spinnerService.Hide();
                if (response != null && response.response != null)
                {
                    Table.QueryAsync();
                    await _toastService.Error("Acción", response.response.Message, autoHide: true);
                }
                else
                {
                    Table.QueryAsync();
                    await _toastService.Error("Acción", "Ha ocurrido un error, por favor inténtalo de nuevo", autoHide: true);
                }
            }


        }

    }
}
