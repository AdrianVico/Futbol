using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadasTest
{
    internal class Partido
    {

        string resultado;
        int numeroJornada;
        List<Equipo> equipos;
        Dictionary<Equipo, Equipo> partidos;
        Dictionary<Equipo, string> resultadosPorEquipo;
        public Partido()
        {
            equipos = RellenarListaEquipos();
            partidos = new Dictionary<Equipo, Equipo>();
            resultadosPorEquipo = new Dictionary<Equipo, string>();
            numeroJornada = 0;
        }

        public List<Equipo> RellenarListaEquipos()
        {
            List<Equipo> equipos = new List<Equipo>();
            equipos.Add(new Equipo("Equipo1"));
            equipos.Add(new Equipo("Equipo2"));
            equipos.Add(new Equipo("Equipo3"));
            equipos.Add(new Equipo("Equipo4"));
            equipos.Add(new Equipo("Equipo5"));
            equipos.Add(new Equipo("Equipo6"));
            equipos.Add(new Equipo("Equipo7"));
            equipos.Add(new Equipo("Equipo8"));
            equipos.Add(new Equipo("Equipo9"));
            equipos.Add(new Equipo("Equipo10"));
            equipos.Add(new Equipo("Equipo11"));
            equipos.Add(new Equipo("Equipo12"));
            return equipos;
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
            List<Equipo> clubes = equipos;

            do
            {
                Random rand = new Random();

                int equipo1 = rand.Next(0, clubes.Count - 1);
                Equipo local = clubes[equipo1];
                clubes.RemoveAt(equipo1);

                int equipo2 = rand.Next(0, clubes.Count - 1);
                Equipo visitante = clubes[equipo2];
                clubes.RemoveAt(equipo2);

                partidos.Add(local, visitante);

            } while (clubes.Count > 0);


            for (int i = 0; i < clubes.Count - 1; i += 2)
            {
                Equipo local = clubes[i];
                Equipo visitante = clubes[i + 1];

                partidos.Add(local, visitante);
            }
        }

        public void AnyadirEquiposAPartidos()
        {
            foreach (KeyValuePair<Equipo, Equipo> e in partidos)
            {
                string resultadoPartido = SimularResultado();

                resultadosPorEquipo.Add(e.Key, resultadoPartido);

                string invertido = resultado.Split('-')[1] + "-" + resultado.Split('-')[0];
                resultadosPorEquipo.Add(e.Value, invertido);
            }
        }

        public void MostrarNumeroJornada()
        {
            numeroJornada++;
            string titulo = $"Partidos de la J{numeroJornada}";
            int posX = (Console.WindowWidth - titulo.Length) / 2;

            Console.SetCursorPosition(posX, Console.CursorTop);
            Console.WriteLine(titulo);
        }

        public void MostrarGuardarPartidos()
        {
            string archivo = "jornadas.txt";
            StreamWriter stw = new StreamWriter(archivo, true);

            MostrarNumeroJornada();
            Console.WriteLine();

            foreach (KeyValuePair<Equipo, Equipo> e in partidos)
            {
                string resultadoPartido = SimularResultado();
                string invertido = resultado.Split('-')[1] + "-" + resultado.Split('-')[0];

                string lineaConsola = e.Key.Nombre.PadRight(10) + resultadoPartido.PadLeft(5).PadRight(9) + e.Value.Nombre.PadLeft(10);
                Console.SetCursorPosition((Console.WindowWidth - lineaConsola.Length) / 2, Console.CursorTop);
                Console.WriteLine(lineaConsola);

                stw.WriteLine(e.Key + ";" + resultadoPartido);
                stw.WriteLine(e.Value + ";" + invertido);
            }

            Console.WriteLine();
            stw.WriteLine("--------------");
            stw.Close();
        }

        public void LimpiarArchivo()
        {
            string archivo = "jornadas.txt";
            StreamWriter stw = new StreamWriter(archivo);
            stw.WriteLine();
        }
    }
}
