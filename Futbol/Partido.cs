using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Partido
    {
        string resultado;
        int numeroJornada;
        List<Equipo> equipos;
        Dictionary<Equipo, Equipo> partidos;
        public Partido()
        {
            equipos = new List<Equipo>();
            partidos = new Dictionary<Equipo, Equipo>();
        }
        public string Resultado { get => resultado; set => resultado = value; }

        public string SimularResultado()
        {
            Random golesEquipo1 = new Random();
            golesEquipo1.Next(0, 9);
            Random golesEquipo2 = new Random();
            golesEquipo2.Next(0, 9);

            resultado = $" {golesEquipo1} - {golesEquipo2} ";

            return resultado;
        }
        public void AnyadirPartido()
        {
            for (int i = 0; i < equipos.Count - 1; i += 2)
            {
                Equipo local = equipos[i];
                Equipo visitante = equipos[i + 1];

                partidos.Add(local, visitante);
            }
        }

        public void AumentarJornada()
        {
            numeroJornada += 1;
        }

        public void MostrarPartidos()
        {
            AumentarJornada();
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.WriteLine($"Partidos de la J{numeroJornada}");
            Console.WriteLine();
            foreach (KeyValuePair<Equipo, Equipo> e in partidos)
            {
                Console.WriteLine(e.Key + SimularResultado() + e.Value);
            }
        }
    }
}
