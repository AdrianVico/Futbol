/*using System;
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
            Console.WriteLine("Bienvenido al menu");
            Console.WriteLine("1. Mercado");
            Console.WriteLine("2. Equipo");
            Console.WriteLine("3. Salir");
            int opcion = 0;
            do
            {
                Console.WriteLine("Elige una opción:");
                opcion = int.Parse(Console.ReadLine());
                switch (opcion)
                {
                    case 1:
                        Console.WriteLine("Elige una opcion: ");
                        Console.WriteLine("1. Comprar");
                        Console.WriteLine("2. Vender");
                        Console.WriteLine("3. Salir");
                        int opcionMercado = int.Parse(Console.ReadLine());
                        switch (opcionMercado)
                        {
                            case 1:
                                Console.WriteLine("Elige un jugador para comprar:");
                                mercado.IniciarMercado();
                                break;
                            case 2:
                                Console.WriteLine("Elige un jugador para vender:");
                                mercado.Vender();
                                break;
                            case 3:
                                Console.WriteLine("Saliendo...");
                                break;
                            default:
                                Console.WriteLine("Opción no válida");
                                break;
                        }
                        break;
                    case 2:
                        break;
                    case 3:
                        Console.WriteLine("Saliendo...");
                        break;
                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                }
            } while (opcion != 4);
        }
    }
}*/
