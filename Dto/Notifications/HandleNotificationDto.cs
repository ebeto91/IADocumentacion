using System.ComponentModel.DataAnnotations;

namespace RAS823_MC_CiudadMunicipal_FrontEnd.Dto.Notifications
{
    public class HandleNotificationDto
    {
        public string? Id { get; set; }
        //public Guid? ExternalId { get; set; }
        //public Guid? FromUserId { get; set; }
        public string? ToUserId { get; set; }
        [StringLength(800)]
        public string Text { get; set; }
        [StringLength(500)]
        public string LinkUrl { get; set; }
        // Tipo Mensaje Interno ó Notificación
        [StringLength(50)]
        public string MessageType { get; set; }
        [StringLength(50)]
        public string? Status { get; set; }
        public bool MarkAsRead { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? TimeAgo { get; set; }
    }

    public class HandleNotificationMarkAsRead
    {
        public string? Id { get; set; }
        public bool MarkAsRead { get; set; }
        public string? ToUserId { get; set; }
    }
}
