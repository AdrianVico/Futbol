using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Jugador
    {
        string nombre;
        string posicion;
        int precio;
        string equipoOrigen;
        bool esCapitan;

        public Jugador(string nombre, string posicion, int precio, string equipoOrigen)
        {
            this.nombre = nombre;
            this.posicion = posicion;
            this.precio = precio;
            this.equipoOrigen = equipoOrigen;
            esCapitan = false;
        }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Posicion { get => posicion; set => posicion = value; }
        public int Precio { get => precio; set => precio = value; }
        public string EquipoOrigen { get => equipoOrigen; set => equipoOrigen = value; }
        public bool EsCapitan { get => esCapitan; set => esCapitan = value; }

        public override string ToString()
        {
            string capitan = esCapitan ? "Es Capitan" : "No es Capitan";

            return $"Nombre: {nombre}, Posición: {posicion}, Precio: {precio}, Equipo de Origen: {equipoOrigen}" +
                $", {capitan}.";
        }
    }
}
