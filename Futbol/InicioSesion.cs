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
            Usuario usuario = null;

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

                    List<string> mensajeNombre = new List<string>
                    {
                        "INICIO DE SESIÓN",
                        "",
                        "Dime el nombre del usuario:",
                        "---------------------------",
                       "|                           |",
                        "---------------------------"
                    };

                    int padTopTexto = Menu.DibujarCuadro(mensajeNombre);

                    int posicionX = (209 - 29) / 2;
                    int posicionY = padTopTexto + mensajeNombre.Count - 2;

                    Console.SetCursorPosition(posicionX, posicionY);
                    nombre = Console.ReadLine();

                    encontrado = EncontrarNombre(lineas, nombre, ref posicion);

                    if (!encontrado)
                    {
                        Menu.DibujarCuadro(new List<string> { "ERROR :(", "No existe el usuario." });
                        Console.ReadKey();
                    }
                } while (!encontrado);

                string[] partes = lineas[posicion].Split(";");
                int intentos = 3;
                bool registrado = false;

                do
                {
                    List<string> mensajePassword = new List<string>
                    {
                        "INICIO DE SESIÓN",
                        "",
                        $"Usuario: {nombre}",
                        "",
                        $"Contraseña de {nombre}:",
                        "---------------------------",
                       "|                           |",
                        "---------------------------",
                        $"(Intentos restantes: {intentos})"
                    };

                    int padTopTexto = Menu.DibujarCuadro(mensajePassword);

                    int posicionX = (209 - 29) / 2; // Centrado aprox para campo de texto
                    int posicionY = padTopTexto + mensajePassword.Count - 3;

                    Console.SetCursorPosition(posicionX, posicionY);
                    password = LeerPasswordConAsteriscos();

                    if (password != partes[1])
                    {
                        intentos--;
                        Menu.DibujarCuadro(new List<string> { "ERROR :(", "Contraseña incorrecta." });
                        Console.ReadKey();
                    }
                    else
                    {
                        Menu.DibujarCuadro(new List<string> { $"Sesión iniciada como {nombre.ToUpper()}" });
                        Console.ReadKey();
                        registrado = true;
                    }
                } while (intentos > 0 && !registrado);

                if (intentos == 0)
                {
                    Menu.DibujarCuadro(new List<string> { "ERROR :(", "Te has quedado sin intentos" });
                    Console.ReadKey();

                }
                else
                {
                    usuario = new Usuario(nombre, password);
                }
            }
            else
            {
                Console.WriteLine("ERROR :(  : El archivo de usuarios no existe");
            }
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

                    List<string> mensajeNombre = new List<string>
                    {
                        "REGISTRO DE USUARIO",
                        "",
                        "Dime el nombre del nuevo usuario:",
                        "---------------------------",
                       "|                           |",
                        "---------------------------"
                    };

                    int padTopTexto = Menu.DibujarCuadro(mensajeNombre);
                    int posicionX = (209 - 29) / 2;
                    int posicionY = padTopTexto + mensajeNombre.Count - 2;
                    Console.SetCursorPosition(posicionX, posicionY);

                    nombre = Console.ReadLine();
                    encontrado = EncontrarNombre(lineas, nombre, ref indice);

                    if (encontrado)
                    {
                        Menu.DibujarCuadro(new List<string> { "ERROR :(", "Ya hay un usuario con ese nombre." });
                        Console.ReadKey();
                    }
                } while (encontrado);
            }

            List<string> mensajePassword = new List<string>
            {
                "REGISTRO DE USUARIO",
                "",
                $"Dime la contraseña para {nombre}:",
                "---------------------------",
               "|                           |",
                "---------------------------"
            };

            int padTopTextoPass = Menu.DibujarCuadro(mensajePassword);
            int posicionXPass = (209 - 29) / 2;
            int posicionYPass = padTopTextoPass + mensajePassword.Count - 2;
            Console.SetCursorPosition(posicionXPass, posicionYPass);

            string password = LeerPasswordConAsteriscos();
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
                    streamWriter?.Close();
                }

                Directory.CreateDirectory(NOMBRE_DIRECTORIO + "\\" + nombre);
                string rutaDatos = Path.Combine(NOMBRE_DIRECTORIO, nombre, nombre + "_datos.txt");
                string rutaEquipo = Path.Combine(NOMBRE_DIRECTORIO, nombre, nombre + "_jugadores_equipo.txt");

                using (FileStream fs = File.Create(rutaDatos)) { }
                using (FileStream fs = File.Create(rutaEquipo)) { }
            }
            Usuario usu = new Usuario(nombre, password);
            //usu.ActualizarFicheroDatos();
            return usu;
        }

        static string LeerPasswordConAsteriscos()
        {
            string password = "";
            ConsoleKeyInfo tecla;

            do
            {
                tecla = Console.ReadKey(true); // true = no muestra la tecla

                if (tecla.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // borra el último asterisco, \b = retrocede el cursor
                }
                else if (!char.IsControl(tecla.KeyChar))// .IsControl es para comprobar que el caracter sea imprimible(no enter,backspace,etc.)
                {
                    password += tecla.KeyChar;// añade el caracter a la password
                    Console.Write("*");
                }
            } while (tecla.Key != ConsoleKey.Enter);

            return password;
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
            List<string> opciones = new List<string> { "Si", "No" };
            return MenuSelector.SeleccionarOpcion(opciones, titulo) == 0 ? true : false;
        }
        public static string ElegirEquipoInicial()
        {
            List<string> preguntas = new List<string> { "¿Qué plantilla quieres?" };
            List<string> opciones = new List<string> { "Betis", "Borussia_Dortmund", "Olympique_Lyonn", "Roma", "Tottenham" };
            return opciones[Menu.CrearMenuVertical(preguntas, opciones)];
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
            string lineas = File.ReadAllText(NOMBRE_DIRECTORIO + $"/{usuario.Nombre}/{usuario.Nombre}_datos.txt");
            string lineasJugadores;
            string[] partes = lineas.Split(";");
            usuario.Dinero = Convert.ToInt64(partes[0]);
            usuario.Puntos = Convert.ToInt32(partes[1]);
            string[] alineacionPartes = partes[2].Split(",");
            int[] alineacion = new int[alineacionPartes.Length];
            for (int i = 0; i < alineacionPartes.Length; i++)
            {
                alineacion[i] = Convert.ToInt32(alineacionPartes[i]);
            }
            usuario.Equipo.Alineacion = alineacion;

        }
        public static Usuario Inicio(bool tipo)
        {
            Usuario usu = null;
            if (tipo)
            {
                Console.Clear();
                usu = IniciarSesion();
                if (usu != null)
                {
                    usu.Equipo = new Equipo(usu.Nombre, Equipo.RellenarEquipo(usu.Nombre));
                    RellenarUsuario(usu);
                }

            }
            else
            {
                Console.Clear();
                usu = RegistroUsuario();
                string nombreUsuario = usu.Nombre;
                string equipoInicial = ElegirEquipoInicial();
                CopiarEquipoInicial(equipoInicial, nombreUsuario);
                if (usu != null)
                {
                    usu.Equipo = new Equipo(usu.Nombre, Equipo.RellenarEquipo(usu.Nombre));
                }
                usu.ActualizarFicheroDatos();
            }
            Console.Clear();
            return usu;
        }
    }
}
