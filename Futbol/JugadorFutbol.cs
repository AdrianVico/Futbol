using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class JugadorFutbol
    {
        string nombre;
        string posicion;
        int dorsal;
        int coste;

        public JugadorFutbol(string nombre, string posicion, int dorsal, int coste)
        {
            this.nombre = nombre;
            this.posicion = posicion;
            this.dorsal = dorsal;
            this.coste = coste;
        }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Posicion { get => posicion; set => posicion = value; }
        public int Dorsal { get => dorsal; set => dorsal = value; }
        public int Coste { get => coste; set => coste = value; }
    }
}
