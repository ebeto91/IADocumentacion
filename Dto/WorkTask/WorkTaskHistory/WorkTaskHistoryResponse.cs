using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskHistory
{


    public class WorkTaskResponseHistoryDtoGeneralResponse
    {
        public ResultModel response { get; set; }
        public WorkTaskResponseHistoryDto definition { get; set; }
    }


    public class WorkTaskResponseHistoryDto
    {
        public WorkTaskHistoryListResponse WorkTaskHistoryListResponse { get; set; }
        public WorkTaskHistoryDocumentListResponse WorkTaskHistoryDocumentListResponse { get; set; }
        public WorkTaskHistoryUserAssignedListResponse WorkTaskHistoryUserAssignedListResponse { get; set; }
        public WorkTaskHistoryCommentListResponse WorkTaskHistoryCommentListResponse { get; set; }
        //public WorkTaskHistoryCommentDocumentListResponse WorkTaskHistoryCommentDocumentListResponse { get; set; }
    }

    #region historico de la tarea y documentos

    /// <summary>
    /// Item principal para saber que va a ser un tab, tiene codigo y nombre y la lista de los historicos de la tarea
    /// </summary>
    public class WorkTaskHistoryListResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<WorkTaskHistoryResponse> List { get; set; }
    }
    /// <summary>
    /// Item de lista para saber que lleva la info de los cambios en la tarea
    /// </summary>
    public class WorkTaskHistoryResponse
    {

        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAtHistory { get; set; }
        public Guid? CreatedUserIdHistory { get; set; }
        public string? CreatedUserName { get; set; }
        public string? CreatedEmailAddress { get; set; }
        public string? CreatedImageProfile { get; set; }

        public DateTime? UpdatedAtHistory { get; set; }
        public Guid? UpdatedUserIdHistory { get; set; }
    }
    #endregion

    #region historico tarea documentos
    /// <summary>
    /// Item principal para saber que va a ser un tab, tiene codigo y nombre y la lista del historico de los documentos de la tarea
    /// </summary>
    public class WorkTaskHistoryDocumentListResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<WorkTaskHistoryDocumentItemWithList> List { get; set; }
    }
    /// <summary>
    /// Item de lista para saber que permite saber como se llama el documento y tiene la lista de historicos de ese documento
    /// </summary>
    public class WorkTaskHistoryDocumentItemWithList
    {
        public string ActualFileName { get; set; }
        public List<WorkTaskHistoryAttachedDocumentResponse> ListHistory { get; set; }
    }
    /// <summary>
    /// Item que tiene los cambios de un documento
    /// </summary>
    public class WorkTaskHistoryAttachedDocumentResponse
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAtHistory { get; set; }
        public Guid? CreatedUserIdHistory { get; set; }
        public string? CreatedUserName { get; set; }
        public string? CreatedEmailAddress { get; set; }
        public string CreatedImageProfile { get; set; }

        public DateTime? UpdatedAtHistory { get; set; }
        public Guid? UpdatedUserIdHistory { get; set; }
    }

    #endregion

    #region historico de los usuarios asignados
    /// <summary>
    /// Item principal para saber que va a ser un tab, tiene codigo y nombre y la lista del historico de los usuarios asignados
    /// </summary>
    public class WorkTaskHistoryUserAssignedListResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<WorkTaskHistoryUserAssignedItemWithList> List { get; set; }
    }

    /// <summary>
    /// Item que me indica los cambios que hubo 
    /// </summary>
    public class WorkTaskHistoryUserAssignedItemWithList
    {
        //public string ActualEmailAddress { get; set; }
        public string EmailAddress { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserPositionTask { get; set; }
        public List<WorkTaskHistoryUserAssignedResponse> ListHistory { get; set; }
    }

    public class WorkTaskHistoryUserAssignedResponse
    {
        //public string Name { get; set; }
        //public string Lastname { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAtHistory { get; set; }
        public Guid? CreatedUserIdHistory { get; set; }
        public string? CreatedUserName { get; set; }
        public string? CreatedEmailAddress { get; set; }
        public string CreatedImageProfile { get; set; }

        public DateTime? UpdatedAtHistory { get; set; }
        public Guid? UpdatedUserIdHistory { get; set; }
    }
    #endregion

    #region historico comentarios
    //Listado general
    public class WorkTaskHistoryCommentListResponse
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public List<WorkTaskHistoryCommentItemWithList> List { get; set; }
    }
    //Item comentario con el historico dentro y los archivos??
    public class WorkTaskHistoryCommentItemWithList
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public List<WorkTaskHistoryCommentResponse> ListHistoryComment { get; set; }
        public List<WorkTaskHistoryCommentDocumentListResponse> ListHistoryDocumentComment { get; set; }
    }

    public class WorkTaskHistoryCommentResponse
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string ImageProfile { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }

        public DateTime CreatedAtHistory { get; set; }
        public Guid? CreatedUserIdHistory { get; set; }
        public string? CreatedUserName { get; set; }
        public string? CreatedEmailAddress { get; set; }
        public string CreatedImageProfile { get; set; }

        public DateTime? UpdatedAtHistory { get; set; }
        public Guid? UpdatedUserIdHistory { get; set; }

    }

    #endregion

    #region historico documentos comentarios
    public class WorkTaskHistoryCommentDocumentListResponse
    {
        public string FileName { get; set; }
        public List<WorkTaskHistoryAttachedDocumentCommentResponse> ListHistory { get; set; }
    }

    public class WorkTaskHistoryAttachedDocumentCommentResponse
    {
        public string Action { get; set; }
        public string Details { get; set; }

        public DateTime CreatedAtHistory { get; set; }
        public Guid? CreatedUserIdHistory { get; set; }
        public string? CreatedUserName { get; set; }
        public string? CreatedEmailAddress { get; set; }
        public string CreatedImageProfile { get; set; }

        public DateTime? UpdatedAtHistory { get; set; }
        public Guid? UpdatedUserIdHistory { get; set; }
    }
    #endregion
}
