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
        public Equipo(string nombre)
        {
            this.nombre = nombre;
            jugadores = new List<Jugador>();
        }
        public string Nombre { get => nombre; set => nombre = value; }
        public void AddJugador(Jugador jugador)
        {
            jugadores.Add(jugador);
        }
        public void VenderJugador(Jugador jugador)
        {
            jugadores.Remove(jugador);
        }
        public static void MostrarAlineacion()
        {
            Console.WriteLine("Alineación del equipo:");
            // Aquí se puede mostrar la alineación del equipo.
        }
    }
}
