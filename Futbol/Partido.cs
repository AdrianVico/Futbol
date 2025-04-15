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
        Dictionary<Equipo, string> resultadosPorEquipo;
        int puntosGanador;
        int puntosPerdedor;
        public Partido()
        {
            equipos = new List<Equipo>();
            partidos = new Dictionary<Equipo, Equipo>();
            resultadosPorEquipo = new Dictionary<Equipo, string>();
            numeroJornada = 0;
            puntosGanador = 0;
            puntosPerdedor = 0;

        }
        public string Resultado { get => resultado; set => resultado = value; }

        public string SimularResultado()
        {
            Random rand = new Random();
            int goles1 = rand.Next(0, 9);
            int goles2 = rand.Next(0, 9);

            resultado = $"{goles1}-{goles2}";
            return resultado;
        }
        public void AnyadirPartidos()
        {
            for (int i = 0; i < equipos.Count - 1; i += 2)
            {
                Equipo local = equipos[i];
                Equipo visitante = equipos[i + 1];

                partidos.Add(local, visitante);
            }
        }

        public void RepartirPuntos(string resultado)
        {
            int goles1 = Convert.ToInt32(resultado.Split('-')[0]);
            int goles2 = Convert.ToInt32(resultado.Split('-')[1]);
            puntosGanador += goles1 > goles2 ? 3 : goles1 == goles2 ? 1 : 0;
            puntosPerdedor += goles2 > goles1 ? 3 : goles2 == goles1 ? 1 : 0;
        }

        public void MostrarPartidos()
        {
            numeroJornada++;
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.WriteLine($"Partidos de la J{numeroJornada}");
            Console.WriteLine();
            foreach (KeyValuePair<Equipo, Equipo> e in partidos)
            {
                string resultadoPartido = SimularResultado();
                Console.WriteLine(e.Key + resultadoPartido + e.Value);
                resultadosPorEquipo.Add(e.Key, resultadoPartido);
                string invertido = resultado.Split('-')[1] + "-" + resultado.Split('-')[0];
                resultadosPorEquipo.Add(e.Value, invertido);
            }
        }

        public void GuardarResultados()
        {
            string archivo = "clasificación.txt";
            StreamWriter stw = new StreamWriter(archivo);

            foreach (KeyValuePair<Equipo, string> e in resultadosPorEquipo)
            {
                stw.WriteLine(e.Key + e.Value);
            }

            stw.Close();
        }
    }
}
