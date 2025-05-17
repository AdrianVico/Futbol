using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        }

        public static void SalirMenu(Menu m)
        {
            m.MostrarMenuPrincipal();
        }

        public void MostrarMenuPrincipal()
        {
            string[] opcionesMenu = { "Mercado", "Equipo", "Liga", "Jornada", "Salir" };
            switch (Menu.MakeMenuOfHorizontal(new List<string> { "Selecciona una opción:" }, new List<string>(opcionesMenu)))
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
                    //salir = true;
                    break;
            }
        }
        public static int DibujarCuadro(List<string> lineas)
        {
            const int ANCHO_CONSOLA = 209;
            const int ALTO_CONSOLA = 51;

            int anchoCuadro = 70;
            int altoCuadro = 51;

            int padLeft = (ANCHO_CONSOLA - anchoCuadro) / 2; // 69
            int padTop = 0;

            // Agregar líneas de transición
            for (int y = 0; y < altoCuadro; y++)
            {
                for (int x = 0; x < anchoCuadro; x++)
                {
                    Console.SetCursorPosition(padLeft + x, padTop + y);
                    Console.Write("█");
                }
            }
            // Dibuja cuadro completo
            for (int y = 0; y < altoCuadro; y++)
            {
                for (int x = 0; x < anchoCuadro; x++)
                {
                    Console.SetCursorPosition(padLeft + x, padTop + y);
                    if (y == 0 || y == altoCuadro - 1 || x == 0 || x == anchoCuadro - 1)
                        Console.Write("█");
                    else
                        Console.Write(" ");
                }
            }

            // Imprimir líneas centradas dentro del cuadro
            int altoTexto = altoCuadro - 2; // espacio entre los bordes
            int padTopTexto = padTop + 1 + (altoTexto - lineas.Count) / 2;

            for (int i = 0; i < lineas.Count && i < altoTexto; i++)
            {
                string linea = lineas[i];
                if (linea.Length > anchoCuadro - 2)
                    linea = linea.Substring(0, anchoCuadro - 2); // cortar si es muy larga

                int anchoTexto = anchoCuadro - 2; // sin los bordes
                int padLeftTexto = (anchoTexto - linea.Length) / 2;

                int xTexto = padLeft + 1 + padLeftTexto;
                int yTexto = padTopTexto + i;

                Console.SetCursorPosition(xTexto, yTexto);
                Console.Write(linea);
            }
            return padTopTexto;
        }
        public static int MakeMenuOf(List<string> preguntasTexto, List<string> opciones, string textoAdicional = "Usa las flechitas para navegar y Enter para seleccionar")
        {
            const int ANCHO_CONSOLA = 209;
            const int ALTO_CONSOLA = 51;

            //hacer las opciones para que se cuadren bien
            int longitudMaxima = opciones.Max(o => o.Length) + (opciones.Max(o => o.Length) % 2 == 0 ? 1 : 0);
            opciones = opciones.Select(o => o + new string(' ', longitudMaxima - o.Length)).ToList();

            // Crear las líneas para el menú completo
            List<string> lineasMenu = new List<string>();

            // Agregar las preguntas al menú
            lineasMenu.AddRange(preguntasTexto);

            // Agregar una línea en blanco entre las preguntas y las opciones
            lineasMenu.Add("");

            // Agregar las opciones al menú
            lineasMenu.AddRange(opciones);

            // Agregar una línea en blanco y el texto adicional
            lineasMenu.Add("");
            lineasMenu.Add(textoAdicional);

            // Dibujar el cuadro inicial con todo el contenido
            int padTopTexto = DibujarCuadro(lineasMenu);

            // Calcular posiciones para las opciones
            int maxLargo = opciones.Max(o => o.Length);
            int POSICION_X_OPCIONES = (ANCHO_CONSOLA / 2) - (maxLargo / 2) - 2; // Centrado aproximado
            int POSICION_Y_PRIMERA_OPCION = padTopTexto + preguntasTexto.Count + 1; // Después de las líneas de preguntas

            int opcionSeleccionada = 0;
            int opcionAnterior = -1;
            ConsoleKey tecla;

            // Calcular la posición Y de cada opción en la pantalla
            int[] posicionesY = new int[opciones.Count];
            for (int i = 0; i < opciones.Count; i++)
            {
                posicionesY[i] = POSICION_Y_PRIMERA_OPCION + i;
            }

            // Marcar inicialmente la primera opción (seleccionada por defecto)
            Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(">" + opciones[0]);
            Console.ResetColor();

            do
            {
                // Si la opción cambió, actualizamos solo las líneas necesarias
                if (opcionAnterior != opcionSeleccionada)
                {
                    // Actualizar opción anterior (quitar el cursor y restaurar color)
                    if (opcionAnterior >= 0)
                    {
                        Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[opcionAnterior]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" " + opciones[opcionAnterior]);
                        Console.ResetColor();
                    }

                    // Actualizar opción actual (poner el cursor y colorear de verde)
                    Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[opcionSeleccionada]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(">" + opciones[opcionSeleccionada]);
                    Console.ResetColor();

                    opcionAnterior = opcionSeleccionada;
                }

                // Leer la tecla presionada
                tecla = Console.ReadKey(true).Key;

                // Actualizar la opción seleccionada según la tecla
                switch (tecla)
                {
                    case ConsoleKey.UpArrow:
                        opcionSeleccionada = (opcionSeleccionada > 0) ? opcionSeleccionada - 1 : opciones.Count - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        opcionSeleccionada = (opcionSeleccionada < opciones.Count - 1) ? opcionSeleccionada + 1 : 0;
                        break;
                }
            } while (tecla != ConsoleKey.Enter);

            return opcionSeleccionada;
        }

        public static int MakeMenuOfHorizontal(List<string> preguntasTexto, List<string> opciones, string textoAdicional = "Usa las flechitas para navegar y Enter para seleccionar")
        {
            const int ANCHO_CONSOLA = 209;
            const int ALTO_CONSOLA = 51;

            // Asegurarse de que todas las opciones tengan el mismo largo para alinearlas mejor
            int longitudMaxima = opciones.Max(o => o.Length) + (opciones.Max(o => o.Length) % 2 == 0 ? 1 : 0);
            opciones = opciones.Select(o => o + new string(' ', longitudMaxima - o.Length)).ToList();

            // Crear las líneas para el menú completo
            List<string> lineasMenu = new List<string>();

            // Agregar las preguntas al menú
            lineasMenu.AddRange(preguntasTexto);

            // Línea en blanco
            lineasMenu.Add("");

            // Texto adicional
            lineasMenu.Add("");
            lineasMenu.Add(textoAdicional);

            // Dibujar el cuadro inicial con todo el contenido excepto las opciones
            int padTopTexto = DibujarCuadro(lineasMenu);

            // Posición vertical para las opciones (una sola línea)
            int POSICION_Y_OPCIONES = padTopTexto + preguntasTexto.Count + 1;

            // Calcular posiciones horizontales para cada opción
            int espacioEntre = 4;
            int totalAnchoOpciones = opciones.Count * (longitudMaxima + espacioEntre) - espacioEntre;
            int inicioX = (ANCHO_CONSOLA - totalAnchoOpciones) / 2;

            int[] posicionesX = new int[opciones.Count];
            for (int i = 0; i < opciones.Count; i++)
            {
                posicionesX[i] = inicioX + i * (longitudMaxima + espacioEntre);
            }

            // Mostrar todas las opciones inicialmente
            for (int i = 0; i < opciones.Count; i++)
            {
                Console.SetCursorPosition(posicionesX[i], POSICION_Y_OPCIONES);
                Console.Write(" " + opciones[i]); // espacio en lugar de flecha
            }

            int opcionSeleccionada = 0;
            int opcionAnterior = -1;
            ConsoleKey tecla;

            // Marcar inicialmente la primera opción
            Console.SetCursorPosition(posicionesX[0], POSICION_Y_OPCIONES);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(">" + opciones[0]);
            Console.ResetColor();

            do
            {
                if (opcionAnterior != opcionSeleccionada)
                {
                    // Restaurar opción anterior
                    if (opcionAnterior >= 0)
                    {
                        Console.SetCursorPosition(posicionesX[opcionAnterior], POSICION_Y_OPCIONES);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" " + opciones[opcionAnterior]);
                        Console.ResetColor();
                    }

                    // Resaltar opción nueva
                    Console.SetCursorPosition(posicionesX[opcionSeleccionada], POSICION_Y_OPCIONES);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(">" + opciones[opcionSeleccionada]);
                    Console.ResetColor();

                    opcionAnterior = opcionSeleccionada;
                }

                // Leer tecla
                tecla = Console.ReadKey(true).Key;

                switch (tecla)
                {
                    case ConsoleKey.LeftArrow:
                        opcionSeleccionada = (opcionSeleccionada > 0) ? opcionSeleccionada - 1 : opciones.Count - 1;
                        break;
                    case ConsoleKey.RightArrow:
                        opcionSeleccionada = (opcionSeleccionada < opciones.Count - 1) ? opcionSeleccionada + 1 : 0;
                        break;
                }

            } while (tecla != ConsoleKey.Enter);

            return opcionSeleccionada;
        }



        public static Usuario Iniciar()
        {
            PantallaInicio();
            return InicioSesion.Inicio(MenuIniciarSesion());
        }
        private static bool MenuIniciarSesion()
        {
            List<string> pregunta = new List<string> { "Tienes una cuenta creada?" };
            List<string> opciones = new List<string> { "Si ", "No " };//si es par se le mete un espacio pq se necesita impar
            return MakeMenuOf(pregunta, opciones, "") == 0 ? true : false;
        }
        private void MostrarMenuMercado()
        {
            string[] opcionesMercado = { "Comprar", "Vender", "Volver" };

            switch (Menu.MakeMenuOf(new List<string> { "Mercado" }, new List<string>(opcionesMercado)))
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
                    //volver = true;
                    break;
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

        private static void PantallaInicio()//mejorarlo
        {
            string[] lineasTitulo = {
@"███████╗ █████╗ ███╗   ██╗████████╗ █████╗ ███████╗██╗   ██╗    ██╗     ███████╗ ██████╗ ███████╗███╗   ██╗██████╗ ███████╗",
@"██╔════╝██╔══██╗████╗  ██║╚══██╔══╝██╔══██╗██╔════╝╚██╗ ██╔╝    ██║     ██╔════╝██╔════╝ ██╔════╝████╗  ██║██╔══██╗██╔════╝",
@"█████╗  ███████║██╔██╗ ██║   ██║   ███████║███████╗ ╚████╔╝     ██║     █████╗  ██║  ███╗█████╗  ██╔██╗ ██║██║  ██║███████╗",
@"██╔══╝  ██╔══██║██║╚██╗██║   ██║   ██╔══██║╚════██║  ╚██╔╝      ██║     ██╔══╝  ██║   ██║██╔══╝  ██║╚██╗██║██║  ██║╚════██║",
@"██║     ██║  ██║██║ ╚████║   ██║   ██║  ██║███████║   ██║       ███████╗███████╗╚██████╔╝███████╗██║ ╚████║██████╔╝███████║",
@"╚═╝     ╚═╝  ╚═╝╚═╝  ╚═══╝   ╚═╝   ╚═╝  ╚═╝╚══════╝   ╚═╝       ╚══════╝╚══════╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═════╝ ╚══════╝" };
            string[] lineasCosa = {
@" ___   _   _   _      ___     _       ___   _  _   _____   ___    ___      ___     _     ___     _        ___    ___    _  _   _____   ___   _  _   _   _     _     ___  ",
@"| _ \ | | | | | |    / __|   /_\     |_ _| | \| | |_   _| | _ \  / _ \    | _ \   /_\   | _ \   /_\      / __|  / _ \  | \| | |_   _| |_ _| | \| | | | | |   /_\   | _ \ ",
@"|  _/ | |_| | | |__  \__ \  / _ \     | |  | .` |   | |   |   / | (_) |   |  _/  / _ \  |   /  / _ \    | (__  | (_) | | .` |   | |    | |  | .` | | |_| |  / _ \  |   / ",
@"|_|    \___/  |____| |___/ /_/ \_\   |___| |_|\_|   |_|   |_|_\  \___/    |_|   /_/ \_\ |_|_\ /_/ \_\    \___|  \___/  |_|\_|   |_|   |___| |_|\_|  \___/  /_/ \_\ |_|_\ "};
            string[] lineasCosa2 = {
@"                                                                                                                                                                         ",
@"                                                                                                                                                                         ",
@"                                                                                                                                                                         ",
@"                                                                                                                                                                         "};

            // Obtener ancho total de la consola
            int anchoConsola = 209;
            bool mostrarCosa = true;
            do
            {
                Console.Clear();
                // Mostrar título centrado
                foreach (string linea in lineasTitulo)
                {
                    int padding = (anchoConsola - linea.Length) / 2;
                    Console.WriteLine(new string(' ', padding) + linea);
                }
                // Mostrar o no mostrar "cosa" (parpadeo)
                if (mostrarCosa)
                {
                    foreach (string linea in lineasCosa)
                    {
                        int padding = (anchoConsola - linea.Length) / 2;
                        Console.WriteLine(new string(' ', padding) + linea);
                    }
                }
                else
                {
                    foreach (string linea in lineasCosa2)
                    {
                        int padding = (anchoConsola - linea.Length) / 2;
                        Console.WriteLine(new string(' ', padding) + linea);
                    }
                }
                // Esperar antes del siguiente parpadeo
                Thread.Sleep(750);
                // Alternar para crear efecto de parpadeo
                mostrarCosa = !mostrarCosa;
            } while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter);
            Console.Clear();
            for (int i = 0; i < 51; i++)
            {
                for (int j = 0; j < 209; j++)
                {
                    Console.SetCursorPosition(j, i);
                    Console.WriteLine("█");
                }
            }
            Console.Clear();
        }
        
    }
}