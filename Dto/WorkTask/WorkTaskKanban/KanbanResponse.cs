using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskKanban
{
    #region input

    public class WorkTaskKanbanInputDto
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario


        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia u otra 

        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado

        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud

        public string? Status { get; set; } // estado de la solicitud

        public string? PrincipalNumber { get; set; } // numero de la solicitud
        public string? UserId { get; set; } // id de usuario

        public string? ResolutionNumber { get; set; } // numero de la resolución interna

        public string? OfficeCodeNumber { get; set; } // numero de oficio


        public string? Priority { get; set; } // Prioridad que va a tener
        public bool? IsVisible { get; set; } // visibiblidad

        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización
    }

    #endregion

   public class WorkTaskKanbanResponseDefinition
    {
        public ResultModel response { get; set; }
        public KanbanResponseDefinition definition { get; set; }
    }
    public class KanbanResponseDefinition
    {
        public List<HeaderKey> ListHeaders { get; set; }
        public List<WorkTaskForKanban> ListWorkTaskGroup { get; set; }
    }
    public class HeaderKey
    {
        public string Code { get; set; }
        public string DisplayLabel { get; set; }
    }


    public class WorkTaskForKanban
    {
        public string Code { get; set; }
        public string DisplayLabel { get; set; }
        public List<WorkTaskResponse> ListWorkTask { get; set; }
    }
}
