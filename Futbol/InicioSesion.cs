using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class InicioSesion
    {
        const string NOMBRE_FICHERO = "../../../Usuarios/usuarios.txt";
        const string NOMBRE_DIRECTORIO = "../../../Usuarios";
        public static Usuario IniciarSesion()
        {
            string nombreFichero = NOMBRE_FICHERO;
            string nombreDirectorio = NOMBRE_DIRECTORIO;
            bool encontrado;
            string nombre = null;
            string password = "";
            List<string> lineas = null;
            int posicion = 0;

            if (File.Exists(nombreFichero))
            {

                try
                {
                    lineas = new List<string>(File.ReadAllLines(nombreFichero));

                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error en el fichero " + ex.Message);
                }


                do
                {
                    encontrado = false;
                    Console.WriteLine("Dime el nombre del usuario: ");
                    nombre = Console.ReadLine();
                    encontrado = EncontrarNombre(lineas, nombre, ref posicion);
                    if (!encontrado)
                    {
                        Console.WriteLine("No existe el usuario.");
                    }

                } while (!encontrado);

                string[] partes = lineas[posicion].Split(";");
                int intentos = 3;
                bool registrado = false;
                do
                {
                    Console.WriteLine("Dime la contraseña del usuario:");
                    password = Console.ReadLine();
                    if (password != partes[1])
                    {
                        intentos--;
                        Console.WriteLine("Contraseña incorrecta.");
                    }
                    else
                    {
                        Console.WriteLine("Inicio de sesión de satisfactorio.");
                        registrado = true;
                    }


                } while (intentos > 0 && !registrado);

                if (intentos == 0)
                {
                    Console.WriteLine("Te has quedado sin intentos");
                }
            }
            Usuario usuario = new Usuario(nombre, password);
            RellenarUsuario(usuario);
            return usuario;
        }

        public static Usuario RegistroUsuario()
        {
            string nombreFichero = NOMBRE_FICHERO;
            string nombreDirectorio = NOMBRE_DIRECTORIO;
            bool encontrado;
            string nombre = null;
            int indice = 0;
            List<string> lineas = null;
            if (File.Exists(nombreFichero))
            {

                try
                {
                    lineas = new List<string>(File.ReadAllLines(nombreFichero));

                }
                catch (IOException ex)
                {
                    Console.WriteLine("Error en el fichero " + ex.Message);
                }


                do
                {
                    encontrado = false;
                    Console.WriteLine("Dime el nombre del nuevo usuario: ");
                    nombre = Console.ReadLine();
                    encontrado = EncontrarNombre(lineas, nombre, ref indice);
                    if (encontrado)
                    {
                        Console.WriteLine("Ya hay un usuario con ese nombre.");
                    }

                } while (encontrado);
            }
            Console.WriteLine("Dime la contraseña: ");
            string password = Console.ReadLine();
            StreamWriter streamWriter = null;
            if (Directory.Exists(nombreDirectorio))
            {
                try
                {
                    streamWriter = new StreamWriter(nombreFichero, true);
                    streamWriter.WriteLine(nombre + ";" + password);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }

                finally 
                {
                    streamWriter.Close();
                }
                Directory.CreateDirectory(NOMBRE_DIRECTORIO+"\\"+nombre);
                string rutaDatos = Path.Combine(NOMBRE_DIRECTORIO, nombre, nombre + "_datos.txt");
                string rutaEquipo = Path.Combine(NOMBRE_DIRECTORIO, nombre, nombre + "_jugadores_equipo.txt");

                using (FileStream fs = File.Create(rutaDatos)) { }
                using (FileStream fs = File.Create(rutaEquipo)) { }
            }
            Usuario usu = new Usuario(nombre, password);
            usu.ActualizarFicheroDatos();
            return usu;
        }
        public static bool EncontrarNombre(List<string> lineas, string nombre, ref int posicion)
        {
            int indice = 0;
            bool encontrado = false;
            string[] partes = null;
            if (lineas.Count >= 1)
            {
                while (!encontrado && indice < lineas.Count)
                {
                    partes = lineas[indice].Split(";");
                    if (partes[0] == nombre)
                    {
                        encontrado = true;
                    }
                    else
                    {
                        indice++;
                    }
                }
            }
            posicion = indice;
            return encontrado;
        }

        public static bool TipoDeInicio()
        {
            string titulo = "¿Tienes una cuenta creada?";
            List<string> opciones = new List<string>{ "Si", "No" };
            /*Console.SetCursorPosition(Console.WindowWidth / 2, 0);
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
                {
                    indice = (indice - 1 + opciones.Length) % opciones.Length;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    indice = (indice + 1) % opciones.Length;
                }

            } while (key.Key != ConsoleKey.Enter);
            */
            return MenuSelector.SeleccionarOpcion(opciones,titulo) == 0 ? true : false;
        }
        public static string ElegirEquipoInicial()
        {
            Console.WriteLine("Ahora vamos a elegir el equipo inicial.");
            string[] opciones = { "Betis", "Borussia_Dortmund", "Olympique_Lyonn", "Roma", "Tottenham" };
            Console.SetCursorPosition(Console.WindowWidth / 2, 0);
            Console.SetCursorPosition(Console.WindowWidth / 2, 1);
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            int indice = 0;
            do
            {
                Console.Clear();
                Console.SetCursorPosition(5, 2);
                Console.Write("¿Qué plantilla quieres?");
                for (int i = 0; i < opciones.Length; i++)
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
                {
                    indice = (indice - 1 + opciones.Length) % opciones.Length;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    indice = (indice + 1) % opciones.Length;
                }

            } while (key.Key != ConsoleKey.Enter);
            return opciones[indice];
        }
        public static void CopiarEquipoInicial(string equipo, string usuario)
        {
            try
            {
                string archivo = $"../../../EquiposIniciales/{equipo}.txt";
                string archivoDestino = $"../../../Usuarios/{usuario}/{usuario}_jugadores_equipo.txt";

                string[] lineas = File.ReadAllLines(archivo);
                File.WriteAllLines(archivoDestino, lineas);
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error de entrada/salida al copiar el equipo: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocurrió un error inesperado: " + ex.Message);
            }


        }

        public static void RellenarUsuario(Usuario usuario)
        {
            string lineas = File.ReadAllText(NOMBRE_DIRECTORIO+$"/{usuario.Nombre}/{usuario.Nombre}_datos.txt");
            string lineasJugadores;
            string[] partes = lineas.Split(";");
            usuario.Dinero = Convert.ToInt64(partes[0]);
            usuario.Puntos = Convert.ToInt32(partes[1]);

        }
        public static Usuario Inicio()
        {
            Usuario usu = null;
            if (TipoDeInicio())
            {
                Console.Clear();
                usu = IniciarSesion();
            }
            else
            {
                Console.Clear();
                usu = RegistroUsuario();
                string nombreUsuario = usu.Nombre;
                string equipoInicial = ElegirEquipoInicial();
                CopiarEquipoInicial(equipoInicial, nombreUsuario);
            }
            
            usu.Equipo = new Equipo(usu.Nombre, Equipo.RellenarEquipo(usu.Nombre));
           // Console.WriteLine(usu);
            return usu;
        }
    }
}
