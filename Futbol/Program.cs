using System.Runtime.InteropServices;

namespace Futbol
{
    internal class Program
    {

        public static void InicioSesion()
        {
            
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Hola soy Izan");
            Console.WriteLine("Hola soy Alejandro");
            Console.WriteLine("Hola soy Sergio");
            Console.WriteLine("Hola soy Marcos");
            Console.WriteLine("Hola soy Marcos");
            Console.WriteLine("Hola soy Marcos");

            if (!Directory.Exists("usuarios_registrados"))
            {
                Directory.CreateDirectory("usuarios_registrados");
            }
        }
    }
}
