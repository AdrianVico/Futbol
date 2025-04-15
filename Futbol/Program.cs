using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

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
            string nombreFichero = "../../../Usuarios/usuarios.txt";
            string nombreDirectorio = "../../../Usuarios";
            bool encontrado;
            int indice = 0;
            string nombre;
            do
            {
                encontrado = false;
                Console.WriteLine("Dime el nombre del nuevo usuario: ");
                nombre = Console.ReadLine();

                if (File.Exists(nombreFichero))
                {
                    List<string> lineas = null;
                   
                    try
                    {
                        lineas = new List<string>(File.ReadAllLines(nombreFichero));
                        string[] partes = null;
                        if (lineas.Count >= 1)
                        {
                            while (!encontrado && indice < lineas.Count) 
                            {
                                partes = lineas[indice].Split(";");
                                if (partes[0] == nombre)
                                {
                                    encontrado = true;
                                    Console.WriteLine("Ya hay un usuario con ese nombre.");
                                }
                                indice++;
                            } 
                        }
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("Error en el fichero " + ex.Message);
                    }

                }

            } while (encontrado);

            Console.WriteLine("Dime la contraseña: ");
            string password = Console.ReadLine();
            StreamWriter streamWriter = null;
            if (Directory.Exists(nombreDirectorio))
            {
                try
                {
                    streamWriter = new StreamWriter(nombreFichero, true);
                    streamWriter.WriteLine(nombre+";"+password);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
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
                Console.Clear();
                RegistroUsuario();
            }
        }
    }
}
