using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ListManagement.ViewDetailManagement
{
    public class ViewDetailManagementBase : ComponentBase
    {
        [Parameter]
        public string managementId { get; set; } 

        public FullManagementDto ModelFirst { get; set; } = new FullManagementDto();
        public UserProfileResponse UserCreator { get; set; } = new UserProfileResponse();
        public FindManagementByPrincipalNumberDto FormObject { get; set; } = new FindManagementByPrincipalNumberDto();
        public List<Catalog> listCatalogData = new List<Catalog>();

        [Inject]
        public ICatalogService _catalogService { get; set; }


        [Inject]
        public IManagementService _managementService { get; set; }
        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }



        public List<string> listImagesSelected = new List<string>();


        public bool IsSecondPageReadyToLoad = false;
        protected async override Task OnInitializedAsync()
        {

            _spinnerService.Show();
            ModelFirst ??= new();




            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT", "POSITION-WORK-TASK"]
            };


            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;



            var response = await _managementService.GetManagementById(managementId);

            if (response != null && response.response.Success)
            {


                IsSecondPageReadyToLoad = true;
                ModelFirst = response.definition;

                if (ModelFirst.AttachedDocuments != null && ModelFirst.AttachedDocuments.Count > 0)
                {
                    foreach (var document in ModelFirst.AttachedDocuments)
                    {
                        listImagesSelected.Add(document.FilePath);
                    }
                }

                if (ModelFirst.CreatedUserId.HasValue)
                {
                    var responseUser = await _userService.GetUserProfile(ModelFirst.CreatedUserId.Value);
                    if (responseUser != null)
                    {
                        UserCreator = responseUser;
                    }
                }
                _spinnerService.Hide();

                StateHasChanged();
            }
            else
            {
                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }


            _spinnerService.Hide();
        }



        #region goTo
        public async Task goBack()
        {
            _navigation.NavigateTo("/miperfil");
        }

        #endregion



    }
}
