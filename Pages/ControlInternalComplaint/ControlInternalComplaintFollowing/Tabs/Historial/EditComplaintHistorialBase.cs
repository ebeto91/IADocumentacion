using BlazorSpinner;
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.CatalogDto;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskHistory;
using RAS823_MC_CiudadMunicipal_FrontEnd.Helpers;
using RAS823_MC_CiudadMunicipal_FrontEnd.Pages.WorkTasks.Edit.Tabs.Historial;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.ControlInternalComplaint.ControlInternalComplaintFollowing.Tabs.Historial
{
    public class EditComplaintHistorialBase : ComponentBase
    {

        [Inject]
        public IWorkTaskService _workTaskService { get; set; }
        [Inject]
        public IWorkTaskHistoryService _workTaskHistoryService { get; set; }
        [NotNull]
        public WorkTaskResponseDetail workTaskResponseDetailConsult { get; set; }
        [Inject]
        public SpinnerService _spinnerService { get; set; }
        [Inject]
        public ToastService _toastService { get; set; }

        public List<Catalog> listCatalogOnlyPositions = new List<Catalog>();


        #region worktask
        public List<WorkTaskHistoryResponse> listHistoryWorkTask { get; set; }
        [NotNull]
        public ComponentShowHistoryDetails componentShowHistoryDetails { get; set; }



        public Task OnListViewItemClick(WorkTaskHistoryResponse item)
        {
            //Logger.Log($"ListViewItem: {item.Description} clicked");
            componentShowHistoryDetails.updateData(item);
            return Task.CompletedTask;
        }

        #endregion



        #region assign users
        [NotNull]
        public ComponentShowHistoryUserAssignedDetails componentShowHistoryUserAssignedDetails { get; set; }
        public List<WorkTaskHistoryUserAssignedItemWithList> workTaskHistoryUserAssignedItemWithLists { get; set; }
        public Task OnListViewItemWorkTaskUserAssignedClick(WorkTaskHistoryUserAssignedItemWithList item)
        {
            componentShowHistoryUserAssignedDetails.updateData(item.ListHistory);
            return Task.CompletedTask;
        }
        #endregion


        #region documents
        [NotNull]
        public ComponentShowHistoryDocumentDetails componentShowHistoryDocumentDetails { get; set; }
        public List<WorkTaskHistoryDocumentItemWithList> workTaskHistoryDocumentItemWithLists { get; set; }
        public Task OnListViewItemDocumentClick(WorkTaskHistoryDocumentItemWithList item)
        {
            //Logger.Log($"ListViewItem: {item.Description} clicked");
            componentShowHistoryDocumentDetails.updateData(item.ListHistory);
            return Task.CompletedTask;
        }


        #endregion


        public async Task UpdateData(WorkTaskResponseDetail workTaskResponseDetailParameter, List<Catalog> catalogs)
        {
            _spinnerService.Show();
            workTaskResponseDetailConsult = workTaskResponseDetailParameter;
            listCatalogOnlyPositions = catalogs.Where(x => x.Collection == "POSITION-WORK-TASK").ToList();

            await UpdateDataLoad();
            _spinnerService.Hide();
        }

        private async Task UpdateDataLoad()
        {
            var response = await _workTaskHistoryService.GetWorkTaskHistoryDetails(workTaskResponseDetailConsult.Id);
            if (response != null && response.response != null && response.response.Success)
            {
                #region load time lapse work history


                var listDataWorkTask = response.definition.WorkTaskHistoryListResponse;

                if (listDataWorkTask.List.Count > 0)
                {
                    listHistoryWorkTask = listDataWorkTask.List;
                }

                if (response.definition.WorkTaskHistoryDocumentListResponse != null)
                {
                    var documentListHistory = response.definition.WorkTaskHistoryDocumentListResponse.List;

                    if (documentListHistory.Count > 0)
                    {
                        workTaskHistoryDocumentItemWithLists = documentListHistory;
                    }
                }

                if (response.definition.WorkTaskHistoryUserAssignedListResponse != null)
                {
                    var assignedUserList = response.definition.WorkTaskHistoryUserAssignedListResponse.List;

                    if (assignedUserList.Count > 0)
                    {
                        workTaskHistoryUserAssignedItemWithLists = assignedUserList;
                    }
                }


                StateHasChanged();

                #endregion


            }
        }
        public async Task updateButton()
        {
            _spinnerService.Show();
            await UpdateDataLoad();
            _spinnerService.Hide();
        }
        public string getUserPosition(string code)
        {
            var item = listCatalogOnlyPositions.FirstOrDefault(x => x.Code == code);
            if (item != null)
            {
                return item.DisplayLabel;
            }
            else
            {
                return "Sin asignar";
            }
        }



    }
}
