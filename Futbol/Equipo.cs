﻿using System;
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

        public Equipo(string nombre, List<Jugador> jugadores, int[] alineacion)
        {
            this.nombre = nombre;
            this.jugadores = jugadores;
            this.alineacion = alineacion;
        }

        public Equipo(string nombre, List<Jugador> jugadores)
        {
            this.nombre = nombre;
            this.jugadores = jugadores;
            banquillo = new List<Jugador>();
            alineacion = new int[] { 1, 4, 4, 2 };
        }

        public string Nombre { get => nombre; set => nombre = value; }
        internal List<Jugador> Jugadores { get => jugadores; set => jugadores = value; }
        internal List<Jugador> Banquillo { get => banquillo; set => banquillo = value; }
        public int[] Alineacion { get => alineacion; set => alineacion = value; }
        

        public static List<Jugador> JugadoresJornada(string nombre)
        {
            List<Jugador> jugadoresEquipo = new List<Jugador>();
            string ruta = $"../../../EquiposLiga/{nombre}.txt";
            string[] jugadores = File.ReadAllLines(ruta);
            string[] partes = null;
            foreach (string j in jugadores)
            {
                partes = j.Split(';');
                jugadoresEquipo.Add(new Jugador(partes[0], partes[1], partes[2],
                    Convert.ToInt32(partes[3])));
            }
            return jugadoresEquipo;
        }

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
            string siglas = nombre.Substring(0, nombre.Length >= 4 ? 4 : nombre.Length);
            int LeftPad = (4 - siglas.Length) / 2;
            int RightPad = 4 - (siglas.Length + LeftPad);
            siglas = new string(' ', LeftPad) + siglas + new string(' ', RightPad);
            return siglas;
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
            foreach (string l in jugadoresFichero)
            {
                partes = l.Split(';');
                jugadores.Add(new Jugador(partes[0], partes[1], partes[2],
                    Convert.ToInt32(partes[3])));
            }
            return jugadores;
        }

        public string[] MostrarCamisetas()
        {
            string[] resultado = new string[(6 + 1 + 1) * (3 + 1)];
            int linea = 0;
            if (alineacion.Length != 4)
            {
                Console.WriteLine("La alineación no es válida(solo 4 lineas)");
            }
            else
            {
                string si = GetSiglas();
                string plantillaCamiseta =
                                "  __    __  \r\n" +
                                " /  `--´  \\ \r\n" +
                               $"/_| {si} |_\\\r\n" +
                                "  |      |  \r\n" +
                                "  |      |  \r\n" +
                                "  |______|  ";
                string[] lineasCamiseta = plantillaCamiseta.Split("\r\n");
                int anchoCamiseta = 12;
                int altoCamiseta = lineasCamiseta.Length;
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

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Equipo other = (Equipo)obj;
            return nombre == other.nombre;
        }

        public override int GetHashCode()
        {
            return nombre.GetHashCode();
        }

        public override string ToString()
        {
            return nombre;
        }
    }
}
