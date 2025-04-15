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

        public static void RegistroUsuario()
        {
            string nombreFichero = "../../../usuarios.txt";
            string nombreDirectorio = "../../../Usuarios";
            bool encontrado;
            int indice = 0;
            do
            {
                encontrado = false;
                Console.WriteLine("Dime el nombre del nuevo usuario: ");
                string nombre = Console.ReadLine();

                if (File.Exists(nombreFichero))
                {
                    List<string> lineas = null;
                   
                    try
                    {
                        lineas = new List<string>(File.ReadAllLines(nombreFichero));
                        do
                        {
                            if (lineas[indice] == nombre)
                            {
                                encontrado = true;
                            }
                            indice++;
                        } while (!encontrado || indice < lineas.Count);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Error en el fichero " + ex.Message);
                    }

                }

            } while (encontrado);

            if (Directory.Exists(nombreDirectorio))
            {
                //if ()
            }
        }

        public static bool TipoDeInicio()
        {
            Console.Write("¿Tienes una cuenta creada?");
            string[] opciones = { "Si", "No" };
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.SetCursorPosition(Console.WindowWidth / 2, 1);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int indice = 0;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(5, 2);
                Console.Write("¿Tienes una cuenta creada?");
                for (int i = 0; i < opciones.Length; i++)
                {
                    int x = Console.WindowWidth/2;
                    int y = 2 + i;
                    Console.SetCursorPosition(x, y);
                    if (i == indice)
                        Console.Write($"> {opciones[i]}");
                    else
                        Console.Write($"  {opciones[i]}");
                }
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    indice = (indice - 1 + opciones.Length) % opciones.Length;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    indice = (indice + 1) % opciones.Length;
                }

            } while (key.Key != ConsoleKey.Enter);

            return indice == 0 ? true : false;
        }
        static void Main(string[] args)
        {
            if (TipoDeInicio())
            {
                InicioSesion();
            }
            else
            {
                RegistroUsuario();
            }
        }
    }
}
