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
        string rutaLeyendas = "../../../Jugadores/LEYENDAS.txt";
        List<Jugador> jugadoresMercado;
        List<Jugador> jugadoresUsuario;
        Usuario usuario;

        public Mercado(Usuario usuario)
        {
            this.usuario = usuario;
            jugadoresMercado = new List<Jugador>();
        }
        public List<Jugador> LeerFicheroJugadores(string ruta)
        {
            List<Jugador> jugadoresTotales = new List<Jugador>();
            string[] jugadores = File.ReadAllLines(ruta);
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
            List<Jugador> jugadoresTotales = LeerFicheroJugadores(rutaLeyendas);
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                int num = random.Next(0, jugadoresTotales.Count);
                jugadoresMercado.Add(jugadoresTotales[num]);
                jugadoresTotales.RemoveAt(num);
            }
        }

        public void SeleccionarJugadorMercado()
        {
            LeerFicheroJugadores(rutaLeyendas);
            AgregarJugadorMercado();
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
            } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

            if (keyInfo.Key != ConsoleKey.Escape)
            {
                ComprarJugador(jugadoresMercado[indice]);
            }
        }
        public void SeleccionarJugadorUsuario()
        {
            jugadoresUsuario = usuario.Equipo.Jugadores;
            ConsoleKeyInfo keyInfo;
            int indice = 0;
            do
            {
                Console.WriteLine("Jugadores en el mercado:");
                for (int i = 0; i < jugadoresUsuario.Count; i++)
                {
                    if (i == indice)
                    {
                        Console.BackgroundColor = ConsoleColor.Magenta;
                        Console.WriteLine($"> {jugadoresUsuario[i].ToString()}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {jugadoresUsuario[i].ToString()}");
                    }
                }
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    indice = (indice == 0) ? jugadoresUsuario.Count - 1 : indice - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    indice = (indice == jugadoresUsuario.Count - 1) ? 0 : indice + 1;
                }
                Console.Clear();
            } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);
           
            if(keyInfo.Key != ConsoleKey.Escape)
            {
                VenderJugador(jugadoresUsuario[indice]);
            }
            
        }
        public void ComprarJugador(Jugador jugador)
        {
            if(usuario.Dinero >= jugador.Precio)
            {
                usuario.Dinero -= jugador.Precio;
                Console.WriteLine($"Has comprado a {jugador.Nombre} por {jugador.Precio}$");
                usuario.Equipo.AddJugador(jugador);
                AgregarJugadoresAlFicheroDelUsuario(jugador);
                jugadoresMercado.Remove(jugador);
                BorrarJugadorDelFichero(jugador);       
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("No tienes suficiente dinero disponible para comprar el jugador.");
            }
            
        }

        public void VenderJugador(Jugador jugador)
        {
            if (usuario.Equipo.Jugadores.Contains(jugador))
            {
                usuario.Dinero += jugador.Precio;
                Console.WriteLine($"Has vendido a {jugador.Nombre} por {jugador.Precio}$");
                usuario.Equipo.RemoveJugador(jugador);
                AgregarJugadorAlFichero(jugador);
                BorrarJugadoresDelFicheroDelUsuario(jugador);
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("No tienes el jugador en tu equipo.");
            }
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
        public void AgregarJugadorAlFichero(Jugador jugador)
        {
            string ruta = "../../../Jugadores/LEYENDAS.txt";
            File.AppendAllText(ruta, $"{jugador.Nombre};{jugador.Posicion};{jugador.EquipoOrigen};{jugador.Precio}\n");
        }
        public void BorrarJugadoresDelFicheroDelUsuario(Jugador jugador)
        {
            string ruta = $"../../../Usuarios/{usuario.Nombre}/{usuario.Nombre}_jugadores_equipo.txt";
            string[] jugadores = File.ReadAllLines(ruta);
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
            File.WriteAllLines(ruta, jugadoresActualizados);
        }
        public void AgregarJugadoresAlFicheroDelUsuario(Jugador jugador)
        {
            string ruta = $"../../../Usuarios/{usuario.Nombre}/{usuario.Nombre}_jugadores_equipo.txt";

            File.AppendAllText(ruta, $"{jugador.Nombre};{jugador.Posicion};{jugador.EquipoOrigen};{jugador.Precio}\n");
        }
        public void IniciarMercado()
        {
            LeerFicheroJugadores(rutaLeyendas);
            AgregarJugadorMercado();
            SeleccionarJugadorMercado();
        }
        public void Vender()
        {
            SeleccionarJugadorUsuario();
        }
    }
}
