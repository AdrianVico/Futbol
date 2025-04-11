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
        public Jornada()
        {
            partidos = new List<Partido>();
        }

        public void AddPartido(Partido partido)
        {
            partidos.Add(partido);
        }
        public void MostrarPartidos()
        {
            Console.WriteLine("Partidos de la jornada:");
            foreach (Partido partido in partidos)
            {
                Console.WriteLine(partido.ToString());
            }
        }
    }
}
