using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Liga
    {
        string nombreLiga;
        List<Equipo> equipos;
        public Liga(string nombreLiga)
        {
            this.nombreLiga = nombreLiga;
            equipos = new List<Equipo>();
        }
        public string Nombre { get => nombreLiga; set => nombreLiga = value; }
        public void AddEquipo(Equipo equipo)
        {
            equipos.Add(equipo);
        }
        public void MostrarEquipos()
        {
            Console.WriteLine($"Equipos de la liga {nombreLiga}:");
            foreach (Equipo equipo in equipos)
            {
                Console.WriteLine(equipo.Nombre);
            }
        }

        public void MostrarClasificacion()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.WriteLine(nombreLiga);
            Console.WriteLine();
            foreach (Equipo equipo in equipos)
            {
                Console.WriteLine($"{equipo}");
            }
        }

        public override string ToString()
        {
            string equiposString = string.Join(", ", equipos.Select(e => e.Nombre));
            
            return $"Nombre: {nombreLiga}, Equipos: {equiposString}";
        }
    }
}
