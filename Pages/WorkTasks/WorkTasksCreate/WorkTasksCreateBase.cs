using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Diagnostics.CodeAnalysis;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using Microsoft.JSInterop;
using LeafletForBlazor;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.CitizenManagement;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Spreadsheet;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Holidays;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Cords;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.WorkTasks.WorkTasksCreate
{
    public class WorkTasksCreateBase : ComponentBase
    {

        [Parameter]
        public string redirect { get; set; }

        public WorkTaskInputCreateDto ModelFirst { get; set; } = new WorkTaskInputCreateDto();

        public CitizenManagmentDto citizenManagmentDto { get; set; } = new CitizenManagmentDto();
        public GetUserForDropDownList ModelSearch = new GetUserForDropDownList();


        [Inject]
        public ICatalogService _catalogService { get; set; }


        [Inject]
        Blazored.LocalStorage.ILocalStorageService localstorage { get; set; }
        [Inject]
        public CustomAuthService _customAuthService { get; set; }


        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();

        public List<SelectedItem> listWorkTaskPosition = new List<SelectedItem>();

        public List<SelectedItem> listDepartmentsSelect = new List<SelectedItem>();
        public List<SelectedItem> listWorkTaskType = new List<SelectedItem>();
        #region districts and Neighborhoods
        [Inject]
        public IDistrictService _districtService { get; set; }

        public List<SelectedItem> listDistricts;
        public List<SelectedItem> listNeighborhoods;
        [Parameter]
        public List<DistrictNeighborhoodsDefinition> listDistrictTotalItems { get; set; }
        #endregion


        public List<string> listDepartmentsName = new List<string>();

        public List<string> listImagesSelected = new List<string>();

        public List<Catalog> listCatalogData = new List<Catalog>();
        [Inject]
        public SpinnerService _spinnerService { get; set; }

        [Inject]
        public IManagementService _managementService { get; set; }

        [Inject]
        public IDepartmentService _departmentService { get; set; }

        [Inject]
        public IHolidaysService _holidaysService { get; set; }
        [Inject]
        public ICatalogAutomaticResponseService _catalogAutomaticResponseService { get; set; }
        [Inject]
        public IUserService _userService { get; set; }




        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }


        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "NewProject:WorkTask";
        public Step? _stepper;
        public bool IsSecondPageReadyToLoad = false;
        public bool SharedUbicationPerson { get; set; } = false;

        public List<DepartmentResponse> departmentListData = new List<DepartmentResponse>();
        public List<UserDepartmentDto> userForListWithDepartment = new List<UserDepartmentDto>();
        public UserCredenditialsDto userCredenditialsDto = new UserCredenditialsDto();



        #region map
        [NotNull]
        public Modal? ModalMap { get; set; }
        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }

        private Coordinates? ModelPosition { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        private IJSObjectReference? module;
        public DotNetObjectReference<WorkTasksCreateBase> self;
        [Inject]
        [NotNull]
        private ToastService? ToastService { get; set; }

        public async Task OpenModal()
        {

            await ModalMap.Toggle();
            await Task.Delay(100);
            StateHasChanged();
        }

        [JSInvokable]
        public void returnOnClickCoordinates(string latitude, string longitude)
        {


            ModelFirst.Latitude = latitude;
            ModelFirst.Longitude = longitude;

            StateHasChanged();
        }

        #endregion



        #region secondPage
        [NotNull]
        public Table<UserDepartmentDto>? Table { get; set; }
        [NotNull]
        public Table<UserDepartmentDto>? TableSecond { get; set; }
        public List<UserDepartmentDto>? SelectedItemsFirstTable { get; set; } = new List<UserDepartmentDto>();




        public List<UserDepartmentDto>? SelectedUsersToSave { get; set; } = new List<UserDepartmentDto>();
        public List<UserDepartmentDto>? SelectedItemsSecondTable { get; set; } = new List<UserDepartmentDto>();

        public List<HolidayDto> holidayDtos { get; set; } = new List<HolidayDto>();

        public IEnumerable<SelectedItem> itemsDepartmentsFilters { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };
        string latitude = BASELOCATION.latitude;
        string longitude = BASELOCATION.longitude;
        #endregion

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                ModelFirst ??= new();
                citizenManagmentDto ??= new();


                var isValid = await _validationRouteService.HasAccessRoute(validScopes);

                if (!isValid)
                {
                    await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                    _navigation.NavigateTo("/login");
                    _spinnerService.Hide();
                    return;
                }




                // Obtener el JWT desde localStorage
                var userTokenData = await _customAuthService.GetClaims();
                var userId = "";
                if (userTokenData != null)
                {
                    var userIdItem = userTokenData.Claims.FirstOrDefault(x => x.Type == "UserId");


                    if (userIdItem != null)
                    {
                        userId = userIdItem.Value;
                    }
                }



                var responseUserCreator = await _userService.GetUserProfile(Guid.Parse(userId));


                _spinnerService.Show();


                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                     "./Pages/WorkTasks/WorkTasksCreate/WorkTasksCreate.razor.js");

                self = DotNetObjectReference.Create(this);
                await module.InvokeVoidAsync("Init", latitude, longitude, self);


                ModelFirst.Latitude = latitude;
                ModelFirst.Longitude = longitude;


                StateHasChanged();




                ModelFirst.ManagementName = responseUserCreator.Name + " " + responseUserCreator.Lastname;
                ModelFirst.ManagementEmail = responseUserCreator.EmailAddress;
                ModelFirst.ManagementPhone = responseUserCreator.PhoneNumber;
                ModelFirst.ManagementPhone = responseUserCreator.PhoneNumber;
                citizenManagmentDto.Identification = responseUserCreator.Identification;


                ModelFirst.CreatedAt = DateTime.Today;
                ModelFirst.PrincipalTypeApplication = PRINCIPALTYPE.MANAGEMENT;

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



                var getAllUserDepartments = await _departmentService.GetDeparmentsWithUsers();
                if (getAllUserDepartments != null && getAllUserDepartments.response != null && getAllUserDepartments.response.Success)
                {

                    userForListWithDepartment = getAllUserDepartments.definition;
                }

                #region load catalog


                CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
                {
                    Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION-MANAGEMENT", "POSITION-WORK-TASK", "TYPE-WORKTASK"]
                };

                var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
                listCatalogData = listAllDataCatalog;



                ////
                var listStatus = listCatalogData.Where(x => x.Code == STATUSMANAGEMENT.NEW || x.Code == STATUSMANAGEMENT.PROCCESS || x.Code == STATUSMANAGEMENT.REVIEW);


                foreach (var item in listStatus)
                {
                    listSelectStatus.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });
                }
                listSelectStatus.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                ////


                ////
                var listypeWorktask = listCatalogData.Where(x => x.Collection == "TYPE-WORKTASK");

                foreach (var item in listypeWorktask)
                {
                    listWorkTaskType.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });
                }
                listWorkTaskType.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
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

                #endregion
                #region load districts

                ///load districts

                var listDistrictsItems = await _districtService.GetDistricts();
                listDistrictTotalItems = listDistrictsItems;
                listDistricts = new List<SelectedItem>();
                listNeighborhoods = new List<SelectedItem>();
                foreach (var item in listDistrictsItems)
                {
                    listDistricts.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });


                }
                listDistricts.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                listNeighborhoods.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                #endregion

                var responseHolidays = await _holidaysService.GetAllHolidaysFilters(new HolidayInputAllDto()
                {
                    GetFutureDays = true
                });



                if (responseHolidays != null && responseHolidays.response != null && responseHolidays.response.Success)
                {
                    holidayDtos = responseHolidays.definition;
                }

                ModelFirst.StartDateApplication = DateTimeOperations.CalculateStartDate(DateTime.Today, holidayDtos);

                var featureDates = DateTimeOperations.CalculateEndDate(ModelFirst.StartDateApplication.Value, DAYSTOADD.days, holidayDtos);
                ModelFirst.DueRateApplication = featureDates;


                StateHasChanged();
                IsSecondPageReadyToLoad = true;





                _spinnerService.Hide();
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


                        ModelFirst.Latitude = latitude;
                        ModelFirst.Longitude = longitude;
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


        #region goTo
        public async Task goToList()
        {
            if (redirect == "tarea")
            {
                _navigation.NavigateTo("/gestionCiudadana/tareas");
            }
            else
            {
                if (redirect == "solicitud")
                {
                    _navigation.NavigateTo("/gestionCiudadana/solicitudes");
                }

            }

        }
        #endregion

        public async Task passToAssingManagement(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;
            await _jsRuntime.InvokeAsync<string>("scrollTop");
            _stepper.Next();
        }

        public void backToDetail()
        {
            _stepper.Prev();
        }


        #region load districts change
        public async Task OnItemChangedDistrict(SelectedItem item)
        {
            var itemSelected = listDistrictTotalItems.FirstOrDefault(x => x.Code == item.Value);
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

                listNeighborhoods = list;
                StateHasChanged();
            }
            else
            {
                listNeighborhoods = list;
                StateHasChanged();
            }
        }

        #endregion



        #region secondPage

        public Task OnItemChanged(DateTime? item)
        {
            if (item != null)
            {
                ModelFirst.StartDateApplication = item.Value;
                //var featureDates = item.Value.AddDays(12);

                var featureDates = DateTimeOperations.CalculateEndDate(item.Value, DAYSTOADD.days, holidayDtos);
                ModelFirst.DueRateApplication = featureDates;
            }
            StateHasChanged();
            return Task.CompletedTask;
        }

        #region table

        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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
        #region second table

        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
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

        #endregion



        #endregion




        public async Task completeDataToWorkTask(EditContext formContext)
        {
            _spinnerService.Show();
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;


            if (!ModelFirst.StartDateApplication.HasValue)
            {
                await _toastService.Error("Información", "Debe seleccionar la fecha de inicio de la tarea para poder guardar, por favor", autoHide: true);
                _spinnerService.Hide();
                return;
            }





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

                //var findName = listDistrictTotalItems.FirstOrDefault(x => x.Code == ModelFirst.DistrictCustom);
                //if (findName != null)
                //{

                //    //ModelFirst.District = findName.DisplayLabel;
                //    if (!string.IsNullOrEmpty(ModelFirst.Neighborhood))
                //    {
                //        var neighborhoodName = findName.NeighborhoodList.FirstOrDefault(x => x.Code == ModelFirst.Neighborhood);
                //        if (neighborhoodName != null)
                //        {
                //            ModelFirst.Neighborhood = neighborhoodName.DisplayLabel;
                //        }
                //    }
                //}

                if (ModelFirst.Latitude == BASELOCATION.latitude && ModelFirst.Longitude == BASELOCATION.longitude)
                {
                    ModelFirst.Latitude = null;
                    ModelFirst.Longitude = null;
                }


                var handleManangement = ModelFirst.ToJson().FromJson<AssingWithoutManagementTaskDto>();
                handleManangement.TypeWorkTask = ModelFirst.TypeWorkTaskCustom;
                handleManangement.District = ModelFirst.DistrictCustom;
                var browserFiles = imageDataUrl.Select(x => x.FileImage);
                handleManangement.attachedFiles = browserFiles.ToList();
                //handleManangement.ManagementId = PrincipalObject.Id;


                var listUserToAssing = new List<WorkTaskUsersAssignedInputDto>();



                foreach (var item in SelectedUsersToSave)
                {
                    var itemAssingWorkTask = item.ToJson().FromJson<WorkTaskUsersAssignedInputDto>();
                    itemAssingWorkTask.UserPositionTask = item.Position;
                    listUserToAssing.Add(itemAssingWorkTask);
                }

                handleManangement.UserDepartmentList = listUserToAssing;



                var response = await _managementService.AssignCreateTaskWithoutManagement(handleManangement);
                if (response != null && response.response.Success)
                {
                    _spinnerService.Hide();
                    var message = response != null && response.response != null ? response.response.Message : "Solicitud asignada con éxito, se estará notificando a los usuarios";
                    await _toastService.Success("¡Proceso correcto!", message, autoHide: true);


                    goToList();
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
                await _toastService.Error("Información", "Selecciona un usuario de la lista para poder asignar la tarea", autoHide: true);
                _spinnerService.Hide();
                return;
            }


        }



        #endregion


        #region images

        #region add new files
        public List<EvidenciaDto> imageDataUrl = new List<EvidenciaDto>();
        private const long MaxFileSize = 10240000L; // 500 KB

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



        #endregion

        #region delete
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

        #endregion

        #endregion
        #region search citizen
        public async Task SearchCitizen()
        {
            _spinnerService.Show();
            if (!string.IsNullOrEmpty(citizenManagmentDto.IdentificationP))
            {

                UserIdentificationInputDto input = new UserIdentificationInputDto
                {
                    Identification = citizenManagmentDto.IdentificationP
                };
                var list = await _userService.GetAllUserResponse(input);
                var users = list.definition;

                var userFind = users.FirstOrDefault(x => x.Identification == citizenManagmentDto.IdentificationP);
                if (userFind != null)
                {
                    ModelFirst.ExternalManagementName = userFind.Name + " " + userFind.Lastname;
                    ModelFirst.ExternalManagementEmail = userFind.EmailAddress;
                    ModelFirst.ExternalManagementPhone = userFind.PhoneNumber ?? "";

                }
                else
                {
                    ModelFirst.ExternalManagementName = "";
                    ModelFirst.ExternalManagementEmail = "";
                    ModelFirst.ExternalManagementPhone = "";
                    await _toastService.Information("Añadir participante", "No se encontro el participante, puede digitar el nombre, correo y teléfono", autoHide: true);
                }
            }
            else
            {
                await _toastService.Information("Añadir participante", "Se debe de un indicar una identificación", autoHide: true);

            }

            StateHasChanged();
            _spinnerService.Hide();
        }

        #endregion

        #region dates


        public DateTimePicker<DateTime?> _picker1 = default!;
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
        #endregion
    }
}
