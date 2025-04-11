using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Mercado
    {
        List<Jugador> jugadoresEnMercado;
        List<Equipo> equipos;
        public Mercado()
        {
            jugadoresEnMercado = new List<Jugador>();
            equipos = new List<Equipo>();
        }
        public void AddJugador(Jugador jugador)
        {
            jugadoresEnMercado.Add(jugador);
        }
        public void VenderJugador(Jugador jugador, Equipo equipoDestino)
        {
            if (jugadoresEnMercado.Contains(jugador) && equipos.Contains(equipoDestino))
            {
                Console.WriteLine($"Vendido {jugador.Nombre} a {equipoDestino.Nombre}");
                jugadoresEnMercado.Remove(jugador);
                equipoDestino.AddJugador(jugador);
            }
            else
            {
                Console.WriteLine("El jugador o el equipo no están disponibles.");
            }
        }
    }
}
