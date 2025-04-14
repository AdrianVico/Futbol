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

        public static int Seleccion()
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
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey();
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        //indice == -1 ? 0 : indice -1; 
                        
                        break;
                }
            }



            return 0;
        }
        static void Main(string[] args)
        {
            
        }
    }
}
