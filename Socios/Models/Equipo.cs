using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socios.Models
{
    public class Equipo
    {

        public int IdEquipo { get; set; }

        public string EquipoNombre { get; set; }

        public double PromedioEdad { get; set; }

        public int CantidadSocios { get; set; }

        public int MenorEdad { get; set; }

        public int MayorEdad { get; set; }

    }
}