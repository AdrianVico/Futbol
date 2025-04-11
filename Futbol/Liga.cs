using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Liga
    {
        string nombre;
        List<Equipo> equipos;

        public Liga(string nombre)
        {
            this.nombre = nombre;
            equipos = new List<Equipo>();
        }
        public string Nombre { get => nombre; set => nombre = value; }
        public void AddEquipo(Equipo equipo)
        {
            equipos.Add(equipo);
        }
        public void MostrarEquipos()
        {
            Console.WriteLine($"Equipos de la liga {nombre}:");
            foreach (Equipo equipo in equipos)
            {
                Console.WriteLine(equipo.Nombre);
            }
        }

        public override string ToString()
        {
            string equiposString = string.Join(", ", equipos.Select(e => e.Nombre));
            
            return $"Nombre: {nombre}, Equipos: {equiposString}";
        }
    }
}
