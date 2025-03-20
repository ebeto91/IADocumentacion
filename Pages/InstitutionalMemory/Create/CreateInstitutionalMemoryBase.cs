using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using NPOI.HSSF.Record;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Users.Create;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Diagnostics.CodeAnalysis;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.InstitutionalMemory;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CitizenManagement;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;
using DocumentFormat.OpenXml.Spreadsheet;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using System.Security.Claims;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Cords;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.InstitutionalMemory.Create
{
    public class CreateInstitutionalMemoryBase : ComponentBase
    {
        public CreateManagementInputDto createManagementInputDto = new CreateManagementInputDto();
        public bool isDisabledSelect { get; set; } = false;
        public bool SharedUbicationPerson { get; set; } = false;
        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();

        public List<SelectedItem> listSelectDistrict = new List<SelectedItem>();

        public List<SelectedItem> listSelectAssociation = new List<SelectedItem>();

        public List<SelectedItem> listSelectNeighborhood = new List<SelectedItem>();

        public List<SelectedItem> listWorkTaskPosition = new List<SelectedItem>();

        public string? associatedId { get; set; }

        public string? RoleName { get; set; }

        public int characterCount = 0;

        string latitude = BASELOCATION.latitude;
        string longitude = BASELOCATION.longitude;

        public DotNetObjectReference<CreateInstitutionalMemoryBase> self;

        [NotNull]
        public Modal? ModalMap { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]

        public IInstitutionalMemoryService _institutionalMemoryService { get; set; }

        private const long MaxFileSize = 10240000L; // 10mb

        [NotNull]
        public Table<UserDepartmentDto>? TableSecond { get; set; }

        public List<UserDepartmentDto>? SelectedItemsFirstTable { get; set; } = new List<UserDepartmentDto>();

        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };

        [NotNull]
        public Table<UserDepartmentDto>? Table { get; set; }

        public List<DepartmentResponse> departmentListData = new List<DepartmentResponse>();
        public List<UserDepartmentDto> userForListWithDepartment = new List<UserDepartmentDto>();

        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IHolidaysService _holidaysService { get; set; }
        public List<UserDepartmentDto>? SelectedUsersToSave { get; set; } = new List<UserDepartmentDto>();
        public List<UserDepartmentDto>? SelectedItemsSecondTable { get; set; } = new List<UserDepartmentDto>();



        private Coordinates? ModelPosition { get; set; }

        public bool isReadyHolydays = false;

        public IEnumerable<SelectedItem> itemsDepartmentsFilters { get; set; }

        [Parameter]
        public string managementId { get; set; }

        public FullManagementDto ModelFirst { get; set; } = new FullManagementDto();

        public UserProfileResponse UserCreator { get; set; } = new UserProfileResponse();
        public FullManagementDto PrincipalObject { get; set; } = new FullManagementDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }



        public List<SelectedItem> listDepartmentsSelect = new List<SelectedItem>();

        public List<string> listDepartmentsName = new List<string>();

        public List<string> listImagesSelected = new List<string>();

        public List<Catalog> listCatalogData = new List<Catalog>();


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

        public List<EvidenciaDto> imageDataUrl = new List<EvidenciaDto>();

        [Inject]
        public IAssociationService _associationService { get; set; }

        public UserProfileResponse Model { get; set; }
        public List<HolidayDto> holidayDtos { get; set; } = new List<HolidayDto>();

        public List<AssociationResponseGroupByDistrict> associationResponseGroupByDistricts = new List<AssociationResponseGroupByDistrict>();

        public List<NeighborhoodTemp> neighborhoodTemp = new List<NeighborhoodTemp>();

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "Edit:Management";
        public Step? _stepper;
        public bool IsSecondPageReadyToLoad = false;


        public List<CatalogAutomaticResponseDto> listCatalogAutomaticResponse = new List<CatalogAutomaticResponseDto>();

        [Inject]
        public CustomAuthService _customAuthService { get; set; }
        [Inject]
        Blazored.LocalStorage.ILocalStorageService localstorage { get; set; }

        [NotNull]
        public ImagePreviewer? ImagePreviewer { get; set; }


        protected async override Task OnInitializedAsync()
        {
            _spinnerService.Show();


            // Obtener el JWT desde localStorage
            var userTokenData = await _customAuthService.GetClaims();


            if (userTokenData != null)
            {
                _spinnerService.Show();


                var role = userTokenData.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                var associationItem = userTokenData.Claims.FirstOrDefault(x => x.Type == "AsociationId");

                if (role != null)
                {
                    RoleName = role.Value;
                }

                if (associationItem != null)
                {
                    associatedId = associationItem.Value;
                }

            }

            /*   var isValid = await _validationRouteService.HasAccessRoute(validScopes);

               if (!isValid)
               {
                   await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                   _navigation.NavigateTo("/login");
                   _spinnerService.Hide();
                   return;
               }*/


            //      var response = await _managementService.GetManagementById(managementId);



            var getDepartments = await _departmentService.GetAllDepartments();
            if (getDepartments != null && getDepartments.response != null && getDepartments.response.Success)
            {

                departmentListData = getDepartments.definition.items;


                listDepartmentsSelect.Add(new SelectedItem()
                {
                    Text = "Seleccionar para filtrar",
                    Value = ""
                });

                foreach (var item in departmentListData)
                {
                    listDepartmentsSelect.Add(new SelectedItem()
                    {
                        Text = item.Name,
                        Value = item.Id.ToString()
                    });
                }
            }


            if (string.IsNullOrEmpty(associatedId))
            {
                var getAllUserDepartments = await _departmentService.GetDeparmentsWithUsers();
                if (getAllUserDepartments != null && getAllUserDepartments.response != null && getAllUserDepartments.response.Success)
                {

                    userForListWithDepartment = getAllUserDepartments.definition;

                    await Table.QueryAsync();
                }
            }



            var objectResponseCatalog = await _catalogAutomaticResponseService.GetCatalogResponseForList(new CatalogResponseInputListDto()
            {
                Enabled = true
            });

            if (objectResponseCatalog != null && objectResponseCatalog.response != null && objectResponseCatalog.response.Success)
            {

                listCatalogAutomaticResponse = objectResponseCatalog.definition;
            }



            StateHasChanged();
            IsSecondPageReadyToLoad = true;






            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION", "POSITION-WORK-TASK", "DISTRICT", "NEIGHBORHOOD", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT"]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;



            ////
            var listStatus = listCatalogData.Where(x => x.Collection == "STATUS-MANAGEMENT");
            var list = listStatus.Where(x => x.Code == "NEW").ToList();

            foreach (var item in list)
            {
                listSelectStatus.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }

            //  listSelectStatus.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////


            ////
            var listPriority = listCatalogData.Where(x => x.Collection == "PRIORITY").OrderBy(x => x.Ref2);

            foreach (var item in listPriority)
            {
                listSelectPriority.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectPriority.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////

            ////
            var listSubtype = listCatalogData.Where(x => x.Collection == "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT");
            foreach (var item in listSubtype)
            {
                listSelectType.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listSelectType.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////
            ///

            ////
            var listPosition = listCatalogData.Where(x => x.Collection == "POSITION-WORK-TASK");
            foreach (var item in listPosition)
            {
                listWorkTaskPosition.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code,
                });
            }
            listWorkTaskPosition.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            ////
            ///


            var listNeighborhood = listCatalogData.Where(x => x.Collection == "NEIGHBORHOOD");
            foreach (var item in listNeighborhood)
            {
                NeighborhoodTemp neighborhoodTempV = new NeighborhoodTemp
                {
                    TextCustom = item.DisplayLabel,
                    ValueCustom = item.Code,
                    Ref = item.Ref2
                };
                neighborhoodTemp.Add(neighborhoodTempV);
            }
            //listSelectDistrict.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));


            var listDistrict = listCatalogData.Where(x => x.Collection == "DISTRICT");

            var data = await _associationService.GetAssociationGroupByDistrict();
            if (data != null && data.response.Success)
            {
                associationResponseGroupByDistricts = data.definition;

                if (!string.IsNullOrEmpty(associatedId))
                {
                    foreach (var item in associationResponseGroupByDistricts)
                    {
                        foreach (var aso in item.ListAssociations)
                        {
                            if (aso.Id == Guid.Parse(associatedId))
                            {
                                var exist = listDistrict.FirstOrDefault(x => x.Code == item.CodeDistrict);
                                createManagementInputDto.District = item.CodeDistrict + "," + exist.Ref1;
                                createManagementInputDto.AssociationRelatedMemoryId = Guid.Parse(associatedId);
                                isDisabledSelect = true;
                            }
                        }
                    }
                }


            }



            foreach (var item in listDistrict)
            {
                listSelectDistrict.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Code + "," + item.Ref1,

                });
            }
            listSelectDistrict.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));

            var responseHolidays = await _holidaysService.GetAllHolidaysFilters(new HolidayInputAllDto()
            {
                GetFutureDays = true
            });



            if (responseHolidays != null && responseHolidays.response != null && responseHolidays.response.Success)
            {
                holidayDtos = responseHolidays.definition;

                isReadyHolydays = true;

            }

            _spinnerService.Hide();
        }


        public Task OnItemChanged(DateTime? item)
        {
            if (item != null)
            {
                createManagementInputDto.StartDateApplication = item.Value;
                var featureDates = DateTimeOperations.CalculateEndDate(item.Value, DAYSTOADD.days, holidayDtos);
                createManagementInputDto.DueRateApplication = featureDates;
            }
            StateHasChanged();
            return Task.CompletedTask;
        }

        public async Task<QueryData<UserDepartmentDto>> OnQueryAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            IEnumerable<UserDepartmentDto> items = userForListWithDepartment;

            var filters = options.ToFilter();

            if (filters != null && filters.Filters.Count > 0)
            {
                items = ApplyFiltersData(filters, items);
            }


            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();
            foreach (var item in items)
            {
                var itemSelected = SelectedItemsFirstTable.FirstOrDefault(x => x.Id == item.Id);
                if (itemSelected != null)
                {
                    item.Selected = true;
                }
            }







            StateHasChanged();
            return new QueryData<UserDepartmentDto>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }

        public void CheckboxChanged(ChangeEventArgs e, TableColumnContext<UserDepartmentDto, Guid> userResponseAssing)
        {
            // get the checkbox state
            bool? x = e.Value as bool?;
            if (x != null)
            {
                if (x.Value)
                {
                    SelectedItemsFirstTable.Add(userResponseAssing.Row);
                    userResponseAssing.Row.Selected = true;
                }
                else
                {
                    SelectedItemsFirstTable.Remove(userResponseAssing.Row);
                    userResponseAssing.Row.Selected = false;
                }
                StateHasChanged();
            }
        }
        private List<UserDepartmentDto> ApplyFiltersData(FilterKeyValueAction filters, IEnumerable<UserDepartmentDto> itemList)
        {
            List<UserDepartmentDto> itemListFilter = itemList.ToList();
            InputsToApplyInFilterTable inputsToApplyInFilterTable = new InputsToApplyInFilterTable();

            foreach (var item in filters.Filters)
            {
                var itemFilter = item.Filters;
                foreach (var itemDataFilter in itemFilter)
                {
                    if (itemDataFilter != null && !string.IsNullOrEmpty(itemDataFilter.FieldKey) && itemDataFilter.FieldValue != null)
                    {
                        var key = itemDataFilter.FieldKey;
                        switch (key)
                        {
                            case "EmailAddress":
                                var emailAddress = itemDataFilter.FieldValue?.ToString()?.Trim();

                                if (!string.IsNullOrEmpty(emailAddress))
                                {

                                    inputsToApplyInFilterTable.EmailAddress = emailAddress;
                                }

                                break;

                            case "DepartmentId":
                                var departmentId = itemDataFilter.FieldValue?.ToString()?.Trim();

                                if (!string.IsNullOrEmpty(departmentId))
                                {
                                    var parseGuid = Guid.Parse(departmentId);
                                    inputsToApplyInFilterTable.DepartmentId = parseGuid;
                                }

                                break;
                            default:
                                break;
                        }

                    }
                }

            }
            var filterApply = itemListFilter;

            if (!string.IsNullOrEmpty(inputsToApplyInFilterTable.EmailAddress))
            {
                filterApply = itemListFilter.Where(x => x.EmailAddress.Contains(inputsToApplyInFilterTable.EmailAddress)).ToList();
            }

            if (inputsToApplyInFilterTable.DepartmentId.HasValue)
            {
                filterApply = filterApply.Where(x => x.DepartmentId == inputsToApplyInFilterTable.DepartmentId).ToList();
            }

            itemListFilter = filterApply;
            return itemListFilter;
        }

        #region buttons

        public async Task passToFirstToSecondTable()
        {

            if (SelectedItemsFirstTable.Count > 0)
            {

                var removeItemSelected = userForListWithDepartment.Where(x => !x.Selected)
                    .OrderByDescending(x => x.NameDepartment)
                    .ToList();

                userForListWithDepartment = removeItemSelected;

                var listSelectedFirst = SelectedItemsFirstTable
                  .OrderByDescending(x => x.NameDepartment)
                  .ToList();
                //agrega a la primera lista
                foreach (var item in listSelectedFirst)
                {
                    item.Selected = false;
                    item.Position = "";
                    var clone = Utility.Clone(item);
                    SelectedUsersToSave.Add(clone);
                }

                SelectedItemsFirstTable = new List<UserDepartmentDto>();

                await TableSecond.QueryAsync();
                await Table.QueryAsync();

                StateHasChanged();
            }
            else
            {
                await _toastService.Warning("Información", "Selecciona al menos un usuario de la lista de colaboradores", autoHide: true);
            }
        }

        public async Task<List<DateTime>> OnGetDisabledDaysCallback(DateTime start, DateTime end)
        {

            var ret = new List<DateTime>();
            if (true)
            {
                var day = start;
                while (day <= end)
                {
                    if (day.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
                    {
                        ret.Add(day);
                    }

                    var isDayInList = holidayDtos.FirstOrDefault(x => x.DateSelected == day);
                    if (isDayInList != null)
                    {
                        ret.Add(day);
                    }

                    day = day.AddDays(1);
                }

                if (DateTime.Today.DayOfWeek is DayOfWeek.Sunday or DayOfWeek.Saturday)
                {
                    // 处理今天是否禁用
                    ret.Add(DateTime.Today);
                }
            }

            await Task.Delay(100);
            return ret;
        }
        public async Task passToSecondToFirstTable()
        {

            if (SelectedItemsSecondTable.Count > 0)
            {
                var removeItemSelected = SelectedUsersToSave.Where(x => !x.Selected)
             .OrderByDescending(x => x.NameDepartment)
             .ToList();

                SelectedUsersToSave = removeItemSelected;

                var listSelectedFirst = SelectedItemsSecondTable
                  .OrderByDescending(x => x.NameDepartment)
                  .ToList();

                //agrega a la primera lista
                foreach (var item in listSelectedFirst)
                {
                    item.Selected = false;
                    item.Position = "";
                    var clone = Utility.Clone(item);
                    userForListWithDepartment.Add(clone);
                }

                userForListWithDepartment =
                    [.. userForListWithDepartment.OrderByDescending(x => x.NameDepartment)];


                SelectedItemsSecondTable = new List<UserDepartmentDto>();

                await TableSecond.QueryAsync();
                await Table.QueryAsync();

                StateHasChanged();
            }
            else
            {
                await _toastService.Warning("Información", "Selecciona al menos un usuario de la lista de colaboradores asignados para devolverlos", autoHide: true);
            }

        }
        #endregion

        public async Task<QueryData<UserDepartmentDto>> OnQuerySecondTableAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            IEnumerable<UserDepartmentDto> items = SelectedUsersToSave;
            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();
            foreach (var item in items)
            {
                var itemSelected = SelectedItemsSecondTable.FirstOrDefault(x => x.Id == item.Id);
                if (itemSelected != null)
                {
                    item.Selected = true;
                }
            }
            listDepartmentsName = new List<string>();
            listDepartmentsName = SelectedUsersToSave.Select(x => x.NameDepartment).Distinct().ToList();
            StateHasChanged();
            return new QueryData<UserDepartmentDto>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }



        public void CheckboxChangedSecondTable(ChangeEventArgs e, TableColumnContext<UserDepartmentDto, Guid> userResponseAssing)
        {
            // get the checkbox state
            bool? x = e.Value as bool?;
            if (x != null)
            {
                if (x.Value)
                {
                    SelectedItemsSecondTable.Add(userResponseAssing.Row);
                    userResponseAssing.Row.Selected = true;
                }
                else
                {
                    SelectedItemsSecondTable.Remove(userResponseAssing.Row);
                    userResponseAssing.Row.Selected = false;
                }
                StateHasChanged();
            }
        }

        public Task OnItemChanged(SelectedItem item)
        {
            var valuePick = item.Value;
            if (valuePick != null)
            {
                var list = associationResponseGroupByDistricts.FirstOrDefault(x => x.CodeDistrict == item.Value.Split(",")[0]);

                if (list != null && list.ListAssociations.Count > 0)
                {
                    if (listSelectAssociation.Count > 0)
                    {
                        listSelectAssociation.Clear();
                    }

                    foreach (var itemDistrictssociation in list.ListAssociations)
                    {
                        listSelectAssociation.Add(new SelectedItem()
                        {
                            Text = itemDistrictssociation.Name,
                            Value = itemDistrictssociation.Id.ToString(),
                        });
                    }

                    listSelectAssociation.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                }
                else
                {

                    listSelectAssociation.Clear();

                    listSelectAssociation.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                }
            }
            else
            {
                listSelectAssociation.Clear();

                listSelectAssociation.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            }

            ListNeighborhood(item);
            StateHasChanged();
            return Task.CompletedTask;
        }

        public void ListNeighborhood(SelectedItem item)
        {
            var valuePick = item.Value;
            if (valuePick != null)
            {
                var list = neighborhoodTemp.Where(x => x.Ref == item.Value.Split(",")[1]).ToList();

                if (list != null)
                {
                    if (listSelectNeighborhood.Count > 0)
                    {
                        listSelectNeighborhood.Clear();
                    }

                    foreach (var itemNeighborhood in list)
                    {
                        listSelectNeighborhood.Add(new SelectedItem()
                        {
                            Text = itemNeighborhood.TextCustom,
                            Value = itemNeighborhood.ValueCustom,
                        });
                    }

                    listSelectNeighborhood.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                }
                else
                {

                    listSelectNeighborhood.Clear();

                    listSelectNeighborhood.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                }
            }
            else
            {
                listSelectAssociation.Clear();

                listSelectAssociation.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
            }


        }

        public async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            var imageFiles = e.GetMultipleFiles();
            var format = "image/png";
            foreach (var image in imageFiles)
            {
                if (image.Size > MaxFileSize)
                {
                    // Mostrar mensaje de error
                    await _toastService.Error($"Ha ocurrido un error con la imagen {image.Name}", $"El archivo excede el tamaño máximo permitido de 10 Mb.", autoHide: true);

                }
                else
                {
                    var resizeImageFile = await image.RequestImageFileAsync(format, 250, 250);
                    var buffer = new byte[resizeImageFile.Size];
                    await resizeImageFile.OpenReadStream(maxAllowedSize: MaxFileSize).ReadAsync(buffer);

                    var imageDataUrlLink = $"data:{format};base64,{Convert.ToBase64String(buffer)}";

                    EvidenciaDto evidenciaDto = new EvidenciaDto()
                    {
                        imageUrl = imageDataUrlLink,
                        Identificador = Guid.NewGuid().ToString(),
                        Delete = false,
                        FileImage = image

                    };
                    imageDataUrl.Add(evidenciaDto);
                }
            }


        }

        public async void HandleDeleteImage(string id)
        {

            if (imageDataUrl.Count > 0)
            {



                //  await module.InvokeVoidAsync("DeleteImage", id);
                var image = imageDataUrl.FirstOrDefault(x => x.Identificador == id);
                if (image != null)
                {
                    imageDataUrl.Remove(image);
                    //image.Delete = true;
                }
            }
            StateHasChanged();
        }

        public void UpdateCharacterCount(ChangeEventArgs e)
        {
            var input = e.Value?.ToString() ?? string.Empty;
            characterCount = input.Length;
        }

        public async Task OpenModal()
        {

            await ModalMap.Toggle();
            await Task.Delay(100);
            //parameters = new RealTimeMap.LoadParameters()  //general map settings
            //{
            //    location = new RealTimeMap.Location()
            //    {
            //        latitude = 9.864840399542144,
            //        longitude = -83.92053186893465,
            //    },
            //    zoomLevel = 18
            //};
            StateHasChanged();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {


                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                     "./Pages/CitizenManagement/CitizenManagment.razor.js");

                self = DotNetObjectReference.Create(this);
                await module.InvokeVoidAsync("Init", latitude, longitude, self);


                createManagementInputDto.LatitudeS = latitude.ToString();
                createManagementInputDto.LongitudeS = longitude.ToString();

                createManagementInputDto.Latitude = latitude;
                createManagementInputDto.Longitude = longitude;

                StateHasChanged();

            }

        }


        public async Task AskForActualUbication()
        {
            SweetAlertResult result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Compartir Ubicación",
                Text = "¿Se encuentra en la misma ubicación del reporte del incidente?",
                Icon = SweetAlertIcon.Info,
                AllowEscapeKey = false,
                ShowCancelButton = true,
                CancelButtonColor = "#D9001B",
                ConfirmButtonText = "Compartir ubicación",
                ConfirmButtonColor = "#0755A3",
                CancelButtonText = "No"
            });

            if (result != null && result.IsConfirmed)
            {
                _spinnerService.Show();
                string positionServiceData = await _jsRuntime.InvokeAsync<string>("getCurrentPosition");
                if (positionServiceData != null)
                {

                    ModelPosition = positionServiceData.FromJson<Coordinates>();
                    if (ModelPosition.coords.accuracy < 600)
                    {
                        var format = String.Format("{0:0.000000}", ModelPosition.coords.latitude);
                        //latitude = ModelPosition.LastLat.ToString("0.000000").Replace(",", ".");
                        latitude = format.Replace(",", ".");
                        //longitude = ModelPosition.LastLong.ToString("0.000000").Replace(",", ".");
                        var formatLongitude = String.Format("{0:0.000000}", ModelPosition.coords.longitude);
                        longitude = formatLongitude.Replace(",", ".");


                        await module.InvokeVoidAsync("AddedMark", latitude, longitude);


                        createManagementInputDto.LatitudeS = latitude.ToString();
                        createManagementInputDto.LongitudeS = longitude.ToString();

                        createManagementInputDto.Latitude = latitude;
                        createManagementInputDto.Longitude = longitude;
                        SharedUbicationPerson = true;
                        StateHasChanged();


                    }
                    else
                    {
                        await ToastService.Information("Obtener ubicación", "No se logro obtener con presicion su ubicación actual, por favor utilizar el mapa.", autoHide: true);
                    }
                }

                _spinnerService.Hide();

            }



            StateHasChanged();
        }

        [JSInvokable]
        public void returnOnClickCoordinates(string latitude, string longitude)
        {
            createManagementInputDto.LatitudeS = latitude.ToString();
            createManagementInputDto.LongitudeS = longitude.ToString();

            createManagementInputDto.Latitude = latitude;
            createManagementInputDto.Longitude = longitude;

            StateHasChanged();
        }

        public async void HandleCreateInstitutionalMemory()
        {
            _spinnerService.Show();
            var images = imageDataUrl
          .Where(e => e.FileImage != null)
          .Select(e => e.FileImage)
          .ToList();

            createManagementInputDto.attachedFiles = images;
            createManagementInputDto.PrincipalTypeApplication = PRINCIPALTYPE.INSTITUTIONALMEMORY;

            if (createManagementInputDto.Latitude == BASELOCATION.latitude && createManagementInputDto.Longitude == BASELOCATION.longitude)
            {
                createManagementInputDto.Latitude = null;
                createManagementInputDto.Longitude = null;
            }


            var listUserToAssing = new List<WorkTaskUsersAssignedInputDto>();


            if (SelectedUsersToSave != null && SelectedUsersToSave.Count > 0)
            {

                var listInvalidUsers = SelectedUsersToSave.Where(x => string.IsNullOrEmpty(x.Position)).ToList();

                if (listInvalidUsers.Count > 0)
                {
                    await _toastService.Error("Información", "Los puestos de la tarea son requeridos, " +
                        "por favor colocar a cada uno de los colaboradores", autoHide: true);
                    _spinnerService.Hide();
                    return;
                }


                var listUserPrincipal = SelectedUsersToSave
                    .Select(x => x.DepartmentId)
                    .Distinct().ToList();


                var listInChargeUsers = SelectedUsersToSave
                    .Where(x => x.Position == USERPOSITIONTASK.INCHARGE)
                    .Select(x => x.DepartmentId).Distinct().ToList();

                if (listUserPrincipal.Count != listInChargeUsers.Count)
                {
                    await _toastService.Error("Información", "Para cada departamento asignado debe colocar al menos un usuario encargado de la tarea", autoHide: true);
                    _spinnerService.Hide();
                    return;
                }


                foreach (var item in SelectedUsersToSave)
                {
                    var itemAssingWorkTask = item.ToJson().FromJson<WorkTaskUsersAssignedInputDto>();
                    itemAssingWorkTask.UserPositionTask = item.Position;
                    listUserToAssing.Add(itemAssingWorkTask);
                }
            }
            createManagementInputDto.UserDepartmentList = listUserToAssing;

            createManagementInputDto.District = !string.IsNullOrEmpty(createManagementInputDto.District) ? createManagementInputDto.District.Split(",")[0] : "";

            if (RoleName == ROLEAUDITORIA.Usuario)
            {
                createManagementInputDto.TypeCreation = TYPEPROCCESS.EXTERNAL;
                if (!string.IsNullOrEmpty(associatedId))
                {
                    createManagementInputDto.AssociationRelatedMemoryId = Guid.Parse(associatedId);
                }
            }
            else
            {
                createManagementInputDto.TypeCreation = TYPEPROCCESS.INTERNAL;
            }

            var result = await _institutionalMemoryService.CreateInstitutionalMemory(createManagementInputDto);
            if (result != null && result.response.Success)
            {
                _spinnerService.Hide();
                await ToastService.Success("Solicitud", result.response.Message, autoHide: true);
                if (RoleName == ROLEAUDITORIA.Usuario && !string.IsNullOrEmpty(associatedId))
                {
                    _navigation.NavigateTo("/miperfil");
                }
                else
                {
                    _navigation.NavigateTo("/memoriaInstitucional");
                }
            }
            else
            {
                _spinnerService.Hide();
                await ToastService.Error("Solicitud", "Ha ocurrido un problema, por favor intentarlo de nuevo.", autoHide: true);
            }
        }

        public async Task goToList()
        {
            _navigation.NavigateTo("javascript:history.back()");
        }

    }
}
