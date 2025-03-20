using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalComplaint.ControlInternalComplaintFollowing.Tabs.Historial;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.List;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile
{
    public class MyUserProfileBase : ComponentBase
    {
        public string userId { get; set; }
        public string associationId { get; set; }
        public static string? GetClassString(TabItem tabItem) => CssBuilder.Default("tabs-item").AddClass("active", tabItem.IsActive).Build();

        [NotNull]
        public ListInstitutionalMemory listInstitutionalMemory { get; set; }

        [NotNull]
        public Tab? TabSetTemplate { get; set; }

        public bool isAllow = false;
        public bool isNextView = false;

        public bool isLoadingMemories = false;

        [NotNull]
        public Modal? Modal { get; set; }

        [Inject]
        public IUserService _userService { get; set; }

        [Inject]
        public IManagementService _managementService { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        Blazored.LocalStorage.ILocalStorageService localstorage { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public NavigationManager _navigation { get; set; }
        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        public string ProfilePicture { get; set; } = "https://www.shutterstock.com/image-vector/vector-flat-illustration-grayscale-avatar-600nw-2264922221.jpg";
        public bool CompartirUbicacion { get; set; }


        public UserProfileResponse Model { get; set; }

        #region management 

        public ModelProfileManagementDefinition ListManagement { get; set; } = new ModelProfileManagementDefinition();
        public int totalAcountPages { get; set; }
        #endregion


        #region comlaints 

        public ModelProfileManagementDefinition ListComplaints { get; set; } = new ModelProfileManagementDefinition();
        public int totalAcountPagesComplaints { get; set; }
        #endregion

        #region worktask


        [Inject]
        public IWorkTaskService _worktaskService { get; set; }

        [Parameter]
        public GetWorkTaskForUsersAssignedDefinition? ListWorktask { get; set; } = new GetWorkTaskForUsersAssignedDefinition();
        public int totalAcountPagesWorkTask { get; set; }

        #endregion

        protected async override Task OnInitializedAsync()
        {




            // Obtener el JWT desde localStorage
            var userTokenData = await _customAuthService.GetClaims();

            _spinnerService.Show();


            var userIdItem = userTokenData.Claims.FirstOrDefault(x => x.Type == "UserId");
            var associationItem = userTokenData.Claims.FirstOrDefault(x => x.Type == "AsociationId");

            if (userIdItem != null)
            {
                userId = userIdItem.Value;
            }

            if (associationItem != null)
            {
                associationId = associationItem.Value;
            }

            var response = await _userService.GetUserProfile(Guid.Parse(userId));
            if (response != null && response.Name != null)
            {
                Model = Utility.Clone(response);

                StateHasChanged();
            }

            if (response.RoleName == ROLEAUDITORIA.Usuario)
            {
                ManagementProfileInputFilterDto managementProfileInputFilterDto = new ManagementProfileInputFilterDto()
                {
                    AssociationRelatedMemoryId = associationId,
                    CreatedUserId = userId,
                    PrincipalTypeApplication = "MANAGEMENT",
                    TypeCreation = "EXTERNAL"
                };
                var responseListManagement = await _managementService.GetAllManagementsFiltered(managementProfileInputFilterDto);
                if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

                {
                    // carga la data
                    ListManagement = responseListManagement.definition;
                    var celling = Math.Ceiling((decimal)responseListManagement.definition.totalCount / 10);
                    totalAcountPages = (int)celling;
                }
                else
                {
                    //error
                    var message = response != null && responseListManagement.response != null ?
                        responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                    await _toastService.Error("Error", message, autoHide: true);
                }

                await LoadComplaints();

            }
            else
            {
                #region management

                //todo cargar las tareas asignadas a las personas
                ManagementProfileInputFilterDto managementProfileInputFilterDto = new ManagementProfileInputFilterDto()
                {
                    AssociationRelatedMemoryId = associationId,
                    CreatedUserId = userId,
                    PrincipalTypeApplication = "MANAGEMENT",
                    TypeCreation = "EXTERNAL"
                };
                var responseListManagement = await _managementService.GetAllManagementsFiltered(managementProfileInputFilterDto);
                if (responseListManagement != null && responseListManagement.response != null && responseListManagement.response.Success)

                {
                    // carga la data
                    ListManagement = responseListManagement.definition;
                    var celling = Math.Ceiling((decimal)responseListManagement.definition.totalCount / 10);
                    totalAcountPages = (int)celling;
                }
                else
                {
                    //error
                    var message = response != null && responseListManagement.response != null ?
                        responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                    await _toastService.Error("Error", message, autoHide: true);
                }

                #endregion

                #region workTask

                //todo cargar las tareas asignadas a las personas
                WorkTaskInputDto workTaskInputDto = new WorkTaskInputDto()
                {
                    UserAssignedId = userId,
                    IsVisible = true,
                };

                var responseListWorkTask = await _worktaskService.GetWorkTaskForUsersAssignedFilter(workTaskInputDto);
                if (responseListWorkTask != null && responseListWorkTask.response != null && responseListWorkTask.response.Success)

                {
                    // carga la data
                    ListWorktask = responseListWorkTask.definition;
                    var celling = Math.Ceiling((decimal)responseListWorkTask.definition.totalCount / 10);
                    totalAcountPagesWorkTask = (int)celling;
                }
                else
                {
                    //error
                    var message = response != null && responseListManagement.response != null ?
                        responseListManagement.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                    await _toastService.Error("Error", message, autoHide: true);
                }

                #endregion


                #region complaints
                await LoadComplaints();
                #endregion
            }

            _spinnerService.Hide();



        }

        private async Task LoadComplaints()
        {
            ManagementProfileInputFilterDto managementProfileInputComplaintFilterDto = new ManagementProfileInputFilterDto()
            {
                AssociationRelatedMemoryId = associationId,
                CreatedUserId = userId,
                PrincipalTypeApplication = PRINCIPALTYPE.CORRUPTION,
                TypeCreation = "EXTERNAL"
            };
            var responseListComplaint = await _managementService.GetAllManagementsFiltered(managementProfileInputComplaintFilterDto);
            if (responseListComplaint != null && responseListComplaint.response != null && responseListComplaint.response.Success)

            {
                // carga la data
                ListComplaints = responseListComplaint.definition;
                var celling = Math.Ceiling((decimal)responseListComplaint.definition.totalCount / 10);
                totalAcountPagesComplaints = (int)celling;
            }
        }

        public async Task OnClickTabItem(TabItem tabItem)
        {

            TabSetTemplate.ActiveTab(tabItem);


            var childText = tabItem.Text;
            switch (childText)
            {
                case "Memorias":
                    ManagementProfileInputFilterDto managementProfileInputFilterDto = new ManagementProfileInputFilterDto();
                    if (!string.IsNullOrEmpty(associationId))
                    {

                        managementProfileInputFilterDto.AssociationRelatedMemoryId = associationId;
                        if (!isLoadingMemories)
                        {
                            await listInstitutionalMemory.UpdateData(managementProfileInputFilterDto, false);
                            isLoadingMemories = true;
                        }
                    }
                    else
                    {
                        managementProfileInputFilterDto.UserAssignedId = userId;
                        if (!isLoadingMemories)
                        {
                            await listInstitutionalMemory.UpdateData(managementProfileInputFilterDto, true);
                            isLoadingMemories = true;
                        }

                    }
                    break;


            }


            StateHasChanged();
        }

        public void HandleCreateCitizenManagment()
        {
            _navigation.NavigateTo("solicitud");
        }
        public void HandleGoToSurvey()
        {
            if (Model.RoleName != ROLEAUDITORIA.Usuario)
            {
                _navigation.NavigateTo("participacionMunicipio");

            }
            else
            {
                _navigation.NavigateTo("participacionCiudadana");
            }

        }

        public async void HandleEmployeeComplaintCallView()
        {
            await Modal.Toggle();
            // _navigation.NavigateTo("/denunciaFuncionario");
        }

        public async void HandleCreateMemory()
        {

            _navigation.NavigateTo("/crearMemoriaInstitucional");
        }

        public async void FollowManagement()
        {

            _navigation.NavigateTo("/seguimientosolicitud");
        }
        public void HandleAllow(ChangeEventArgs e)
        {
            isAllow = (bool)e.Value;
        }

        public async void HandleComplaint()
        {
            if (isAllow)
            {
                isNextView = true;
                Modal.Close();
            }
        }
        public async Task<bool> CloseModal()
        {
            if (isAllow && isNextView)
            {
                _navigation.NavigateTo("/denunciaFuncionario");
            }
            isAllow = false;
            return true;
        }
        public async void HandleCloseModal()
        {
            isNextView = false;
            isAllow = false;
            await Modal.Close();
        }

    }
}
