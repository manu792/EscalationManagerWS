using EscalationManagerWS.Data.Models;
using EscalationManagerWS.Data.Setup;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationManagerWS.Data
{
    public class Repository
    {
        private CNMContext context;
        private int daysBack;

        public Repository()
        {
            context = new CNMContext();
            daysBack = Convert.ToInt32(ConfigurationManager.AppSettings["Threshold"]);
        }

        public void EscalateToDirector()
        {
            try
            {
                var date = DateTime.Now.AddDays(daysBack);
                var estado = context.Estados.FirstOrDefault(x => x.Nombre.Equals("por revisar", StringComparison.OrdinalIgnoreCase));
                var role = context.Roles.FirstOrDefault(x => x.Name.Equals("Jefatura", StringComparison.OrdinalIgnoreCase));
                var categoriaDG = context.Categorias.FirstOrDefault(x => x.Nombre.Equals("director general", StringComparison.OrdinalIgnoreCase));
                var directorGeneral = context.Empleados.FirstOrDefault(x => x.CategoriaId == categoriaDG.CategoriaId);

                var solicitudes = context.SolicitudesVacaciones.Where(x => x.UltimaActualizacion <= date &&
                x.EstadoId == estado.EstadoId &&
                context.Empleados.FirstOrDefault(e => e.EmpleadoId == x.AprobadorId).Roles.Any(r => r.RoleId == role.Id))
                .ToList();

                foreach (var solicitud in solicitudes)
                {
                    solicitud.AprobadorId = directorGeneral.EmpleadoId;
                    solicitud.UltimaActualizacion = DateTime.Now;
                    context.SolicitudesVacaciones.Add(solicitud);
                    context.Entry(solicitud).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                WriteLog("Ejecucion exitosa");
            }
            catch(Exception ex)
            {
                Console.Write(ex);
                WriteLog(ex.Message);
            }
            
        }
        public IEnumerable<SolicitudVacaciones> EscalateToRejected()
        {
            try
            {
                var date = DateTime.Now.AddDays(daysBack);
                var estado = context.Estados.FirstOrDefault(x => x.Nombre.Equals("por revisar", StringComparison.OrdinalIgnoreCase));
                var estadoRechazado = context.Estados.FirstOrDefault(x => x.Nombre.Equals("rechazado", StringComparison.OrdinalIgnoreCase));
                var role = context.Roles.FirstOrDefault(x => x.Name.Equals("director", StringComparison.OrdinalIgnoreCase));
                var categoriaDG = context.Categorias.FirstOrDefault(x => x.Nombre.Equals("director general", StringComparison.OrdinalIgnoreCase));
                var directorGeneral = context.Empleados.FirstOrDefault(x => x.CategoriaId == categoriaDG.CategoriaId);

                var solicitudes = context.SolicitudesVacaciones.Where(x => x.UltimaActualizacion <= date &&
                x.EstadoId == estado.EstadoId &&
                context.Empleados.FirstOrDefault(e => e.EmpleadoId == x.AprobadorId).Roles.Any(r => r.RoleId == role.Id))
                .ToList();

                foreach (var solicitud in solicitudes)
                {
                    solicitud.EstadoId = estadoRechazado.EstadoId;
                    solicitud.UltimaActualizacion = DateTime.Now;
                    context.SolicitudesVacaciones.Add(solicitud);
                    context.Entry(solicitud).State = System.Data.Entity.EntityState.Modified;
                }

                context.SaveChanges();
                WriteLog("Ejecucion exitosa");

                return solicitudes;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                WriteLog(ex.Message);
                throw;
            }

        }

        public Empleado GetBoss(Empleado solicitante)
        {
            var role = context.Roles.FirstOrDefault(x => x.Name.Equals("jefatura", StringComparison.OrdinalIgnoreCase));

            if (solicitante.Roles.Any(x => x.RoleId == role.Id))
                return GetBoss();

            return context.Empleados.FirstOrDefault(x => x.UnidadTecnicaId == solicitante.UnidadTecnicaId && x.Roles.Any(r => r.RoleId == role.Id));
        }

        public Empleado GetBoss()
        {
            var categoria = context.Categorias.FirstOrDefault(x => x.Nombre.Equals("director general", StringComparison.OrdinalIgnoreCase));

            return context.Empleados.FirstOrDefault(x => x.CategoriaId == categoria.CategoriaId);
        }

        private void WriteLog(string ex)
        {
            string exeFolder = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(exeFolder + "/Log.txt", true))
            {
                file.WriteLine(ex);
            }
        }
    }
}
