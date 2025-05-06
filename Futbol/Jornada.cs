using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Jornada
    {
        List<Partido> partidos;
        int numeroJornada;
        public Jornada()
        {
            partidos = new List<Partido>();
            numeroJornada = 1;
        }

        public void AddPartido(Partido partido)
        {
            partidos.Add(partido);
        }
        public void MostrarPartidos()
        {
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.WriteLine($"Partidos de la J{numeroJornada}");
            Console.WriteLine();
            foreach (Partido partido in partidos)
            {
                Console.WriteLine(partido.ToString());
            }
            numeroJornada++;
        }
    }
}
