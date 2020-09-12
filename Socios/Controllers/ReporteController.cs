using Socios.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Socios.Controllers
{
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Generar()
        {
            if (Request.Files.Count == 0) return HttpNotFound();

            var path = GrabarAnexo(Request.Files[0]);

            var socios = new List<Socio>();
            socios = LeerArchivo(path);

            var reporte = new ReporteViewModel();
            reporte.CantidadSocios = socios.Count();
            reporte.PromedioEdadRacing = GetEdadPromedioRacing(socios);
            reporte.NombresFrecuentesRiver = GetNombreFrecuentesRiver(socios);
            reporte.SociosCasados = GetSociosCasados(socios);
            reporte.Equipos = GetEquipos(socios);

            return View("Reporte", reporte);
        }

        private string GrabarAnexo(HttpPostedFileBase x)
        {
            string path = "~/archivos/" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string pathToSave = Server.MapPath(path);

            // Determina si existe el directorio
            if (!Directory.Exists(pathToSave))
            {
                // Si no existe, intentamos crearlo.
                DirectoryInfo di = Directory.CreateDirectory(pathToSave);
            }

            string filename = String.Format("{0:HHmmssfff}", DateTime.Now) + "_" + Path.GetFileName(x.FileName);
            filename = filename.Replace(" ", "_");
            string pathfinal = Path.Combine(pathToSave, filename);
            x.SaveAs(pathfinal);
            return pathfinal;
        }

        public List<Socio> LeerArchivo(string path)
        {
            String line;
            var socios = new List<Socio>();
            try
            {
                const Int32 BufferSize = 128;
                StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding(1252), true, BufferSize);

                line = sr.ReadLine();

                int i = 0;

                while (line != null && line != "")
                {
                    char separadorComa = ';';
                    string[] arrayDatos = line.Split(separadorComa);

                    var s = new Socio()
                    {
                        IdSocio = i,
                        Nombre = arrayDatos[0],
                        Edad = Int32.Parse(arrayDatos[1]),
                        Equipo = arrayDatos[2],
                        EstadoCivil = arrayDatos[3],
                        NivelEstudios = arrayDatos[4]
                    };
                    socios.Add(s);
                    line = sr.ReadLine();
                    i++;
                }

                sr.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return socios;
        }

        public double GetEdadPromedioRacing(List<Socio> socios)
        {
            double promedio = 0;
            var filtro = socios.Where(x => x.Equipo == "Racing").Select(x => x.Edad).ToList();
            if (filtro != null && filtro.Count != 0) promedio = filtro.Average();

            return Math.Round(promedio, 2);
        }

        public List<string> GetNombreFrecuentesRiver(List<Socio> socios)
        {
            var nombres = new List<string>();

            var queryNombre = from socio in socios
                              where socio.Equipo == "River"
                              group socio by socio.Nombre into resultado
                              orderby resultado.Count() descending
                              select resultado;

            var queryNombre5 = queryNombre.ToList().Take(5);
            foreach (var nombre in queryNombre5)
            {
                nombres.Add(nombre.Key);
            }

            return nombres;
        }

        public List<Socio> GetSociosCasados(List<Socio> socios)
        {
            var resultado = new List<Socio>();

            resultado = socios.Where(x => x.EstadoCivil == "Casado" && x.NivelEstudios == "Universitario")
                .OrderBy(x => x.Edad).Take(100).ToList();

            return resultado;
        }

        public List<Equipo> GetEquipos(List<Socio> socios)
        {
            var resultado = new List<Equipo>();

            var equipos = socios.Select(x => x.Equipo).Distinct();

            foreach (var equipo in equipos)
            {
                var e = new Equipo();
                var s = socios.Where(x => x.Equipo == equipo);
                e.EquipoNombre = equipo;
                e.CantidadSocios = s.Count();
                e.PromedioEdad = s.Select(x => x.Edad).Average();
                e.MenorEdad = s.Select(x => x.Edad).Min();
                e.MayorEdad = s.Select(x => x.Edad).Max();

                resultado.Add(e);
            }

            var resultadoOrdenado = resultado.OrderByDescending(x => x.CantidadSocios).ToList();

            return resultadoOrdenado;
        }
    }
}