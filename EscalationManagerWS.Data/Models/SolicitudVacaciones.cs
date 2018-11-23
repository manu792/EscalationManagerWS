using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationManagerWS.Data.Models
{
    [Table("SolicitudesVacaciones")]
    public class SolicitudVacaciones
    {
        public int SolicitudVacacionesId { get; set; }
        [Required]
        [Column("Cedula")]
        public string EmpleadoId { get; set; }
        public int CantidadDiasSolicitados { get; set; }
        [MaxLength(500, ErrorMessage = "El comentario debe ser máximo de 500 caracteres")]
        public string Comentario { get; set; }
        public int EstadoId { get; set; }
        public string AprobadorId { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime UltimaActualizacion { get; set; }

        // Virtual properties
        public virtual Estado Estado { get; set; }
        public virtual Empleado Empleado { get; set; }
    }
}
