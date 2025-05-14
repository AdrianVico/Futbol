using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    public static class MenuSelector
    {
        public static int SeleccionarOpcion(List<string> opciones, string titulo)
        {
            ConsoleKeyInfo key;
            int indice = 0;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(5, 2);
                Console.Write(titulo);

                for (int i = 0; i < opciones.Count; i++)
                {
                    int x = Console.WindowWidth / 2;
                    int y = 2 + i;
                    Console.SetCursorPosition(x, y);
                    if (i == indice)
                        Console.Write($"> {opciones[i]}");
                    else
                        Console.Write($"  {opciones[i]}");
                }

                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                    indice = (indice - 1 + opciones.Count) % opciones.Count;
                else if (key.Key == ConsoleKey.DownArrow)
                    indice = (indice + 1) % opciones.Count;

            } while (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Escape);
            if (key.Key == ConsoleKey.Escape)
            {
                indice = -1;
            }
            return indice;
        }
    }
}
