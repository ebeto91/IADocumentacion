using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Management
{
    public class HandleManagementTaskDto : ManagementDto
    {
        public Guid ManagementId { get; set; }
    }

    public class AssingManagementTaskDto : ManagementDto
    {
        public Guid ManagementId { get; set; }
        public List<WorkTaskUsersAssignedInputDto> UserDepartmentList { get; set; }
    }
    public class AssingWithoutManagementTaskDto : ManagementDto
    {
        public string? TypeWorkTask { get; set; } // en caso de dividirlo en proyecto, obra menor
        public List<IBrowserFile> attachedFiles { get; set; }
        public List<WorkTaskUsersAssignedInputDto> UserDepartmentList { get; set; }
    }
    public class WorkTaskUsersAssignedInputDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        [StringLength(50)]
        public string? Lastname { get; set; }
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [StringLength(8)]
        public string? PhoneNumber { get; set; }
        public Guid DepartmentId { get; set; }
        public DateTime? LastNotificationTime { get; set; }
        public bool Enabled { get; set; }
        [StringLength(300)]
        public string UserPositionTask { get; set; } //Por si se requiere saber que usuario es el encargado en la tarea
    }
}
