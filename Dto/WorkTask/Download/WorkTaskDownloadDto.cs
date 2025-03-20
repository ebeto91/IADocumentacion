using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.AssociationDistrict;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.General;
using RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.WorkTaskKanban;
using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.WorkTask.Download
{
    #region input
    public class WorkTaskDownloadDto
    {
        public Guid? Id { get; set; }
        public string? Title { get; set; } // lo que coloca en la descripcion el usuario

        [StringLength(300)]
        public string? AditionalInformation { get; set; } // información adicional por si se requiere
        public string? PrincipalTypeApplication { get; set; } // tipo de la solicitud si es denuncia u otra 

        public string? TypeApplication { get; set; } // tipo de la solicitud una vez ya clasificada por el departamento encargado

        public string? TypeCreation { get; set; } // permite saber si fue interna o externa la solicitud

        public string? Status { get; set; } // estado de la solicitud

        public string? PrincipalNumber { get; set; } // numero de la solicitud

        public string? ResolutionNumber { get; set; } // numero de la resolución interna

        public string? OfficeCodeNumber { get; set; } // numero de oficio
        public bool? IsVisible { get; set; } // tarea visible


        public string? Priority { get; set; } // Prioridad que va a tener

        public DateTime? StartDateApplication { get; set; } //Inicio de aplicacion
        public DateTime? DueRateApplication { get; set; } //Fecha Finalización
    }
    #endregion

    #region response definition


    public class DownloadWorktaskResponseDefinition
    {
        public ResultModel response { get; set; }
        public DownloadWorktaskDefinition definition { get; set; }
    }

    public class DownloadWorktaskDefinition
    {
        public int totalCount { get; set; }
        public List<WorkTaskResponse> items { get; set; }
    }


    #endregion

}
