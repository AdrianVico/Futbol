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
        Liga liga;
        Partido partido;
        Jornada jornada;

        internal Usuario Usuario { get => usuario; set => usuario = value; }
        internal Mercado Mercado { get => mercado; set => mercado = value; }

        public Menu(Usuario usuario)
        {
            jornada = new Jornada();
            this.usuario = usuario;
            mercado = new Mercado(usuario);
            liga = new Liga();
            partido = new Partido(usuario);
        }

        public static void SalirMenu(Menu m)
        {
            m.MostrarMenuPrincipal();
        }

        public void MostrarMenuPrincipal()
        {
            string[] opcionesMenu = { "Mercado", "Equipo", "Jornada", "Liga", "Salir" };
            switch (Menu.CrearMenuPrincipal(new List<string> { "Selecciona una opción:" }, new List<string>(opcionesMenu), usuario, usuario.Equipo.MostrarCamisetas().ToList()))
            {
                case 0:
                    MostrarMenuMercado();
                    break;
                case 1:
                    MostrarMenuEquipo();
                    break;
                case 2:
                    jugarJornada();
                    break;
                case 3:
                    mostrarLiga();
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
        public static int CrearMenuVertical(List<string> preguntasTexto, List<string> opciones, string textoAdicional = "Usa las flechitas para navegar y Enter para seleccionar")
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

        public static int CrearMenuPrincipal(List<string> preguntasTexto, List<string> opciones, Usuario usuario, List<string> textoAdicional)
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
            lineasMenu.Add("");
            lineasMenu.Add("");
            lineasMenu.AddRange(textoAdicional);

            // Dibujar el cuadro inicial con todo el contenido excepto las opciones
            int padTopTexto = DibujarCuadro(lineasMenu);

            //poner info usuario
            MostrarInfoUsuario(usuario);

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

        private static void MostrarInfoUsuario(Usuario usuario)
        {
            Console.SetCursorPosition(70, 1);
            Console.WriteLine($"{usuario.Nombre} {usuario.Dinero}$ Pts: {usuario.Puntos}");
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
            return CrearMenuVertical(pregunta, opciones, "") == 0 ? true : false;
        }
        public void MostrarMenuMercado()
        {
            string[] opcionesMercado = { "Comprar", "Vender", "Volver" };

            switch (Menu.CrearMenuVertical(new List<string> { "Mercado" }, new List<string>(opcionesMercado)))
            {
                case 0:
                    Console.Clear();
                    mercado.IniciarMercado();
                    Console.ReadKey();
                    break;
                case 1:
                    Console.Clear();
                    mercado.Vender();
                    Console.ReadKey();
                    break;
                case 2:
                    MostrarMenuPrincipal();
                    break;
            }
        }
        private void MostrarMenuEquipo()
        {
            string[] opcionesEquipo = { "Ver equipo", "Modificar alineación", "Volver" };
            int indice = Menu.CrearMenuVertical(new List<string> {"Plantilla"}, new List<string>(opcionesEquipo));
            
            switch (indice)
            {
                case 0:
                    Console.Clear();
                    usuario.Equipo.Jugadores.ForEach(j => Console.WriteLine(j.ToString()));
                    Console.ReadKey();
                    Console.Clear();
                    MostrarMenuEquipo();
                    break;
                case 1:
                    ModificarAlineacion();
                    break;
                case 2:
                    MostrarMenuPrincipal();
                    break;
            } 
            
        }
        private void jugarJornada()
        {
            ConsoleKey tecla;

            Console.Clear();
            partido.AnyadirPartidos();
            partido.AnyadirEquiposAPartidos();
            partido.MostrarGuardarPartidos();
            jornada.MostrarPartidos();
            partido.Partidos.Clear();
            partido.ResultadosPorEquipo.Clear();

            tecla = Console.ReadKey(true).Key;
            if (tecla == ConsoleKey.Enter)
            {
                MostrarMenuPrincipal();
            }
        }
        private void mostrarLiga()
        {
            ConsoleKey tecla;

            Console.Clear();
            liga.MostrarClasificacion();

            tecla = Console.ReadKey(true).Key;
            if (tecla == ConsoleKey.Enter)
            {
                MostrarMenuPrincipal();
            }
        }

        private void ModificarAlineacion()
        {
            Console.Clear();
            List<string> opcionesAlineacion = new List<string>
           {
               "4-4-2",
               "4-3-3",
               "5-3-2",
               "4-2-4",
               "3-5-2",
               "5-4-1",
               "Volver"
           };
            int opcionSeleccionada = Menu.CrearMenuVertical(new List<string> { "Modificar alineación" }, opcionesAlineacion);

            if (opcionSeleccionada == 6)
            {
                MostrarMenuEquipo();
            }
            else
            {
                int[] nuevaAlineacion;
                switch (opcionSeleccionada)
                {
                    case 0:
                        nuevaAlineacion = new int[] { 1, 4, 4, 2 };
                        break;
                    case 1:
                        nuevaAlineacion = new int[] { 1, 4, 3, 3 };
                        break;
                    case 2:
                        nuevaAlineacion = new int[] { 1, 5, 3, 2 };
                        break;
                    case 3:
                        nuevaAlineacion = new int[] { 1, 4, 2, 4 };
                        break;
                    case 4:
                        nuevaAlineacion = new int[] { 1, 3, 5, 2 };
                        break;
                    case 5:
                        nuevaAlineacion = new int[] { 1, 5, 4, 1 };
                        break;
                    default:
                        nuevaAlineacion = null;
                        break;
                }
                if (nuevaAlineacion != null)
                {
                    usuario.Equipo.Alineacion = nuevaAlineacion;
                    Console.Clear();
                }
                MostrarMenuPrincipal();
            }
        }

        private static void PantallaInicio()
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

            // Obtener ancho total de la consola
            int anchoConsola = 209;
            int altoConsola = 51;
            bool mostrarCosa = true;
            do
            {
                Console.Clear();
                // Mostrar título centrado
                for (int i = 0; i < lineasTitulo.Length; i++)
                {
                    Console.SetCursorPosition((anchoConsola - lineasTitulo[i].Length) / 2, i + ((altoConsola - (lineasTitulo.Length+lineasCosa.Length))/2));
                    Console.WriteLine(lineasTitulo[i]);
                }
                // Mostrar o no mostrar "cosa" (parpadeo)

                for (int i = 0; i < lineasCosa.Length; i++)
                {
                    Console.SetCursorPosition((anchoConsola - lineasCosa[i].Length) / 2, lineasTitulo.Length + i + ((altoConsola - (lineasTitulo.Length + lineasCosa.Length)) / 2));
                    if (mostrarCosa)
                        Console.WriteLine(lineasCosa[i]);
                    else
                        Console.WriteLine(new string(' ', lineasCosa[i].Length));
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