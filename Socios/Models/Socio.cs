﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Socios.Models
{
    public class Socio
    {

        public int IdSocio { get; set; }

        public string Nombre { get; set; }

        public int Edad { get; set; }

        public string Equipo { get; set; }

        public string EstadoCivil { get; set; }

        public string NivelEstudios { get; set; }

    }
}