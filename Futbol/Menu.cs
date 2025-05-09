using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Menu
    {
        Usuario usuario;
        Mercado mercado;

        public Menu(Usuario usuario)
        {
            this.usuario = usuario;
            mercado = new Mercado(usuario);
            string[] opcionesMenu = { "Mercado", "Equipo", "Liga", "Jornada", "Salir" };
            int indiceSeleccionado = 0;
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("Bienvenido al menú\n");

                for (int i = 0; i < opcionesMenu.Length; i++)
                {
                    if (i == indiceSeleccionado)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"> {opcionesMenu[i]}   ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($"  {opcionesMenu[i]}   ");
                    }
                }
                ConsoleKeyInfo tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.LeftArrow:
                        indiceSeleccionado = (indiceSeleccionado == 0) ? opcionesMenu.Length - 1 : indiceSeleccionado - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        indiceSeleccionado = (indiceSeleccionado + 1) % opcionesMenu.Length;
                        break;
                    case ConsoleKey.Enter:
                        switch (indiceSeleccionado)
                        {
                            case 0:
                                MostrarMenuMercado();
                                break;
                            case 1:
                                MostrarMenuEquipo();
                                break;
                            case 2:
                                Console.WriteLine("\n(Mostrar liga no implementado)");
                                Console.ReadKey();
                                break;
                            case 3:
                                Console.WriteLine("\n(Mostrar jornada no implementado)");
                                Console.ReadKey();
                                break;
                            case 4:
                                salir = true;
                                break;
                        }
                        break;
                }
            }
        }

        private void MostrarMenuMercado()
        {
            string[] opcionesMercado = { "Comprar", "Vender", "Volver" };
            int indice = 0;
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("Mercado\n");

                for (int i = 0; i < opcionesMercado.Length; i++)
                {
                    if (i == indice)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {opcionesMercado[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {opcionesMercado[i]}");
                    }
                }

                ConsoleKeyInfo tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.UpArrow:
                        indice = (indice == 0) ? opcionesMercado.Length - 1 : indice - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        indice = (indice + 1) % opcionesMercado.Length;
                        break;
                    case ConsoleKey.Enter:
                        switch (indice)
                        {
                            case 0:
                                Console.Clear();
                                Console.WriteLine("Elige un jugador para comprar:");
                                mercado.IniciarMercado();
                                Console.ReadKey();
                                break;
                            case 1:
                                Console.Clear();
                                Console.WriteLine("Elige un jugador para vender:");
                                mercado.Vender();
                                Console.ReadKey();
                                break;
                            case 2:
                                volver = true;
                                break;
                        }
                        break;
                }
            }
        }
        private void MostrarMenuEquipo()
        {
            string[] opcionesEquipo = { "Ver equipo", "Modificar alineación", "Volver" };
            int indice = 0;
            bool volver = false;

            while (!volver)
            {
                Console.Clear();
                Console.WriteLine("Equipo\n");
                for (int i = 0; i < opcionesEquipo.Length; i++)
                {
                    if (i == indice)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"> {opcionesEquipo[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {opcionesEquipo[i]}");
                    }
                }
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                switch (tecla.Key)
                {
                    case ConsoleKey.UpArrow:
                        indice = (indice == 0) ? opcionesEquipo.Length - 1 : indice - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        indice = (indice + 1) % opcionesEquipo.Length;
                        break;
                    case ConsoleKey.Enter:
                        switch (indice)
                        {
                            case 0:
                                Console.Clear();
                                usuario.Equipo.Jugadores.ForEach(j => Console.WriteLine(j.ToString()));
                                Console.ReadKey();
                                break;
                            case 1:
                                // Modificar alineación
                                break;
                            case 2:
                                volver = true;
                                break;
                        }
                        break;
                }
            }
        }
    }
}