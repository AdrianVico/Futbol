using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Futbol
{
    internal class Menu
    {
        Usuario usuario;
        Mercado mercado;
        Liga liga;
        Partido partido;

        internal Usuario Usuario { get => usuario; set => usuario = value; }
        internal Mercado Mercado { get => mercado; set => mercado = value; }

        public Menu(Usuario usuario)
        {
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
            string[] opcionesMenu = { "Mercado", "Equipo", "Jornada", "Liga", "Penaltis", "Salir" };
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
                    penaltis();
                    break;
                case 5:
                    DibujarCuadro(new List<string> { "Gracias por jugar!", "Pulsa Intro para salir..." });
                    ConsoleKey key;
                    do
                    {
                        key = Console.ReadKey(true).Key;
                    } while (key != ConsoleKey.Enter);
                    break;
            }
        }

        public void penaltis()
        {
            long dinero = usuario.Dinero;
            bool correcto = false;
            long cantidadApostada;
            do
            {
                List<string> mensajeDinero = new List<string>
                    {
                        "PENALTIS",
                        "",
                        "Cuanto quieres apostar ( Max: "+dinero+" )",
                        "---------------------------",
                       "|                          $|",
                        "---------------------------"
                    };

                int padTopTexto = Menu.DibujarCuadro(mensajeDinero);

                int posicionX = (209 - 29) / 2;
                int posicionY = padTopTexto + mensajeDinero.Count - 2;

                Console.SetCursorPosition(posicionX, posicionY);
                string cantidadApostadaString = Console.ReadLine() ?? "0";
                correcto = long.TryParse(cantidadApostadaString, out cantidadApostada) && cantidadApostada > 0 && cantidadApostada <= dinero;
                if (!correcto)
                {
                    Menu.DibujarCuadro(new List<string> { "ERROR :(", "Ingreso no valido" });
                    Console.ReadKey();
                }
            } while (!correcto);
            bool gol = jugarPenaltis();
            usuario.Dinero -= cantidadApostada;
            usuario.Dinero += gol ? (long)(cantidadApostada * 1.5) : 0;
            usuario.ActualizarFicheroDatos();
            List<string> mensaje = new List<string> {
                @"                     ___    ",
                @" o__        o__     |   |\  ",
                @"/|          /\      |   |X\ ",
                @"/ > o        <\     |   |XX\",
                @"^^^^^^^^^^^^^^^^^^^^^^^^^^^^",
                "",
            };
            mensaje.AddRange(new List<string> { gol ? "GOOOOOOOOOOOOOLLL!!!" : "MALA PUNTERIA :(", gol ? "HAS GANADO:" : "HAS PERDIDO:", gol ? (cantidadApostada * 1.5).ToString() + "$" : cantidadApostada.ToString() + "$" });
            Menu.DibujarCuadro(mensaje);
            Console.ReadKey();
            MostrarMenuPrincipal();
        }
        public static bool jugarPenaltis()
        {
            Console.CursorVisible = false;
            List<string> mensajes = new List<string>
            {
                "¡Apunta bien!",
                "Solo los valientes marcan.",
                "Demuestra tu puntería.",
                "¡Este gol es tuyo!",
                "Golpea con precisión.",
                "¿Tienes lo que se necesita?",
                "Reta al portero.",
                "¡Solo los cracks marcan aquí!",
                "¡Atrévete a disparar!",
                "¿Podrás con el portero?",
                "¡Dispara al destino!",
                "¡La gloria te espera!",
                "¡Momento de la verdad!",
                "¡Es ahora o nunca!",
                "¡Gol de campeonato!"
            };

            Random random = new Random();
            string titulo = mensajes[random.Next(mensajes.Count)];

            List<string> porteria = new List<string>
            {
                titulo,
                "",
                "╔══════════════════════════════════════════════════════════╗",
                "║ ╔══════════════════════════════════════════════════════╗ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
                "║ ║                                                      ║ ║",
               @"/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\/\",
            };
            int padTop = DibujarCuadro(porteria)+2;
            int padLeft = 74;

            int[][] posiciones = new int[6][]
            {
                new int[] { 5, 3 },    // Arriba Izquierda
                new int[] { 27, 3 },   // Arriba Centro
                new int[] { 50, 3 },   // Arriba Derecha
                new int[] { 5, 11 },   // Abajo Izquierda
                new int[] { 27, 11 },  // Abajo Centro
                new int[] { 50, 11 }   // Abajo Derecha
            };

            bool[] paradas = new bool[6];

            int seleccionado = 0;
            ConsoleKey tecla;

            do
            {
                for (int i = 0; i < 6; i++)
                {
                    int x = posiciones[i][0];
                    int y = posiciones[i][1];
                    if (i == seleccionado)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.SetCursorPosition(padLeft + x, padTop + y);
                        Console.Write("╔═══╗");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 1);
                        Console.Write("║ + ║");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 2);
                        Console.Write("╚═══╝");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.SetCursorPosition(padLeft + x, padTop + y);
                        Console.Write("╔═══╗");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 1);
                        Console.Write("║   ║");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 2);
                        Console.Write("╚═══╝");
                        Console.ResetColor();
                    }
                }

                tecla = Console.ReadKey(true).Key;

                // Movimiento: 0 1 2 (arriba), 3 4 5 (abajo)
                switch (tecla)
                {
                    case ConsoleKey.LeftArrow:
                        if (seleccionado != 0 && seleccionado != 3) seleccionado--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (seleccionado != 2 && seleccionado != 5) seleccionado++;
                        break;
                    case ConsoleKey.UpArrow:
                        if (seleccionado > 2) seleccionado -= 3;
                        break;
                    case ConsoleKey.DownArrow:
                        if (seleccionado <= 2) seleccionado += 3;
                        break;
                }
            } while (tecla != ConsoleKey.Enter);
            Thread.Sleep(500);

            int parada = random.Next(1, 4);

            if (parada == 1)
            {
                // Parar una columna aleatoria (0=izq, 1=centro, 2=dcha)
                int colParada = random.Next(0, 3);
                List<int> posicionesParadas = new List<int> { colParada, 3 + colParada };

                for (int i = 0; i < 6; i++)
                {
                    if (posicionesParadas.Contains(i))
                    {
                        int x = posiciones[i][0];
                        int y = posiciones[i][1];
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(padLeft + x, padTop + y);
                        Console.Write("╔═══╗");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 1);
                        Console.Write("║ X ║");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 2);
                        Console.Write("╚═══╝");
                        Console.ResetColor();
                        paradas[i] = true;
                        Thread.Sleep(500);
                    }
                }
            }
            else if (parada == 2)
            {
                // Parar dos columnas aleatorias
                int col1 = random.Next(0, 3);
                int col2;
                do
                {
                    col2 = random.Next(0, 3);
                } while (col2 == col1);
                List<int> posicionesParadas = new List<int> { col1, 3 + col1, col2, 3 + col2 };

                for (int i = 0; i < 6; i++)
                {
                    if (posicionesParadas.Contains(i))
                    {
                        int x = posiciones[i][0];
                        int y = posiciones[i][1];
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(padLeft + x, padTop + y);
                        Console.Write("╔═══╗");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 1);
                        Console.Write("║ X ║");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 2);
                        Console.Write("╚═══╝");
                        Console.ResetColor();
                        paradas[i] = true;
                        Thread.Sleep(500);
                    }
                }
            }
            else if (parada == 3)
            {
                // Parar una fila aleatoria (0=arriba, 1=abajo)
                int filaParada = random.Next(0, 2);
                List<int> posicionesParadas = new List<int> { filaParada * 3 , filaParada * 3 + 1, filaParada * 3 + 2 };

                for (int i = 0; i < 6; i++)
                {
                    if (posicionesParadas.Contains(i))
                    {
                        int x = posiciones[i][0];
                        int y = posiciones[i][1];
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(padLeft + x, padTop + y);
                        Console.Write("╔═══╗");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 1);
                        Console.Write("║ X ║");
                        Console.SetCursorPosition(padLeft + x, padTop + y + 2);
                        Console.Write("╚═══╝");
                        Console.ResetColor();
                        paradas[i] = true;
                        Thread.Sleep(500);
                    }
                }
            }
            Thread.Sleep(1500);
            return !paradas[seleccionado];
        }



        public static int DibujarCuadro(List<string> lineas, Usuario usuario = null)
        {
            const int ANCHO_CONSOLA = 209;
            const int ALTO_CONSOLA = 51;

            int anchoCuadro = 70;
            int altoCuadro = 51;

            int padLeft = (ANCHO_CONSOLA - anchoCuadro) / 2; // 69
            int padTop = 0;

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

            int altoTexto = altoCuadro - 2;
            int padTopTexto = padTop + 1 + (altoTexto - lineas.Count) / 2;

            for (int i = 0; i < lineas.Count && i < altoTexto; i++)
            {
                string linea = lineas[i];
                if (linea.Length > anchoCuadro - 2)
                    linea = linea.Substring(0, anchoCuadro - 2);

                int anchoTexto = anchoCuadro - 2;
                int padLeftTexto = (anchoTexto - linea.Length) / 2;

                int xTexto = padLeft + 1 + padLeftTexto;
                int yTexto = padTopTexto + i;

                Console.SetCursorPosition(xTexto, yTexto);

                if (usuario != null && linea.Contains(usuario.Nombre))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write(linea);
                Console.ResetColor();
            }
            return padTopTexto;
        }
        public static int CrearMenuVertical(List<string> preguntasTexto, List<string> opciones, string textoAdicional = "Usa las flechitas para navegar y Enter para seleccionar")
        {
            const int ANCHO_CONSOLA = 209;
            const int ALTO_CONSOLA = 51;

            int longitudMaxima = opciones.Max(o => o.Length) + (opciones.Max(o => o.Length) % 2 == 0 ? 1 : 0);
            opciones = opciones.Select(o => o + new string(' ', longitudMaxima - o.Length)).ToList();

            List<string> lineasMenu = new List<string>();

            lineasMenu.AddRange(preguntasTexto);

            lineasMenu.Add("");

            lineasMenu.AddRange(opciones);

            lineasMenu.Add("");
            lineasMenu.Add(textoAdicional);

            int padTopTexto = DibujarCuadro(lineasMenu);

            int maxLargo = opciones.Max(o => o.Length);
            int POSICION_X_OPCIONES = (ANCHO_CONSOLA / 2) - (maxLargo / 2) - 2;
            int POSICION_Y_PRIMERA_OPCION = padTopTexto + preguntasTexto.Count + 1;

            int opcionSeleccionada = 0;
            int opcionAnterior = -1;
            ConsoleKey tecla;

            int[] posicionesY = new int[opciones.Count];
            for (int i = 0; i < opciones.Count; i++)
            {
                posicionesY[i] = POSICION_Y_PRIMERA_OPCION + i;
            }

            Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(">" + opciones[0]);
            Console.ResetColor();

            do
            {
                if (opcionAnterior != opcionSeleccionada)
                {
                    if (opcionAnterior >= 0)
                    {
                        Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[opcionAnterior]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" " + opciones[opcionAnterior]);
                        Console.ResetColor();
                    }

                    Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[opcionSeleccionada]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(">" + opciones[opcionSeleccionada]);
                    Console.ResetColor();

                    opcionAnterior = opcionSeleccionada;
                }

                tecla = Console.ReadKey(true).Key;

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

        public static int CrearMenuVerticalUsuario(List<string> preguntasTexto, List<string> opciones, Usuario usuario, string textoAdicional = "Usa las flechitas para navegar y Enter para seleccionar")
        {
            const int ANCHO_CONSOLA = 209;
            const int ALTO_CONSOLA = 51;

            int longitudMaxima = opciones.Max(o => o.Length) + (opciones.Max(o => o.Length) % 2 == 0 ? 1 : 0);
            opciones = opciones.Select(o => o + new string(' ', longitudMaxima - o.Length)).ToList();

            List<string> lineasMenu = new List<string>();

            lineasMenu.AddRange(preguntasTexto);

            lineasMenu.Add("");

            lineasMenu.AddRange(opciones);

            lineasMenu.Add("");
            lineasMenu.Add(textoAdicional);

            int padTopTexto = DibujarCuadro(lineasMenu);

            MostrarInfoUsuario(usuario);

            int maxLargo = opciones.Max(o => o.Length);
            int POSICION_X_OPCIONES = (ANCHO_CONSOLA / 2) - (maxLargo / 2) - 2;
            int POSICION_Y_PRIMERA_OPCION = padTopTexto + preguntasTexto.Count + 1;

            int opcionSeleccionada = 0;
            int opcionAnterior = -1;
            ConsoleKey tecla;

            int[] posicionesY = new int[opciones.Count];
            for (int i = 0; i < opciones.Count; i++)
            {
                posicionesY[i] = POSICION_Y_PRIMERA_OPCION + i;
            }

            Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[0]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(">" + opciones[0]);
            Console.ResetColor();

            do
            {
                if (opcionAnterior != opcionSeleccionada)
                {
                    if (opcionAnterior >= 0)
                    {
                        Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[opcionAnterior]);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" " + opciones[opcionAnterior]);
                        Console.ResetColor();
                    }

                    Console.SetCursorPosition(POSICION_X_OPCIONES, posicionesY[opcionSeleccionada]);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(">" + opciones[opcionSeleccionada]);
                    Console.ResetColor();

                    opcionAnterior = opcionSeleccionada;
                }

                tecla = Console.ReadKey(true).Key;

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

            List<string> lineasMenu = new List<string>();

            lineasMenu.AddRange(preguntasTexto);

            lineasMenu.Add("");

            lineasMenu.Add("");
            lineasMenu.Add("");
            lineasMenu.Add("");
            lineasMenu.AddRange(textoAdicional);

            int padTopTexto = DibujarCuadro(lineasMenu);

            MostrarInfoUsuario(usuario);

            int POSICION_Y_OPCIONES = padTopTexto + preguntasTexto.Count + 1;

            int espacioEntre = 2;
            int totalAnchoOpciones = opciones.Count * (longitudMaxima + espacioEntre) - espacioEntre;
            int inicioX = (ANCHO_CONSOLA - totalAnchoOpciones) / 2;

            int[] posicionesX = new int[opciones.Count];
            for (int i = 0; i < opciones.Count; i++)
            {
                posicionesX[i] = inicioX + i * (longitudMaxima + espacioEntre);
            }

            for (int i = 0; i < opciones.Count; i++)
            {
                Console.SetCursorPosition(posicionesX[i], POSICION_Y_OPCIONES);
                Console.Write(" " + opciones[i]);
            }

            int opcionSeleccionada = 0;
            int opcionAnterior = -1;
            ConsoleKey tecla;

            Console.SetCursorPosition(posicionesX[0], POSICION_Y_OPCIONES);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(">" + opciones[0]);
            Console.ResetColor();

            do
            {
                if (opcionAnterior != opcionSeleccionada)
                {
                    if (opcionAnterior >= 0)
                    {
                        Console.SetCursorPosition(posicionesX[opcionAnterior], POSICION_Y_OPCIONES);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(" " + opciones[opcionAnterior]);
                        Console.ResetColor();
                    }

                    Console.SetCursorPosition(posicionesX[opcionSeleccionada], POSICION_Y_OPCIONES);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(">" + opciones[opcionSeleccionada]);
                    Console.ResetColor();

                    opcionAnterior = opcionSeleccionada;
                }

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

        public static void MostrarInfoUsuario(Usuario usuario)
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
            List<string> opciones = new List<string> { "Si ", "No " };
            return CrearMenuVertical(pregunta, opciones, "") == 0 ? true : false;
        }
        public void MostrarMenuMercado()
        {
            string[] opcionesMercado = { "Comprar", "Vender", "Volver" };

            switch (Menu.CrearMenuVerticalUsuario(new List<string> { "Mercado" }, new List<string>(opcionesMercado), usuario))
            {
                case 0:
                    Console.Clear();
                    mercado.IniciarMercado();
                    ConsoleKey key;
                    do
                    {
                        key = Console.ReadKey(true).Key;
                    } while (key != ConsoleKey.Enter);
                    break;
                case 1:
                    Console.Clear();
                    mercado.Vender();
                    do
                    {
                        key = Console.ReadKey(true).Key;
                    } while (key != ConsoleKey.Enter);
                    break;
                case 2:
                    MostrarMenuPrincipal();
                    break;
            }
        }
        private void MostrarMenuEquipo()
        {
            string[] opcionesEquipo = { "Ver equipo", "Modificar alineación", "Volver" };
            int indice = Menu.CrearMenuVertical(new List<string> { "Plantilla" }, new List<string>(opcionesEquipo));

            switch (indice)
            {
                case 0:
                    Console.Clear();
                    DibujarCuadro(MostrarEquipo());
                    ConsoleKey key;
                    do
                    {
                        key = Console.ReadKey(true).Key;
                    } while (key != ConsoleKey.Enter);
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
            Console.Clear();
            partido.AnyadirPartidos();
            partido.AnyadirEquiposAPartidos();
            partido.MostrarNumeroJornada();

            List<string> partidosTexto = partido.ObtenerPartidosComoTexto();
            DibujarCuadro(partidosTexto, usuario);

            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Enter);

            partido.GuardarPartidos();

            if (liga.ComprobarFinal(usuario) == 8)
            {
                string archivo = $"../../../Usuarios/{usuario.Nombre}/numeroJornada.txt";
                string archivoLiga = $"../../../Usuarios/{usuario.Nombre}/jornadas.txt";
                liga.CargarJornadas(archivoLiga);

                int puntosUsuario = 0;
                if (liga.PuntosPorEquipo.ContainsKey(usuario.Equipo.Nombre))
                {
                    puntosUsuario = liga.PuntosPorEquipo[usuario.Equipo.Nombre];
                }

                long dineroGanado = puntosUsuario * 1000000;
                usuario.Dinero += dineroGanado;
                usuario.ActualizarFicheroDatos();

                DibujarCuadro(liga.GanadorLiga());

                do
                {
                    key = Console.ReadKey(true).Key;
                } while (key != ConsoleKey.Enter);

                DibujarCuadro(new List<string> {
                    $"¡Has ganado {dineroGanado:N0}$ por tus {puntosUsuario} puntos!",
                    "Pulsa Intro tecla para continuar..."
                });

                File.WriteAllText(archivo, "0");
                File.WriteAllText(archivoLiga, "");
            }

            partido.Partidos.Clear();
            partido.ResultadosPorEquipo.Clear();

            do
            {
                key = Console.ReadKey(true).Key;
            } while (key != ConsoleKey.Enter);

            MostrarMenuPrincipal();
        }

        private void mostrarLiga()
        {
            string archivo = $"../../../Usuarios/{usuario.Nombre}/jornadas.txt";
            Console.Clear();

            liga.CargarJornadas(archivo);

            List<string> tablaComoTexto = liga.ObtenerTablaComoTexto();

            DibujarCuadro(tablaComoTexto, usuario);

            ConsoleKey tecla;
            do
            {
                tecla = Console.ReadKey(true).Key;
            } while (tecla != ConsoleKey.Enter);

            MostrarMenuPrincipal();
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
                    usuario.ActualizarFicheroDatos();
                    Console.Clear();
                }
                MostrarMenuPrincipal();
            }
        }
        private List<string> MostrarEquipo()
        {
            List<string> lineas = new List<string>();

            lineas.Add("LISTA DE JUGADORES");
            lineas.Add("");
            lineas.Add("Nombre".PadRight(20) + "Posición".PadRight(15) + "Equipo Origen".PadRight(20));
            lineas.Add(new string('-', 60));

            foreach (var j in usuario.Equipo.Jugadores)
            {
                string linea = j.Nombre.PadRight(20) + j.Posicion.PadRight(15) + j.EquipoOrigen.PadRight(20);
                lineas.Add(linea);
            }
            return lineas;
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

            int anchoConsola = 209;
            int altoConsola = 51;
            bool mostrarCosa = true;
            do
            {
                Console.Clear();
                for (int i = 0; i < lineasTitulo.Length; i++)
                {
                    Console.SetCursorPosition((anchoConsola - lineasTitulo[i].Length) / 2, i + ((altoConsola - (lineasTitulo.Length+lineasCosa.Length))/2));
                    Console.WriteLine(lineasTitulo[i]);
                }

                for (int i = 0; i < lineasCosa.Length; i++)
                {
                    Console.SetCursorPosition((anchoConsola - lineasCosa[i].Length) / 2, lineasTitulo.Length + i + ((altoConsola - (lineasTitulo.Length + lineasCosa.Length)) / 2));
                    if (mostrarCosa)
                        Console.WriteLine(lineasCosa[i]);
                    else
                        Console.WriteLine(new string(' ', lineasCosa[i].Length));
                }
                Thread.Sleep(750);
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