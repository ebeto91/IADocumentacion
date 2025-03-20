using Microsoft.AspNetCore.Components.Forms;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskComments
{
    #region list response
    public class ListWorkTaskCommentResponse
    {
        public ResultModel Response { get; set; }
        public List<WorkTaskCommentsGroupResponse> Definition { get; set; }
    }

    public class WorkTaskCommentsGroupResponse
    {
        public string Type { get; set; }
        public List<WorkTaskCommentResponse> WorkTaskComments { get; set; }
    }

    public class WorkTaskCommentResponse
    {
        public Guid Id { get; set; }
        [StringLength(300)]
        public string Comment { get; set; }
        [StringLength(30)]
        public string Type { get; set; }
        public Guid UserId { get; set; }
        public Guid WorkTaskId { get; set; }
        public string? Name { get; set; }

        public string? Lastname { get; set; }
        public string EmailAddress { get; set; }
        public string? ImageProfile { get; set; }
        public string UserName { get; set; }
        public string TimeAgo { get; set; }
        public bool IsEdited { get; set; }

        public List<WorkTaskCommentAttachedDocumentDto> AttachedDocuments { get; set; }
    }
    public class WorkTaskCommentAttachedDocumentDto
    {
        public Guid Id { get; set; }

        public Guid TaskCommentId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        [StringLength(100)]
        public string MimeType { get; set; }
        public Guid WorkTaskId { get; set; }
        public bool ToDeleted { get; set; }
    }

    #endregion

    #region handle

    public class HandleWorkTaskCommentDto
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(maximumLength: 2000, ErrorMessage = "La longitud máxima permitida es de {1} caracteres.")]
        public string Comment { get; set; }
        [StringLength(30)]
        public string Type { get; set; }

        public string UserId { get; set; }
        public string WorkTaskId { get; set; }

        public List<WorkTaskCommentAttachedDocumentDto>? AttachedDocuments { get; set; }
        public List<IBrowserFile>? AttachedNewFiles { get; set; }
    }

    #endregion
}
