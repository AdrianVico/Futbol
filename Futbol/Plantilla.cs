using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Plantilla
    {
        string nombre;
        List<JugadorFutbol> jugadores;
        public Plantilla(string nombre)
        {
            this.nombre = nombre;
            jugadores = new List<JugadorFutbol>();
        }
        public string Nombre { get => nombre; set => nombre = value; }
        public List<JugadorFutbol> Jugadores { get => jugadores; set => jugadores = value; }
        public void AddJugador(JugadorFutbol jugador)
        {
            jugadores.Add(jugador);
        }
        public void VenderJugador(JugadorFutbol jugador)
        {
            jugadores.Remove(jugador);
        }
    }
}
