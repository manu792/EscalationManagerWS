using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationManagerWS.Data.Models
{
    [Table("Estados")]
    public class Estado
    {
        public int EstadoId { get; set; }
        public string Nombre { get; set; }
    }
}
