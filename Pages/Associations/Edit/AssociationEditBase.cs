using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Associations.Edit
{
    public class AssociationEditBase : ComponentBase
    {
        [Inject]
        public IAssociationService _associationService { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        [Parameter]
        public HandleAssociationConfig handleAssociationConfigEdit { get; set; } = new HandleAssociationConfig();
        [Parameter]
        public List<DistrictNeighborhoodsDefinition> listDistrict { get; set; }
        [Parameter]
        public IEnumerable<SelectedItem> itemsCatalogSelect { get; set; }
        [Parameter]
        public IEnumerable<SelectedItem> itemsCatalogNeighborhoodSelect { get; set; }
        [Parameter]
        public EventCallback ActionChild { get; set; }

        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }

        public async Task HandleFormValid()
        {
            _spinnerService.Show();

            handleAssociationConfigEdit.Name = handleAssociationConfigEdit.Name.Trim();
            var response = await _associationService.PutAssociation(handleAssociationConfigEdit);
            if (response != null && response.response.Success)
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Información editada con éxito";
                await _toastService.Success("¡Proceso correcto!", message, autoHide: true);
                handleAssociationConfigEdit = new HandleAssociationConfig();
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
            handleAssociationConfigEdit = new HandleAssociationConfig();
            await ActionChild.InvokeAsync(null);
        }

        public async Task OnItemChanged(SelectedItem item)
        {
            var itemSelected = listDistrict.FirstOrDefault(x => x.Code == item.Value);
            var list = new List<SelectedItem>();
            if (itemSelected != null)
            {
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
                StateHasChanged();
            }
            else
            {
                itemsCatalogNeighborhoodSelect = list;
                StateHasChanged();
            }
        }
    }
}
