﻿using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CatalogResponse.Edit
{

    public class CatalogResponseEditBase : ComponentBase
    {
        [Inject]
        public ICatalogAutomaticResponseService _catalogResponseService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Parameter]
        //[SupplyParameterFromForm]
        public ManagementCatalogResponse managementCatalogResponseEdit { get; set; } = new ManagementCatalogResponse();


        [Parameter]
        public List<Catalog> listCatalog { get; set; }
        [Parameter]
        public IEnumerable<SelectedItem> itemsCatalogSelect { get; set; }

        [Parameter]
        public EventCallback ActionChild { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public async Task HandleFormValid()
        {
            _spinnerService.Show();


            var itemListSelected = listCatalog.FirstOrDefault(x => x.Code == managementCatalogResponseEdit.Code);

            managementCatalogResponseEdit.Collection = itemListSelected != null ? itemListSelected.Collection : "";
            var response = await _catalogResponseService.PutCatalogResponse(managementCatalogResponseEdit);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información editada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                managementCatalogResponseEdit = new ManagementCatalogResponse();
                await ActionChild.InvokeAsync(null);
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }

        }

        public async Task CloseModal()
        {
            managementCatalogResponseEdit = new ManagementCatalogResponse();
            await ActionChild.InvokeAsync(null);
        }

    }
}
