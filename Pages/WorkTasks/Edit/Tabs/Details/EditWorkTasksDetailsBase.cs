using BlazorSpinner;
using BootstrapBlazor.Components;
using CurrieTechnologies.Razor.SweetAlert2;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2019.Excel.RichData2;
using DocumentFormat.OpenXml.Spreadsheet;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using NPOI.HSSF.Record;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication;
using RAS823_MC_CiudadMunicipal_FrontEnd.Authentication.CustomUser;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CitizenManagment;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Department;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.District;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Users;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ListManagement;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.Profile.MyUserProfile.ListWorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.WorkTasks.Edit.Tabs.Details.Comments;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static System.Net.WebRequestMethods;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.WorkTasks.Edit.Tabs.Details
{
    public class EditWorkTasksDetailsBase : ComponentBase
    {

        [Inject]
        public IWorkTaskService _workTaskService { get; set; }

        [Inject]
        public IUserService _userService { get; set; }

        [Parameter]
        public WorkTaskInputEditTrackableDto workTaskResponseDetail { get; set; }


        [NotNull]
        public WorkTaskResponseDetail workTaskResponseDetailConsult { get; set; }

        [Parameter]
        [NotNull]
        public List<Catalog> listCatalogData { get; set; }
        [Parameter]
        public List<DepartmentResponse> departmentListData { get; set; }
        //[Parameter]
        //public List<UserDepartmentDto> userForListWithDepartment { get; set; }

        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }
        [Inject]
        public NavigationManager _navigationManager { get; set; }

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


        public List<string> listDepartmentsName = new List<string>();

        public List<string> listImagesSelected = new List<string>();
        public SelectedItem selectedItem { get; set; }

        List<DistrictNeighborhoodsDefinition> listDistrictNeighborhoodsDefinition = new List<DistrictNeighborhoodsDefinition>();

        [NotNull]
        public Modal? ModalMap { get; set; }


        #region resolution message modal
        [NotNull]
        public Modal? ModalReasonFinal { get; set; }

        List<CatalogAutomaticResponseDto> listAutomaticResponse = new List<CatalogAutomaticResponseDto>();
        public List<SelectedItem> listAutomaticResponseSelect = new List<SelectedItem>();

        public async Task OnItemChanged(SelectedItem item)
        {
            if (item != null)
            {
                if (item.Value != "")
                {
                    var guid = Guid.Parse(item.Value);
                    var itemAutomaticResponse = listAutomaticResponse.FirstOrDefault(x => x.Id == guid);
                    if (itemAutomaticResponse != null)
                    {
                        workTaskResponseDetail.ResolutionReason = itemAutomaticResponse.Description;
                    }


                }
                else
                {
                    workTaskResponseDetail.ResolutionReason = "";
                }
            }
            StateHasChanged();
        }
        public async Task CloseModal()
        {
            await ModalReasonFinal.Toggle();
        }

        public async Task OpenModalResolution()
        {

            if (workTaskResponseDetail.ResolutionNumber != null && !string.IsNullOrEmpty(workTaskResponseDetail.ResolutionNumber.Trim()))
            {
                _spinnerService.Show();
                var findListByCode = listAutomaticResponse.Where(x => x.Code == workTaskResponseDetail.Status);
                listAutomaticResponseSelect.Clear();
                selectedItem = new SelectedItem();
                listAutomaticResponseSelect.Add(new SelectedItem()
                {
                    Text = "Mensaje nuevo",
                    Value = ""
                });

                foreach (var item in findListByCode)
                {
                    listAutomaticResponseSelect.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Id.ToString()
                    });
                }

                StateHasChanged();
                _spinnerService.Hide();
                await ModalReasonFinal.Toggle();
            }
            else
            {
                await _toastService.Warning("Campo requerido", "Debe dar un número de resolución para poder completar la tarea correctamente", autoHide: true);
            }

        }
        #endregion

        [Parameter]
        public EventCallback ActionChild { get; set; }

        public UserProfileResponse UserProfile { get; set; }
        [NotNull]
        public EditWorkTasksDetailsComments editWorkTasksDetailComponent { get; set; }

        public bool showButtonResolution { get; set; }
        public bool userAllowToEdit { get; set; }

        public bool _isActive = false;   // the name field
        [Parameter]
        public bool IsActive    // the Name property
        {
            get => _isActive;
            set
            {

                _isActive = value;
            }
        }

        [Inject]
        private IJSRuntime _jsRuntime { get; set; }

        public string latitude = "";
        public string longitude = "";

        private IJSObjectReference? module;
        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {


                module = await _jsRuntime.InvokeAsync<IJSObjectReference>("import",
                      "./Pages/InstitutionalMemory/Edit/Details/EditInstitutionalMemoryDetails.razor.js");


                StateHasChanged();

            }

        }

        public async Task OnItemChangedStatus(SelectedItem item)
        {
            if (item != null)
            {
                var isToNotifyChangeStatus = workTaskResponseDetail.Status == STATUSMANAGEMENT.DONTAPPLY
        || workTaskResponseDetail.Status == STATUSMANAGEMENT.DONE || workTaskResponseDetail.Status == STATUSMANAGEMENT.FORTHCOMINGBUDGET;

                if (isToNotifyChangeStatus)
                {
                    showButtonResolution = true;
                }
                else
                {
                    showButtonResolution = false;
                }

            }
            StateHasChanged();
        }

        public async Task UpdateData(List<Catalog> catalogs, List<WorkTaskUsersAssignedDto> listUsers
            , List<WorkTaskUsersAssignedDto> listUserSelectableParameter, string workTaskId, WorkTaskResponseDetail workTaskResponseDetailParameter,
            List<CatalogAutomaticResponseDto> catalogAutomaticResponseDtos, List<DistrictNeighborhoodsDefinition> districtNeighborhoodsDefinitions, bool userAllowToEditParameter)
        {



            // Obtener el JWT desde localStorage
            var userTokenData = await _customAuthService.GetClaims();

            _spinnerService.Show();
            if (userTokenData != null)
            {
                var userIdItem = userTokenData.Claims.FirstOrDefault(x => x.Type == "UserId");

                var userId = "";
                if (userIdItem != null)
                {
                    userId = userIdItem.Value;
                }


                var response = await _userService.GetUserProfile(Guid.Parse(userId));
                if (response != null && response.Name != null)
                {
                    UserProfile = Utility.Clone(response);
                    var imageProfileDefault = ImageProfile.ImageDefault;
                    UserProfile.ImageProfile = !string.IsNullOrEmpty(UserProfile.ImageProfile) ? UserProfile.ImageProfile : imageProfileDefault;
                    StateHasChanged();
                }

            }

         


            listAutomaticResponse = catalogAutomaticResponseDtos;

            latitude = !string.IsNullOrEmpty(workTaskResponseDetailParameter.Latitude) ? workTaskResponseDetailParameter.Latitude : "";
            longitude = !string.IsNullOrEmpty(workTaskResponseDetailParameter.Longitude) ? workTaskResponseDetailParameter.Longitude : "";

            workTaskResponseDetailConsult = workTaskResponseDetailParameter;

            listCatalogData = catalogs;

            listAssignedUsers = listUsers;
            listSelectableUsers = listUserSelectableParameter;

            if (listCatalogData != null)
            {
                ////
                var listStatus = listCatalogData.Where(x => x.Collection == "STATUS-MANAGEMENT").ToList();

                var actualStatus = workTaskResponseDetailConsult.Status;
                var findActualDataToFilter = listStatus.FirstOrDefault(x => x.Code == actualStatus);

                if (findActualDataToFilter != null)
                {
                    var listThatApply = findActualDataToFilter.Ref3.Split(",");
                    var joinData = (from us in listStatus
                                    join ur in listThatApply on us.Code equals ur
                                    select us).ToList();

                    foreach (var item in joinData)
                    {


                        listSelectStatus.Add(new SelectedItem()
                        {
                            Text = item.DisplayLabel,
                            Value = item.Code,
                        });
                    }
                    listSelectStatus.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                    listSelectStatus.Insert(1, (new SelectedItem { Text = findActualDataToFilter.DisplayLabel, Value = findActualDataToFilter.Code }));
                }

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
                ///
                   ////
                var listWorkTaskTypeFind = listCatalogData.Where(x => x.Collection == "TYPE-WORKTASK");

                foreach (var item in listWorkTaskTypeFind)
                {
                    listWorkTaskType.Add(new SelectedItem()
                    {
                        Text = item.DisplayLabel,
                        Value = item.Code,
                    });
                }
                listWorkTaskType.Insert(0, (new SelectedItem { Text = "Seleccione una opción", Value = "" }));
                ////

                ///

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

                listDistrictNeighborhoodsDefinition = districtNeighborhoodsDefinitions;

                if (TableFirstReference != null)
                {
                    await TableFirstReference.QueryAsync();
                }

                if (TableSecondReference != null)
                {
                    await TableSecondReference.QueryAsync();
                }


                if (editWorkTasksDetailComponent != null)
                {
                    await editWorkTasksDetailComponent.UpdateData(workTaskResponseDetail, UserProfile, workTaskId);
                }
                userAllowToEdit = userAllowToEditParameter;

                if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
                {

                    await module.InvokeVoidAsync("Init", latitude, longitude);
                }

                StateHasChanged();
            }


            var isToNotifyChangeStatus = workTaskResponseDetail.Status == STATUSMANAGEMENT.NEW || workTaskResponseDetail.Status == STATUSMANAGEMENT.DONTAPPLY || workTaskResponseDetail.Status == STATUSMANAGEMENT.DONE || workTaskResponseDetail.Status == STATUSMANAGEMENT.FORTHCOMINGBUDGET;
            if (isToNotifyChangeStatus)
            {
                showButtonResolution = true;
                StateHasChanged();
            }

        }


        public Task OnItemChanged(DateTime? item)
        {
            if (item != null)
            {
                workTaskResponseDetail.StartDateApplication = item.Value;
                var featureDates = item.Value.AddDays(12);
                workTaskResponseDetail.DueRateApplication = featureDates;
            }
            StateHasChanged();
            return Task.CompletedTask;
        }


        public async Task completeCloseModal(EditContext formContext)
        {
            bool formIsValid = formContext.Validate();

            var data = formContext.GetValidationMessages();
            if (formIsValid == false)
                return;

            await ModalReasonFinal.Toggle();
            await saveGeneralData(formContext);
        }


        public async Task saveGeneralData(EditContext formContext)
        {

            bool formIsValid = formContext.Validate();
            if (formIsValid == false)
                return;

            //TODO PONER POR ACÁ QUE CUENDO ES RESOLUCIÓN , DENEGADA MUESTRE UN MODAL PARA PODER DAR UNA RAZON FINAL
            _spinnerService.Show();





            if (workTaskResponseDetail.AttachedDocuments != null)
            {
                var findNewFiles = workTaskResponseDetail.AttachedDocuments.Where(x => x.NewFile != null).Select(x => x.NewFile);

                if (findNewFiles != null && findNewFiles.Count() > 0)
                {
                    workTaskResponseDetail.AttachedNewFiles = findNewFiles.ToList();


                    var onlyFilesSaved = workTaskResponseDetail.AttachedDocuments.Where(x => x.NewFile == null);
                    workTaskResponseDetail.AttachedDocuments = onlyFilesSaved.ToList();
                }
            }

            //if (workTaskResponseDetail.StartDateApplication != null)
            //{
            //    var convertTime = workTaskResponseDetail.StartDateApplication.Value.ToString("yyyy-MM-dd");
            //    workTaskResponseDetail.StartDateApplication = DateTime.Parse(convertTime);
            //}


            //if (workTaskResponseDetail.DueRateApplication != null)
            //{
            //    var convertTime = workTaskResponseDetail.DueRateApplication.Value.ToString("yyyy-MM-dd");
            //    workTaskResponseDetail.DueRateApplication = DateTime.Parse(convertTime);
            //}
            workTaskResponseDetail.TypeWorkTask = workTaskResponseDetail.TypeWorkTaskCustom;

            var response = await _workTaskService.EditWorkTask(workTaskResponseDetail);
            if (response != null && response.response != null && response.response.Success)
            {
                await _toastService.Success("Información editada", "Tarea actualizada con éxito");
                //await ActionChild.InvokeAsync(null);
                _navigationManager.NavigateTo("/gestionCiudadana/tareas");
            }
            else
            {
                var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
            }

            _spinnerService.Hide();

        }

        #region table

        [NotNull]
        public Table<WorkTaskAttachedDocumentDto>? Table { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<WorkTaskAttachedDocumentDto>> OnQueryAsync(QueryPageOptions options)
        {
            IEnumerable<WorkTaskAttachedDocumentDto> items = new List<WorkTaskAttachedDocumentDto>();
            if (workTaskResponseDetail.AttachedDocuments != null)
            {
                items = workTaskResponseDetail.AttachedDocuments.Where(x => !x.ToDeleted);

                return new QueryData<WorkTaskAttachedDocumentDto>()
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
                return new QueryData<WorkTaskAttachedDocumentDto>()
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
                    WorkTaskAttachedDocumentDto evidenciaDto = new WorkTaskAttachedDocumentDto()
                    {
                        Id = newId,
                        FilePath = imageDataUrlLink,
                        FileName = file.Name,
                        ToDeleted = false,
                        WorkTaskId = workTaskResponseDetail.Id,
                        MimeType = format,
                        NewFile = file

                    };

                    if (workTaskResponseDetail.AttachedDocuments == null)
                    {
                        workTaskResponseDetail.AttachedDocuments = new List<WorkTaskAttachedDocumentDto>();
                    }

                    workTaskResponseDetail.AttachedDocuments.Add(evidenciaDto);
                }
            }
        }

        #region delete
        public async Task deleteInfo(TableColumnContext<WorkTaskAttachedDocumentDto, Guid> item)
        {

            _spinnerService.Show();

            var itemIndex = workTaskResponseDetail.AttachedDocuments.FindIndex(x => x.Id == item.Row.Id);
            if (itemIndex >= 0)
            {
                var itemFound = workTaskResponseDetail.AttachedDocuments[itemIndex];
                itemFound.ToDeleted = true;
                workTaskResponseDetail.AttachedDocuments[itemIndex] = itemFound;
                await _toastService.Success($"Borrado", $"Archivo borrado temporalmente", autoHide: true);
            }

            await Table.QueryAsync();
            _spinnerService.Hide();
        }

        #endregion

        #endregion


        #region users selection 
        List<WorkTaskUsersAssignedDto> listAssignedUsers = new List<WorkTaskUsersAssignedDto>();
        List<WorkTaskUsersAssignedDto> listSelectableUsers = new List<WorkTaskUsersAssignedDto>();
        #region first table
        [NotNull]
        public Table<WorkTaskUsersAssignedDto>? TableFirstReference { get; set; }
        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<WorkTaskUsersAssignedDto>> OnQueryFirstTableAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            IEnumerable<WorkTaskUsersAssignedDto> items = listSelectableUsers;

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
                var itemSelected = SelectedItemsFirstTable.FirstOrDefault(x => x.UserId == item.UserId);
                if (itemSelected != null)
                {
                    item.Selected = true;
                }
            }


            StateHasChanged();
            return new QueryData<WorkTaskUsersAssignedDto>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }

        private List<WorkTaskUsersAssignedDto> ApplyFiltersData(FilterKeyValueAction filters, IEnumerable<WorkTaskUsersAssignedDto> itemList)
        {
            List<WorkTaskUsersAssignedDto> itemListFilter = itemList.ToList();

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
                                    inputsToApplyInFilterTable.DepartmentIdString = departmentId;
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

            if (!string.IsNullOrEmpty(inputsToApplyInFilterTable.DepartmentIdString))
            {
                filterApply = filterApply.Where(x => x.DepartmentId == inputsToApplyInFilterTable.DepartmentIdString).ToList();
            }

            itemListFilter = filterApply;

            return itemListFilter;
        }

        public void CheckboxChangedFirstTable(ChangeEventArgs e, TableColumnContext<WorkTaskUsersAssignedDto, string> userResponseAssing)
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



        public string FindNamePosition(string code)
        {
            var item = listWorkTaskPosition.FirstOrDefault(x => x.Value == code);
            if (item != null)
            {
                return item.Text;
            }
            else
            {
                return "Sin asignar";
            }
        }


        #endregion


        public List<WorkTaskUsersAssignedDto>? SelectedItemsFirstTable { get; set; } = new List<WorkTaskUsersAssignedDto>();
        public List<WorkTaskUsersAssignedDto>? SelectedItemsSecondTable { get; set; } = new List<WorkTaskUsersAssignedDto>();



        //public List<WorkTaskUsersAssignedDto>? SelectedUsersToSave { get; set; } = new List<WorkTaskUsersAssignedDto>();
        public IEnumerable<SelectedItem> itemsDepartmentsFilters { get; set; }
        public IEnumerable<int> PageItems => new int[] { 20, 10, 5 };



        #region buttons

        public async Task passToFirstToSecondTable()
        {

            if (SelectedItemsFirstTable.Count > 0)
            {

                var removeItemSelected = listSelectableUsers.Where(x => !x.Selected)
                    .OrderByDescending(x => x.DepartmentName)
                    .ToList();

                listSelectableUsers = removeItemSelected;

                var listSelectedFirst = SelectedItemsFirstTable
                  .OrderByDescending(x => x.DepartmentName)
                  .ToList();
                //agrega a la primera lista
                foreach (var item in listSelectedFirst)
                {
                    item.Selected = false;
                    item.Position = "";
                    var clone = Utility.Clone(item);
                    listAssignedUsers.Add(clone);
                }

                SelectedItemsFirstTable = new List<WorkTaskUsersAssignedDto>();

                await TableSecondReference.QueryAsync();
                await TableFirstReference.QueryAsync();

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
                var removeItemSelected = listAssignedUsers.Where(x => !x.Selected)
             .OrderByDescending(x => x.DepartmentName)
             .ToList();

                listAssignedUsers = removeItemSelected;

                var listSelectedFirst = SelectedItemsSecondTable
                  .OrderByDescending(x => x.DepartmentName)
                  .ToList();

                //agrega a la primera lista
                foreach (var item in listSelectedFirst)
                {
                    item.Selected = false;
                    item.Position = "";
                    var clone = Utility.Clone(item);
                    listSelectableUsers.Add(clone);
                }

                listSelectableUsers =
                    [.. listSelectableUsers.OrderByDescending(x => x.DepartmentName)];


                SelectedItemsSecondTable = new List<WorkTaskUsersAssignedDto>();

                await TableSecondReference.QueryAsync();
                await TableFirstReference.QueryAsync();

                StateHasChanged();
            }
            else
            {
                await _toastService.Warning("Información", "Selecciona al menos un usuario de la lista de colaboradores asignados para devolverlos", autoHide: true);
            }

        }


        #endregion


        #region second table
        [NotNull]
        public Table<WorkTaskUsersAssignedDto>? TableSecondReference { get; set; }


        /// <summary>
        /// Se encarga de cargar los items basados hasta que ya tenga data
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<QueryData<WorkTaskUsersAssignedDto>> OnQuerySecondTableAsync(QueryPageOptions options)
        {

            _spinnerService.Show();

            IEnumerable<WorkTaskUsersAssignedDto> items = listAssignedUsers;

            var filters = options.ToFilter();

            if (filters != null && filters.Filters.Count > 0)
            {
                items = ApplyFiltersDataSecondTable(filters, items);
            }


            var total = items.Count();
            items = items.Skip((options.PageIndex - 1) * options.PageItems).Take(options.PageItems).ToList();

            _spinnerService.Hide();
            foreach (var item in items)
            {
                var itemSelected = SelectedItemsSecondTable.FirstOrDefault(x => x.UserId == item.UserId);
                if (itemSelected != null)
                {
                    item.Selected = true;
                }
            }

            listDepartmentsName = new List<string>();
            listDepartmentsName = listAssignedUsers.Select(x => x.DepartmentName).Distinct().ToList();

            StateHasChanged();
            return new QueryData<WorkTaskUsersAssignedDto>()
            {
                Items = items,
                TotalCount = total,
                IsSorted = true,
                IsFiltered = true,
                IsSearch = true
            };

        }

        private List<WorkTaskUsersAssignedDto> ApplyFiltersDataSecondTable(FilterKeyValueAction filters, IEnumerable<WorkTaskUsersAssignedDto> itemList)
        {
            List<WorkTaskUsersAssignedDto> itemListFilter = itemList.ToList();


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
                            case "DepartmentId":
                                var departmentId = itemDataFilter.FieldValue?.ToString()?.Trim();

                                if (!string.IsNullOrEmpty(departmentId))
                                {

                                    var filterApply = itemListFilter.Where(x => x.DepartmentId == departmentId)
                                        .OrderByDescending(x => x.DepartmentName)
                                        .ToList();


                                    itemListFilter = filterApply;

                                }

                                break;
                            default:
                                break;
                        }

                    }
                }

            }
            return itemListFilter;
        }

        public void CheckboxChangedSecondTable(ChangeEventArgs e, TableColumnContext<WorkTaskUsersAssignedDto, string> userResponseAssing)
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


        public async Task saveDataUserDepartments()
        {
            StateHasChanged();
            if (listAssignedUsers.Count > 0)
            {
                var listInvalidUsers = listAssignedUsers.Where(x => string.IsNullOrEmpty(x.UserPositionTask)).ToList();

                if (listInvalidUsers.Count > 0)
                {
                    await _toastService.Error("Información", "Los puestos de la tarea son requeridos, " +
                        "por favor colocar a cada uno de los colaboradores", autoHide: true);
                    _spinnerService.Hide();
                    return;
                }

                var listUserPrincipal = listAssignedUsers
                 .Select(x => x.DepartmentId)
                 .Distinct().ToList();


                var listInChargeUsers = listAssignedUsers
                    .Where(x => x.UserPositionTask == USERPOSITIONTASK.INCHARGE)
                    .Select(x => x.DepartmentId).Distinct().ToList();

                if (listUserPrincipal.Count != listInChargeUsers.Count)
                {
                    await _toastService.Error("Información", "Para cada departamento asignado debe colocar al menos un usuario encargado de la tarea", autoHide: true);
                    _spinnerService.Hide();
                    return;
                }



                HandleChangeUsersAssingWorkTask handleChangeUsersAssing = new HandleChangeUsersAssingWorkTask()
                {
                    WorkTaskId = workTaskResponseDetailConsult.Id,
                };
                //usuarios que han cambiado para no estar
                var findUserThatAreChanged = (from us in workTaskResponseDetailConsult.ListAssignedUsers
                                              join ur in listSelectableUsers on us.UserId equals ur.UserId
                                              select ur).ToList();
                foreach (var item in findUserThatAreChanged)
                {
                    item.Enabled = false;
                }

                foreach (var item in listAssignedUsers)
                {
                    if (item.IsUserBeforeAssigned)
                    {
                        item.Enabled = true;
                    }
                }

                listAssignedUsers.AddRange(findUserThatAreChanged);


                var groupDepartment = listAssignedUsers.GroupBy(x => x.DepartmentId);
                List<HandleChangeUsersDepartmentAssingWorkTask> handle = new List<HandleChangeUsersDepartmentAssingWorkTask>();
                foreach (var item in groupDepartment)
                {
                    handle.Add(new HandleChangeUsersDepartmentAssingWorkTask()
                    {
                        DepartmentId = item.Key,
                        ListAssingUsers = item.ToList()
                    });
                }
                handleChangeUsersAssing.DepartmentList = handle;



                var response = await _workTaskService.ChangeUserAssignedToWorkTask(handleChangeUsersAssing);
                if (response != null && response.response != null && response.response.Success)
                {
                    await _toastService.Success("Información editada", "Usuarios actualizados con éxito");
                    //await ActionChild.InvokeAsync(null);
                    _navigationManager.NavigateTo("/gestionCiudadana/tareas");
                }
                else
                {
                    var message = response != null && response.response != null ? response.response.Message : "Ha ocurrido un error, inténtalo de nuevo por favor";
                    await _toastService.Error("Ha ocurrido un error", message, autoHide: true);
                }


            }
            else
            {
                await _toastService.Warning("Información", "Selecciona al menos un usuario de la lista de colaboradores", autoHide: true);
            }
        }
        #endregion


        #region map
        public async Task OpenModal()
        {

            await ModalMap.Toggle();
            await Task.Delay(100);
            StateHasChanged();
        }

        #endregion

        #region district
        public string findNameDistrict(string code)
        {
            var item = listDistrictNeighborhoodsDefinition.FirstOrDefault(x => x.Code == code);
            if (item != null)
            {
                return item.DisplayLabel;
            }
            else
            {
                return "Sin distrito";
            }
        }
        public string findNameNeighborhood(string codeDistrict, string codeNeighborhood)
        {
            var item = listDistrictNeighborhoodsDefinition.FirstOrDefault(x => x.Code == codeDistrict);
            if (item != null)
            {
                var itemNeighborhood = item.NeighborhoodList.FirstOrDefault(x => x.Code == codeNeighborhood);
                if (itemNeighborhood != null)
                {
                    return itemNeighborhood.DisplayLabel;
                }
                else
                {
                    return "Sin barrio";
                }



            }
            else
            {
                return "Sin barrio";
            }
        }
        #endregion
    }
}
