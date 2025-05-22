using System;
using System.Collections.Generic;
using System.IO;

namespace Futbol
{
    internal class Liga
    {
        private Dictionary<string, List<string>> resultadosPorEquipo = new Dictionary<string, List<string>>();
        private Dictionary<string, int> puntosPorEquipo = new Dictionary<string, int>();

        public void CargarJornadas(string rutaArchivo)
        {
            resultadosPorEquipo.Clear();
            puntosPorEquipo.Clear();

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
                                if (!resultadosPorEquipo.ContainsKey(equipo))
                                {
                                    resultadosPorEquipo[equipo] = new List<string>();
                                    puntosPorEquipo[equipo] = 0;
                                }

                                resultadosPorEquipo[equipo].Add(resultado);

                                int puntos = 0;
                                if (golesFavor > golesContra)
                                {
                                    puntos = 3;
                                }
                                else if (golesFavor == golesContra)
                                {
                                    puntos = 1;
                                }

                                puntosPorEquipo[equipo] += puntos;
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

            List<string> nombresEquipos = resultadosPorEquipo.Keys.ToList();

            List<string> equiposOrdenados = nombresEquipos
                .OrderByDescending(nombre => puntosPorEquipo.ContainsKey(nombre) ? puntosPorEquipo[nombre] : 0)
                .ToList();

            foreach (string nombre in equiposOrdenados)
            {
                List<string> resultados = resultadosPorEquipo[nombre];
                string resultadosTexto = string.Join(" | ", resultados);
                int puntos = puntosPorEquipo[nombre];

                string linea = $"{nombre.PadRight(12)} {resultadosTexto.PadRight(40)} {puntos}";
                lineas.Add(linea);
            }

            return lineas;
        }


    }
}
