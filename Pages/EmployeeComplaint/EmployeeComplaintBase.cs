using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using NPOI.SS.Formula.Functions;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.EmployeeComplaint
{
    public class EmployeeComplaintBase : ComponentBase
    {
        [Inject]
        public ToastService _toastService { get; set; }

        public bool isError = false;
        public List<EvidenciaDto> evidenceDataUrl = new List<EvidenciaDto>();
        [Inject]
        public NavigationManager Navigation { get; set; }

        public CitizenManagmentDto citizenManagmentDto = new CitizenManagmentDto();

        public int characterCount = 0;

        [Inject]
        public ICitizenManagment _citizenManagmentService { get; set; }

        public bool isAnonymous { get; set; } = false;

        public bool isNotification { get; set; } = false;
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public CustomAuthService _customAuthService { get; set; }

        public CreateManagementInputDto createManagementInputDto = new CreateManagementInputDto();

        public DenunciaFuncionario denunciaFuncionario = new DenunciaFuncionario();



        public List<DistrictNeighborhoodsDefinition> listDistricts { get; set; } = new List<DistrictNeighborhoodsDefinition>();

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        [Inject]
        public IUserService _userService { get; set; }

        public UserProfileResponse userResponseManagementClone { get; set; }

        [Inject]
        public IDistrictService _districtService { get; set; }

        public IEnumerable<SelectedItem> itemsCatalogSelect;

        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();
            createManagementInputDto.Title = "Denuncia";

            var userTokenData = await _customAuthService.GetClaims();
            var userId = "";
            if (userTokenData != null)
            {

                var idToken = userTokenData.Claims.FirstOrDefault(c => c.Type == "UserId");
                var response = await _userService.GetUserProfile(Guid.Parse(idToken.Value));
                if (response != null && response.Name != null)
                {


                    userResponseManagementClone = Utility.Clone(response);

                    createManagementInputDto.ManagementName = userResponseManagementClone.Name + " " + userResponseManagementClone.Lastname;
                    createManagementInputDto.ManagementEmail = userResponseManagementClone.EmailAddress;
                    createManagementInputDto.ManagementPhone = userResponseManagementClone.PhoneNumber;





                    StateHasChanged();
                }

                List<SelectedItem> listSelect = new List<SelectedItem>();

                listDistricts = await _districtService.GetDistricts();

                foreach (var item in listDistricts)
                {
                    listSelect.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });


                }
                listSelect.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));

                itemsCatalogSelect = listSelect;
                int x = 0;


            }







            _spinnerService.Hide();
            //return base.OnInitializedAsync();
        }

        public async void HandleEmployeeComplaint()
        {
            _spinnerService.Show();
            createManagementInputDto.IsAnonymous = isAnonymous;
            createManagementInputDto.District = denunciaFuncionario.DistrictCustom;
            createManagementInputDto.ContactPoint = denunciaFuncionario.ContactPointCustom;
            createManagementInputDto.DateIndicident = denunciaFuncionario.DateIndicident;
            createManagementInputDto.NameUserIncident = denunciaFuncionario.NameUserIncident;
            createManagementInputDto.PrincipalTypeApplication = PRINCIPALTYPE.CORRUPTION;
            createManagementInputDto.AllowContact = isNotification;
            createManagementInputDto.Description = denunciaFuncionario.Description;


            var images = evidenceDataUrl.Where(e => e.FileImage != null && e.Delete == false).Select(e => e.FileImage).ToList();
            createManagementInputDto.attachedFiles = images;


            var findDistrict = itemsCatalogSelect.FirstOrDefault(x => x.Value == createManagementInputDto.District);


            createManagementInputDto.Title = createManagementInputDto.Title + $" Distrito: {findDistrict?.Text}" + $" Día incidente: {((DateTime)createManagementInputDto.DateIndicident).ToString("dd/MM/yyyy")}";

            var result = await _citizenManagmentService.CreateCitizenManagment(createManagementInputDto);
            if (result != null && result.response.Success)
            {
                _spinnerService.Hide();
                await ToastService.Success("Denuncia", result.response.Message, autoHide: true);
                Navigation.NavigateTo("/miperfil");
            }
            else
            {
                var message = result != null && result.response != null ? result.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                _spinnerService.Hide();
                await ToastService.Error("Denuncia", message, autoHide: true);
            }
        }

        public void UpdateCharacterCount(ChangeEventArgs e)
        {
            var input = e.Value?.ToString() ?? string.Empty;
            characterCount = input.Length;
        }

        public void ValidateInput(FocusEventArgs e)
        {

        }

        public void HandleNotification(ChangeEventArgs e)
        {
            isNotification = (bool)e.Value;
        }

        public void HandleAnonymous(ChangeEventArgs e)
        {
            isAnonymous = (bool)e.Value;
        }

        #region table

        [NotNull]
        public Table<EvidenciaDto>? Table { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<EvidenciaDto>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<EvidenciaDto> items = new List<EvidenciaDto>();
            if (evidenceDataUrl != null)
            {
                items = evidenceDataUrl.Where(x => !x.Delete);

                return new QueryData<EvidenciaDto>()
                {
                    Items = items,
                    TotalCount = items.Count(),
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };
            }
            else
            {
                return new QueryData<EvidenciaDto>()
                {
                    Items = items,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };

            }





        }

        #region add new files

        private const long MaxFileSize = 10240000L; // 500 KB
        public async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();

            foreach (var file in imageFiles)
            {
                if (file.Size > MaxFileSize)
                {
                    // Mostrar mensaje de error
                    await _toastService.Error($"Ha ocurrido un error con la imagen {file.Name}", $"El archivo excede el tamaño máximo permitido de 10 MB.", autoHide: true);

                }
                else
                {

                    var format = file.ContentType;
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream(MaxFileSize).ReadAsync(buffer);

                    var imageDataUrlLink = await _jsRuntime.InvokeAsync<string>("generateLink", file.Name, format, buffer);

                    var newId = Guid.NewGuid();
                    EvidenciaDto evidenciaDto = new EvidenciaDto()
                    {
                        imageUrl = imageDataUrlLink,
                        Identificador = Guid.NewGuid().ToString(),
                        Delete = false,
                        FileImage = file

                    };

                    if (evidenceDataUrl == null)
                    {
                        evidenceDataUrl = new List<EvidenciaDto>();
                    }

                    evidenceDataUrl.Add(evidenciaDto);
                }
            }

        }


        #region delete
        public async Task deleteInfo(TableColumnContext<EvidenciaDto, string> item)
        {
            _spinnerService.Show();

            var itemIndex = evidenceDataUrl.FindIndex(x => x.Identificador == item.Row.Identificador);
            if (itemIndex >= 0)
            {
                var itemFound = evidenceDataUrl[itemIndex];
                itemFound.Delete = true;
                evidenceDataUrl[itemIndex] = itemFound;
            }

            await Table.QueryAsync();
            _spinnerService.Hide();
        }

        #endregion

        #endregion
        #endregion

    }
}
