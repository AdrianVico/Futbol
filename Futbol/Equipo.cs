using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Equipo
    {
        string nombre;
        List<Jugador> jugadores;
        List<Jugador> banquillo;
        int[] alineacion = null;

        public Equipo(string nombre)
        {
            this.nombre = nombre;
            jugadores = new List<Jugador>();
            banquillo = new List<Jugador>();
            alineacion = new int[4];
        }

        public Equipo(string nombre, List<Jugador> jugadores, List<Jugador> banquillo, int[] alineacion)
        {
            this.nombre = nombre;
            this.jugadores = jugadores;
            this.banquillo = banquillo;
            this.alineacion = alineacion;
        }

        public Equipo(string nombre, List<Jugador> jugadores)
        {
            this.nombre = nombre;
            this.jugadores = jugadores;
            banquillo = new List<Jugador>();
            alineacion = new int[] {1, 4, 4, 2};
        }

        public string Nombre { get => nombre; set => nombre = value; }
        internal List<Jugador> Jugadores { get => jugadores; set => jugadores = value; }
        internal List<Jugador> Banquillo { get => banquillo; set => banquillo = value; }
        public int[] Alineacion { get => alineacion; set => alineacion = value; }

        public void AddJugador(Jugador jugador)
        {
            jugadores.Add(jugador); 
        }
        public void RemoveJugador(Jugador jugador)
        {
            jugadores.Remove(jugador);
        }
        public string GetSiglas()
        {
            return nombre.Substring(0, 4);
        }
        public string GetNombreCamiseta()
        {
            return nombre;
        }

        public static List<Jugador> RellenarEquipo(string nombre)
        {
            List<Jugador> jugadores = new List<Jugador>();
            string ruta = $"../../../Usuarios/{nombre}/{nombre}_jugadores_equipo.txt";
            string[] jugadoresFichero = File.ReadAllLines(ruta);
            string[] partes = null;
            foreach (string l in  jugadoresFichero)
            {
                partes = l.Split(';');
                jugadores.Add(new Jugador(partes[0], partes[1], partes[2],
                    Convert.ToInt32(partes[3])));
            }
            return jugadores;
        }

        public string[] MostrarCamisetas()
        {
            string[] resultado = new string[100];//(6 + 1 +1 ) * (3 + 1) 6 de alto de la camiseta + 1 del nombre, 3 filas de jugadores + 1 del portero
            int linea = 0;
            // Verificar que la alineación es válida
            if (alineacion.Length != 4)
            {
                Console.WriteLine("La alineación no es válida(solo 4 lineas)");
            }
            else
            {
                // Plantilla de la camiseta
                string si = GetSiglas();
                string plantillaCamiseta =
                                "  __    __  \r\n" +
                                " /  `--´  \\ \r\n" +
                               $"/_| {si} |_\\\r\n" +
                                "  |      |  \r\n" +
                                "  |      |  \r\n" +
                                "  |______|  ";
                string[] lineasCamiseta = plantillaCamiseta.Split("\r\n");//6
                int anchoCamiseta = 12; // Ancho de caracteres cada camiseta
                int altoCamiseta = lineasCamiseta.Length; // Alto de la camiseta
                //padding
                int anchoMax = alineacion.Max() * anchoCamiseta;
                foreach (int fila in alineacion)
                {
                    int anchoFila = fila * anchoCamiseta;
                    for (int a = 0; a < altoCamiseta + 1; a++)
                    {
                        resultado[linea] = new string(' ', (anchoMax - anchoFila) / 2);
                        linea++;
                    }
                    resultado[linea] = "";
                    linea++;
                }
                //texto
                linea = 0;
                string[] nombresJugadores = jugadores.Select(j => j.Nombre).ToArray();
                for (int i = 0; i < nombresJugadores.Length; i++)
                {
                    if (nombresJugadores[i].Length > 10)
                        nombresJugadores[i] = " " + nombresJugadores[i].Substring(0, 10) + " ";
                    else
                        nombresJugadores[i] =
                        new string(' ', (anchoCamiseta - nombresJugadores[i].Length) / 2)
                        + nombresJugadores[i]
                        + new string(' ', (anchoCamiseta - nombresJugadores[i].Length) / 2 + ((anchoCamiseta - nombresJugadores[i].Length) % 2));
                }
                int jugadorActual = 0;
                foreach (int fila in alineacion)
                {
                    for (int l = 0; l < altoCamiseta; l++)
                    {
                        for (int j = 0; j < fila; j++)
                        {
                            resultado[linea] += lineasCamiseta[l];
                        }
                        linea++;
                    }
                    for (int j = 0; j < fila; j++)
                    {
                        resultado[linea] += nombresJugadores[jugadorActual];
                        jugadorActual++;
                    }
                    linea++;
                    resultado[linea] += "";
                    linea++;
                }
            }
            return resultado;
        }
    }
}
