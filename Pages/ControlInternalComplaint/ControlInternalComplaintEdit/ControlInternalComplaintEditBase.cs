using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services;
using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalComplaint.ControlInternalComplaintEdit
{
    public class ControlInternalComplaintEditBase : ComponentBase
    {

        [Parameter]
        public string managementId { get; set; }

        public FullComplaitDto ModelFirst { get; set; } = new FullComplaitDto();

        public UserProfileResponse UserCreator { get; set; } = new UserProfileResponse();
        public FullComplaitDto PrincipalObject { get; set; } = new FullComplaitDto();
        [Inject]
        public ICatalogService _catalogService { get; set; }

        public List<SelectedItem> listSelectStatus = new List<SelectedItem>();

        public List<SelectedItem> listSelectPriority = new List<SelectedItem>();

        public List<SelectedItem> listSelectType = new List<SelectedItem>();

        public List<SelectedItem> listWorkTaskPosition = new List<SelectedItem>();

        public List<SelectedItem> listDepartmentsSelect = new List<SelectedItem>();

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
        public ICatalogAutomaticResponseService _catalogAutomaticResponseService { get; set; }
        [Inject]
        public IUserService _userService { get; set; }
        [Inject]
        public IDistrictService _districtService { get; set; }
        [Inject]
        public NavigationManager _navigation { get; set; }

        [Inject]
        public ToastService _toastService { get; set; }

        [Inject]
        public SweetAlertService _sweetAlertService { get; set; }

        [Inject]
        public IValidationRouteService _validationRouteService { get; set; }
        private string validScopes { get; } = "Edit:Complaint";
        public Step? _stepper;
        public bool IsSecondPageReadyToLoad = false;

        public List<DepartmentResponse> departmentListData = new List<DepartmentResponse>();
        public List<UserDepartmentDto> userForListWithDepartment = new List<UserDepartmentDto>();
        public List<CatalogAutomaticResponseDto> listCatalogAutomaticResponse = new List<CatalogAutomaticResponseDto>();
        public List<SelectedItem> listDistricts = new List<SelectedItem>();
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        //[NotNull]
        //public ImagePreviewer? ImagePreviewer { get; set; }

        //public Task ShowImagePreviewer() => ImagePreviewer.Show();


        #region aprove

        public List<SelectedItem> listDefaultResponseResolve = new List<SelectedItem>();
        [NotNull]
        public Modal? ModalAprove { get; set; }
        public SelectedItem selectedItem { get; set; } = new SelectedItem();

        public Task OnShownCallbackModalLoadAsync()
        {
            _spinnerService.Hide();
            PrincipalObject.ResolutionReason = "";
            return Task.CompletedTask;
        }
        protected async Task ActionModalAssignComponentFather()
        {
            PrincipalObject.ResolutionReason = "";
            ModalAprove.Toggle();
        }
        protected async Task ActionAproveComplete()
        {


            await ModalAprove.Toggle();

            SweetAlertResult result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Acción",
                Text = "Solicitud aprobada con éxito",
                Icon = SweetAlertIcon.Success,
                AllowEscapeKey = false,
                ConfirmButtonText = "Ok",
                ConfirmButtonColor = "#0755A3",
            });

            if (result != null)
            {

                await goToList();
            }



        }

        #endregion
        #region denied

        public List<SelectedItem> listDefaultResponseDenied = new List<SelectedItem>();
        [NotNull]
        public Modal? ModalDenied { get; set; }
        protected async Task ActionModalDeniedComponentFather()
        {
            PrincipalObject.ResolutionReason = "";
            ModalDenied.Toggle();
        }


        protected async Task ActionDeniedComplete()
        {

            await ModalDenied.Toggle();

            SweetAlertResult result = await _sweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Acción",
                Text = "Solicitud denegada con éxito",
                Icon = SweetAlertIcon.Success,
                AllowEscapeKey = false,
                ConfirmButtonText = "Ok",
                ConfirmButtonColor = "#0755A3",
            });

            if (result != null)
            {

                await goToList();
            }

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



        public IEnumerable<SelectedItem> itemsDepartmentsFilters { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };
        #endregion

        protected async override Task OnInitializedAsync()
        {

            _spinnerService.Show();
            ModelFirst ??= new();
            UserCreator ??= new();

            var isValid = await _validationRouteService.HasAccessRoute(validScopes);

            if (!isValid)
            {
                await _toastService.Error("Ha ocurrido un error", "Acceso no autorizado, contacta con un administrador, por favor", autoHide: true);
                _navigation.NavigateTo("/login");
                _spinnerService.Hide();
                return;
            }


            var response = await _managementService.GetManagementById(managementId);
            if (response != null && response.response != null && response.response.Success)
            {

                var listDistrictsItems = await _districtService.GetDistricts();

                foreach (var item in listDistrictsItems)
                {
                    listDistricts.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });


                }
                listDistricts.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));


                var fullManagementDto = response.definition.ToJson().FromJson<FullComplaitDto>();

                PrincipalObject = fullManagementDto;

                ModelFirst = fullManagementDto;

                if (ModelFirst.AttachedDocuments != null && ModelFirst.AttachedDocuments.Count > 0)
                {
                    foreach (var document in ModelFirst.AttachedDocuments)
                    {
                        listImagesSelected.Add(document.FilePath);
                    }
                }

                if (fullManagementDto.CreatedUserId.HasValue)
                {
                    var responseUser = await _userService.GetUserProfile(fullManagementDto.CreatedUserId.Value);
                    if (responseUser != null)
                    {
                        UserCreator = responseUser;
                    }


                }


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
                _spinnerService.Hide();

            }
            else
            {

                _spinnerService.Hide();
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                goToList();
            }


            CatalogInputCollectionDto catalogInputCollectionDto = new CatalogInputCollectionDto()
            {
                Collections = ["PRIORITY", "STATUS-MANAGEMENT", "SUBPRINCIPALTYPE-APPLICATION-COMPLAINT", "POSITION-WORK-TASK",]
            };

            var listAllDataCatalog = await _catalogService.GetCatalogByFilters(catalogInputCollectionDto);
            listCatalogData = listAllDataCatalog;



            ////
            var listStatus = listCatalogData.Where(x => x.Collection == "STATUS-MANAGEMENT");

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
            var listSubtype = listCatalogData.Where(x => x.Collection == "SUBPRINCIPALTYPE-APPLICATION-COMPLAINT");
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
            _spinnerService.Hide();
        }



        #region goTo
        public async Task goToList()
        {
            _navigation.NavigateTo("/gestionCiudadana/denuncias");
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


        public async Task openResolveModal(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;


            selectedItem = new SelectedItem();

            var listFilter = listCatalogAutomaticResponse.Where(x => x.Code == STATUSMANAGEMENT.DONE);

            var listDataResolve = new List<SelectedItem>();

            listDataResolve.Add(new SelectedItem()
            {
                Text = "Mensaje nuevo",
                Value = "",
                GroupName = ""
            });

            foreach (var item in listFilter)
            {
                listDataResolve.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Id.ToString()
                });
            }
            listDefaultResponseResolve = listDataResolve;
            ModalAprove.Toggle();
        }


        public async Task openDeniedModal(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;


            selectedItem = new SelectedItem();

            var listFilter = listCatalogAutomaticResponse.Where(x => x.Code == STATUSMANAGEMENT.DONTAPPLY);

            var listDataResolve = new List<SelectedItem>();

            listDataResolve.Add(new SelectedItem()
            {
                Text = "Mensaje nuevo",
                Value = "",
                GroupName = ""
            });

            foreach (var item in listFilter)
            {
                listDataResolve.Add(new SelectedItem()
                {
                    Text = item.DisplayLabel,
                    Value = item.Id.ToString()
                });
            }
            listDefaultResponseDenied = listDataResolve;
            ModalDenied.Toggle();

        }

        #region secondPage

        public Task OnItemChanged(DateTime? item)
        {
            if (item != null)
            {
                ModelFirst.StartDateApplication = item.Value;
                var featureDates = item.Value.AddDays(12);
                ModelFirst.DueRateApplication = featureDates;
            }
            StateHasChanged();
            return Task.CompletedTask;
        }


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

        public async Task completeDataToWorkTask(EditContext formContext)
        {
            _spinnerService.Show();
            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;



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

                var handleManangement = PrincipalObject.ToJson().FromJson<AssingManagementTaskDto>();
                handleManangement.ManagementId = PrincipalObject.Id;


                var listUserToAssing = new List<WorkTaskUsersAssignedInputDto>();



                foreach (var item in SelectedUsersToSave)
                {
                    var itemAssingWorkTask = item.ToJson().FromJson<WorkTaskUsersAssignedInputDto>();
                    itemAssingWorkTask.UserPositionTask = item.Position;
                    listUserToAssing.Add(itemAssingWorkTask);
                }

                handleManangement.UserDepartmentList = listUserToAssing;

                var response = await _managementService.AssignCreateTaskWithManagement(handleManangement);
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

        #region table

        [NotNull]
        public Table<ManagementAttachedDocumentDto>? TableDocuments { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<ManagementAttachedDocumentDto>> OnQueryDocumentsAsync(QueryPageOptions options)
        {
            IEnumerable<ManagementAttachedDocumentDto> items = new List<ManagementAttachedDocumentDto>();
            if (ModelFirst.AttachedDocuments != null)
            {
                items = ModelFirst.AttachedDocuments;

                return new QueryData<ManagementAttachedDocumentDto>()
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
                return new QueryData<ManagementAttachedDocumentDto>()
                {
                    Items = items,
                    TotalCount = 0,
                    IsSorted = true,
                    IsFiltered = true,
                    IsSearch = true
                };

            }





        }

        #endregion
    }
}
