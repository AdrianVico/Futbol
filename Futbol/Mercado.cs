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
            this.equipoUsu = equipoUsu;
            jugadoresMercado = new List<Jugador>();
        }
        public List<Jugador> LeerFicheroJugadores()
        {
            List<Jugador> jugadoresTotales = new List<Jugador>();
            string[] jugadores = File.ReadAllLines("../../../Jugadores/LEYENDAS.txt");
            foreach (string linea in jugadores)
            {
                string[] datos = linea.Split(';');
                Jugador jugador = new Jugador(datos[0], datos[1], datos[2], Convert.ToInt32(datos[3]));
                jugadoresTotales.Add(jugador);
            }
            return jugadoresTotales;
        }
        public void AgregarJugadorMercado()
        {
            jugadoresMercado.Clear();
            List <Jugador> jugadoresTotales = LeerFicheroJugadores();
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next(0, jugadoresTotales.Count);
                jugadoresMercado.Add(jugadoresTotales[num]);
                jugadoresTotales.RemoveAt(num);
            }
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
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"> {jugadoresMercado[i].ToString()}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {jugadoresMercado[i].ToString()}");
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
            Console.WriteLine($"Has comprado a {jugador.Nombre} por {jugador.Precio}$");
            equipoUsu.AddJugador(jugador);
            jugadoresMercado.Remove(jugador);
            BorrarJugadorDelFichero(jugador);
            Console.ReadLine();
        }
        public void BorrarJugadorDelFichero(Jugador jugador)
        {
            string[] jugadores = File.ReadAllLines("../../../Jugadores/LEYENDAS.txt");
            List<string> jugadoresActualizados = new List<string>(jugadores);

            bool encontrado = false;
            for (int i = 0; i < jugadores.Length && !encontrado; i++)
            {
                if (jugadores[i].Contains(jugador.Nombre))
                {
                    jugadoresActualizados.RemoveAt(i);
                    encontrado = true;
                }
            }
            File.WriteAllLines("../../../Jugadores/LEYENDAS.txt", jugadoresActualizados);
        }
        public void AgregarJugadorAlFicheroDelEquipo(Jugador jugador)
        {

        }
    }
}
