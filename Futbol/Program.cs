using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace Futbol
{
    internal class Program
    {

        public static void InicioSesion()
        {
            Console.Write("");
        }

        public static bool InicioDeSesion()
        {
            Console.Write("¿Tienes una cuenta creada?");
            string[] opciones = { "Si", "No" };
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.WriteLine("> "+opciones[0]);
            Console.ResetColor();
            Console.SetCursorPosition(Console.WindowWidth / 2, 1);
            Console.WriteLine(opciones[1]);
            ConsoleKeyInfo keyInfo;
            int indice = 0;
            
            do
            {
                    Console.SetCursorPosition(Console.WindowWidth / 2, indice);
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(opciones[indice]);
                    Console.ResetColor();
                    keyInfo = Console.ReadKey(true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.UpArrow:
                            indice = (indice != 0) ? indice-- : 1;
                            break;
                        case ConsoleKey.DownArrow:
                            indice = (indice != 1) ? indice++ : 0;
                            break;
                    }
                    Console.SetCursorPosition(Console.WindowWidth / 2, indice);
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("> " + opciones[indice]);
                    Console.ResetColor();
            } while (keyInfo.Key != ConsoleKey.Enter);
            return indice != 1 ? true : false;
        }
        static void Main(string[] args)
        {
            Console.WriteLine(InicioDeSesion());
        }
    }
}
