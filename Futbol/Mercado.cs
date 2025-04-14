using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Mercado
    {
        List<Jugador> jugadoresMercado;
        Equipo equipoUsu;

        public Mercado(Equipo equipoUsu)
        {
            jugadoresMercado = new List<Jugador>();
            this.equipoUsu = equipoUsu;
        }
        public void AddJugador(Jugador jugador)
        {
            jugadoresMercado.Add(jugador);
        }
        public void SeleccionarJugador()
        {
            ConsoleKeyInfo keyInfo;
            int indice = 0;
            do
            {
                Console.WriteLine("Jugadores en el mercado:");
                for (int i = 0; i < jugadoresMercado.Count; i++)
                {
                    if (i == indice)
                    {
                        Console.WriteLine($"> {jugadoresMercado[i].ToString()}");
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine(jugadoresMercado[i].ToString());
                    }
                }
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    indice = (indice == 0) ? jugadoresMercado.Count - 1 : indice - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    indice = (indice == jugadoresMercado.Count - 1) ? 0 : indice + 1;
                }
                Console.Clear();
            } while (keyInfo.Key != ConsoleKey.Enter);

            ComprarJugador(jugadoresMercado[indice]);
        }
        public void ComprarJugador(Jugador jugador)
        {
            Console.WriteLine($"Has comprado a {jugador.Nombre} por {jugador.Precio}.");
            equipoUsu.AddJugador(jugador);
            jugadoresMercado.Remove(jugador);
        }
    }
}
