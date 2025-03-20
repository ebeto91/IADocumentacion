using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using RAS823_MC_CiudadMunicipal_FrontEnd.Services.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Pages.WorkTasks.Edit.Tabs.Attachments
{
    public class EditWorkTasksAttachmentsBase : ComponentBase
    {

        [Inject]
        public IWorkTaskService _workTaskService { get; set; }

        [NotNull]
        public SkeletonEditor skeletonEditor {  get; set; }

        public string MyProperty { get; set; }

        private bool _isActive;  // the name field
        [Parameter]
        public bool IsActive    // the Name property
        {
            get => _isActive;
            set
            {
                _isActive = value;
                UpdateData();
            }
        }
        public async Task UpdateData()
        {
            
            if (_isActive == true)
            {
                skeletonEditor.Active = true;
                MyProperty = _isActive.ToString() + " " + @DateTime.Now.Ticks.ToString();
            }
        }
    }
}
