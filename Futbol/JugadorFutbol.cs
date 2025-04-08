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
        int coste;

        public JugadorFutbol(string nombre, string posicion, int coste)
        {
            this.nombre = nombre;
            this.posicion = posicion;
            this.coste = coste;
        }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Posicion { get => posicion; set => posicion = value; }
        public int Coste { get => coste; set => coste = value; }

        public override string ToString()
        {
            return $"Nombre: {nombre}, Posición: {posicion}, Coste: {coste}";
        }
    }
}
