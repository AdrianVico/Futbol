﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Liga
    {
        Dictionary<Equipo, List<string>> resultados;
        Dictionary<Equipo, int> puntos;
        int totalJornadas;
        string archivo = "jornadas.txt";

        public Liga()
        {
            resultados = new Dictionary<Equipo, List<string>>();
            puntos = new Dictionary<Equipo, int>();
            totalJornadas = 0;
        }

        public void CargarResultados()
        {
            if (!File.Exists(archivo))
            {
                Console.WriteLine("El archivo no se encontró.");
                return;
            }

            string[] lineas = File.ReadAllLines(archivo);
            int jornadaActual = 1;

            foreach (string linea in lineas)
            {
                if (linea.StartsWith("-"))
                {
                    jornadaActual++;
                }
                else
                {
                    string[] datos = linea.Split(';');
                    string nombreEquipo = datos[0];
                    string resultado = datos[1];
                    /*
                    if (!resultados.ContainsKey(nombreEquipo))
                    {
                        resultados[nombreEquipo] = new List<string>();
                        puntos[nombreEquipo] = 0;
                    }

                    resultados[nombreEquipo].Add(resultado);

                    string[] goles = resultado.Split('-');
                    int golesFavor = int.Parse(goles[0]);
                    int golesContra = int.Parse(goles[1]);

                    if (golesFavor > golesContra)
                        puntos[nombreEquipo] += 3;
                    else if (golesFavor == golesContra)
                        puntos[nombreEquipo] += 1;*/
                }
            }

            totalJornadas = CalcularTotalJornadas();
        }

        private int CalcularTotalJornadas()
        {
            int jornadas = 1;
            string[] lineas = File.ReadAllLines(archivo);

            foreach (string linea in lineas)
            {
                if (linea.StartsWith("-"))
                    jornadas++;
            }

            return jornadas;
        }

        public void MostrarClasificacion()
        {
            CargarResultados();
            Console.WriteLine("\nCLASIFICACIÓN GENERAL\n");
            Console.Write("Equipo".PadRight(15));

            for (int j = 1; j <= totalJornadas; j++)
                Console.Write($"J{j}   ");

            Console.WriteLine("PT");
            /*
            foreach (var equipo in resultados)
            {
                string nombre = equipo.Key;
                List<string> res = equipo.Value;

                Console.Write(nombre.PadRight(15));

                foreach (string r in res)
                    Console.Write($"{r.PadRight(5)}");

                for (int i = res.Count; i < totalJornadas; i++)
                    Console.Write("---- ");

                Console.WriteLine($"{puntos[nombre]}");
            }*/
        }

        public void MostrarJornadas()
        {
            if (!File.Exists(archivo))
            {
                Console.WriteLine("El archivo no se encontró.");
                return;
            }

            string[] lineas = File.ReadAllLines(archivo);
            int jornadaActual = 1;

            Console.WriteLine("\nRESULTADOS POR JORNADA:\n");

            foreach (string linea in lineas)
            {
                if (linea.StartsWith("-"))
                {
                    jornadaActual++;
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine($"J{jornadaActual}: {linea}");
                }
            }
        }
    }
}
