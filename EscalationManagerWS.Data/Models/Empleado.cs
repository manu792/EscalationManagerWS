using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationManagerWS.Data.Models
{
    [Table("Empleados")]
    public class Empleado
    {
        [Column("Cedula")]
        public string EmpleadoId { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string FotoRuta { get; set; }
        public int CategoriaId { get; set; }
        public int UnidadTecnicaId { get; set; }
        public bool EstaActivo { get; set; }
        public bool EsContrasenaTemporal { get; set; }
        public string Correo { get; set; }
        public string ContrasenaHash { get; set; }
        public string SelloSeguridad { get; set; }
        public string Telefono { get; set; }
        public string NombreUsuario { get; set; }


        // Virtual properties
        public virtual Categoria Categoria { get; set; }
        public virtual ICollection<UsuarioRoles> Roles { get; set; }
    }
}
