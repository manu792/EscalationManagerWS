using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationManagerWS.Data.Models
{
    public class UsuarioRoles
    {
        [Key]
        [Column("UserId", Order = 0)]
        public string EmpleadoId { get; set; }
        [Key]
        [Column(Order = 1)]
        public string RoleId { get; set; }
    }
}
