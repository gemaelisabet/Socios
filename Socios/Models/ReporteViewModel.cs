using Socios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socios.Models
{
    public class ReporteViewModel
    {

        public List<Socio> SociosCasados { get; set; }

        public List<Equipo> Equipos { get; set; }

        public int CantidadSocios { get; set; }

        public double PromedioEdadRacing { get; set; }

        public List <string> NombresFrecuentesRiver { get; set; }

    }
}