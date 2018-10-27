using EscalationManagerWS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationManagerWS.Logic
{
    public class EscalationManager
    {
        private Repository repository;
        private EmailNotificationService emailService;

        public EscalationManager()
        {
            repository = new Repository();
            emailService = new EmailNotificationService();
        }

        public void EscalateIfNeeded()
        {
            repository.EscalateToDirector();
            var solicitudes = repository.EscalateToRejected();
            var directorGeneral = repository.GetBoss();

            foreach (var solicitud in solicitudes)
            {
                var jefe = repository.GetBoss(solicitud.Empleado);

                emailService.SendEmailAsync(solicitud.Empleado.Correo, (jefe != null ? jefe.Correo : directorGeneral.Correo), "Solicitud de Vacaciones Rechazada", $"Se le informa que la solicitud de vacaciones, con Id {solicitud.SolicitudVacacionesId}, para el colaborador(a) {solicitud.Empleado.Nombre} {solicitud.Empleado.SegundoApellido} {solicitud.Empleado.SegundoApellido} ha sido rechazada de forma automática debido a que no ha sido procesada por la persona encargada, ni por el Director General durante el período límite. Contacte a su encargado para más información.");
            }
        }
    }
}
