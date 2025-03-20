using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Roles;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Users.Create;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.FindManagementByPrincipalNumber
{
    public class FindManagementByPrincipalNumberBase : ComponentBase
    {

        public FullManagementDto ModelFirst { get; set; } = new FullManagementDto();
        public UserProfileResponse UserCreator { get; set; } = new UserProfileResponse();
        public FindManagementByPrincipalNumberDto FormObject { get; set; } = new FindManagementByPrincipalNumberDto();
        public List<Catalog> listCatalogData = new List<Catalog>();

        [Inject]
        public ICatalogService _catalogService { get; set; }


        [Inject]
        public IManagementService _managementService { get; set; }

        [Inject]
        public IDepartmentService _departmentService { get; set; }
        [Inject]
        public ICatalogAutomaticResponseService _catalogAutomaticResponseService { get; set; }
        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }

   

        public List<string> listImagesSelected = new List<string>();



        public Step? _stepper;

        public bool IsSecondPageReadyToLoad = false;
        protected async override Task OnInitializedAsync()
        {

            _spinnerService.Show();
            ModelFirst ??= new();




            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION", "POSITION-WORK-TASK"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;



            ////
            var listStatus = listCatalogData.Where(x => x.Collection == "STATUS-MANAGEMENT");

            //foreach (var item in listStatus)
            //{
            //    listSelectStatus.Add(new SelectedItem()
            //    {
            //        Text = item.DisplayLabel,
            //        Value = item.Code,
            //    });
            //}
            //listSelectStatus.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////


            ////
            var listPriority = listCatalogData.Where(x => x.Collection == "PRIORITY").OrderBy(x => x.Ref2);

            //foreach (var item in listPriority)
            //{
            //    listSelectPriority.Add(new SelectedItem()
            //    {
            //        Text = item.DisplayLabel,
            //        Value = item.Code,
            //    });
            //}
            //listSelectPriority.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////

            ////
            var listSubtype = listCatalogData.Where(x => x.Collection == "SUBPRINCIPALTYPE-APPLICATION");
            //foreach (var item in listSubtype)
            //{
            //    listSelectType.Add(new SelectedItem()
            //    {
            //        Text = item.DisplayLabel,
            //        Value = item.Code,
            //    });
            //}
            //listSelectType.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////
            ///

            ////
            var listPosition = listCatalogData.Where(x => x.Collection == "POSITION-WORK-TASK");
            //foreach (var item in listPosition)
            //{
            //    listWorkTaskPosition.Add(new SelectedItem()
            //    {
            //        Text = item.DisplayLabel,
            //        Value = item.Code,
            //    });
            //}
            //listWorkTaskPosition.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////
            _spinnerService.Hide();
        }

        public async Task HandleValidSubmit()
        {

            if (!string.IsNullOrEmpty(FormObject.PrincipalNumberToFind.Trim()))
            {
                _spinnerService.Show();
                var response = await _managementService.GetManagementByPrincipalNumber(FormObject.PrincipalNumberToFind);

                if (response != null && response.response.Success)
                {
                    await _stepper.Next();
                    _spinnerService.Hide();
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


                    StateHasChanged();
                }
                else
                {
                    _spinnerService.Hide();
                    var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                    await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                }
            }
            else
            {
                await _toastService.Warning("Información incorrecta", "Coloca correctamente el número de seguimiento", autoHide: true);
            }

        }


        #region goTo
        public async Task goToHome()
        {
            _navigation.NavigateTo("/");
        }


        public async Task backToFirstStep()
        {
            _stepper.Prev();
        }
        #endregion



    }
}
