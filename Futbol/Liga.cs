using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Linq;

namespace Futbol
{
    internal class Liga
    {
        private Dictionary<string, List<string>> resultadosPorEquipo = new Dictionary<string, List<string>>();
        private Dictionary<string, int> puntosPorEquipo = new Dictionary<string, int>();
        public Liga()
        {
            this.ResultadosPorEquipo = resultadosPorEquipo;
            this.PuntosPorEquipo = puntosPorEquipo;
        }
        public Dictionary<string, List<string>> ResultadosPorEquipo { get => resultadosPorEquipo; set => resultadosPorEquipo = value; }
        public Dictionary<string, int> PuntosPorEquipo { get => puntosPorEquipo; set => puntosPorEquipo = value; }
        public void CargarJornadas(string rutaArchivo)
        {
            ResultadosPorEquipo.Clear();
            PuntosPorEquipo.Clear();

            if (!File.Exists(rutaArchivo))
            {
                Console.WriteLine("No hay archivo");
                return;
            }

            string[] lineas = File.ReadAllLines(rutaArchivo);
            List<string> jornadaActual = new List<string>();
            bool ultimaFueSeparador = false;

            for (int i = 0; i < lineas.Length; i++)
            {
                string lineaLimpia = lineas[i].Trim();

                if (lineaLimpia.StartsWith("-----"))
                {
                    if (jornadaActual.Count > 0)
                    {
                        ProcesarJornada(jornadaActual);
                        jornadaActual = new List<string>();
                    }
                    ultimaFueSeparador = true;
                }
                else if (lineaLimpia.Length > 0)
                {
                    jornadaActual.Add(lineaLimpia);
                    ultimaFueSeparador = false;
                }
            }

            if (jornadaActual.Count > 0 && !ultimaFueSeparador)
            {
                ProcesarJornada(jornadaActual);
            }
        }


        private void ProcesarJornada(List<string> jornada)
        {
            for (int i = 0; i < jornada.Count; i++)
            {
                string linea = jornada[i];
                string[] partes = linea.Split(';');

                if (partes.Length == 2)
                {
                    string equipo = partes[0];
                    string resultado = partes[1];

                    if (resultado.Contains("-"))
                    {
                        string[] goles = resultado.Split('-');

                        if (goles.Length == 2)
                        {
                            bool exito1 = int.TryParse(goles[0], out int golesFavor);
                            bool exito2 = int.TryParse(goles[1], out int golesContra);

                            if (exito1 && exito2)
                            {
                                if (!ResultadosPorEquipo.ContainsKey(equipo))
                                {
                                    ResultadosPorEquipo[equipo] = new List<string>();
                                    PuntosPorEquipo[equipo] = 0;
                                }

                                ResultadosPorEquipo[equipo].Add(resultado);

                                int puntos = 0;
                                if (golesFavor > golesContra)
                                {
                                    puntos = 3;
                                }
                                else if (golesFavor == golesContra)
                                {
                                    puntos = 1;
                                }

                                PuntosPorEquipo[equipo] += puntos;
                            }
                        }
                    }
                }
            }
        }
        public List<string> ObtenerTablaComoTexto()
        {
            List<string> lineas = new List<string>();
            lineas.Add("Equipo        Resultados por jornada                  Puntos");

            List<string> nombresEquipos = ResultadosPorEquipo.Keys.ToList();

            List<string> equiposOrdenados = nombresEquipos
                .OrderByDescending(nombre => PuntosPorEquipo.ContainsKey(nombre) ? PuntosPorEquipo[nombre] : 0)
                .ToList();

            foreach (string nombre in equiposOrdenados)
            {
                List<string> resultados = ResultadosPorEquipo[nombre];
                string resultadosTexto = string.Join(" | ", resultados);
                int puntos = PuntosPorEquipo[nombre];

                string linea = $"{nombre.PadRight(12)} {resultadosTexto.PadRight(40)} {puntos}";
                lineas.Add(linea);
            }

            return lineas;
        }

        public int ComprobarFinal(Usuario usuario)
        {
            string archivo = $"../../../Usuarios/{usuario.Nombre}/numeroJornada.txt";
            bool final = false;
            int numero = 0;
            if (File.Exists(archivo))
            {
                StreamReader sr = new StreamReader(archivo);
                string line = sr.ReadLine();   
                numero = Convert.ToInt32(line);
                sr.Close();
            }

            return numero;
        }

        public List<string> GanadorLiga()
        {
            List<string> lineas = new List<string>();

            List<string> nombresEquipos = ResultadosPorEquipo.Keys.ToList();

            List<string> equiposOrdenados = nombresEquipos
                .OrderByDescending(
                    nombre => PuntosPorEquipo.ContainsKey(nombre) ? PuntosPorEquipo[nombre] : 0
                )
                .ToList();

            if (equiposOrdenados.Count > 0)
            {
                string mejorEquipo = equiposOrdenados[0];

                List<string> resultados = ResultadosPorEquipo[mejorEquipo];
                string resultadosTexto = string.Join(" | ", resultados);
                int puntos = PuntosPorEquipo.ContainsKey(mejorEquipo) ? PuntosPorEquipo[mejorEquipo] : 0;

                string linea = mejorEquipo.PadRight(12) + " " +
                               resultadosTexto.PadRight(40) + " " +
                               puntos.ToString();

                lineas.Add(linea);
            }

            return lineas;
        }
    }
}
