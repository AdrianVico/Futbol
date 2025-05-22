using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Partido
    {
        private Random rand = new Random();
        private string resultado;
        private string archivo;
        private int numeroJornada;
        private string rutaNumeroJornada;
        private List<Equipo> equipos;
        private Dictionary<Equipo, Equipo> partidos;
        private Dictionary<Equipo, string> resultadosPorEquipo;
        private Usuario usuario;

        public Partido(Usuario usuario)
        {
            this.usuario = usuario;
            equipos = RellenarListaEquipos();
            partidos = new Dictionary<Equipo, Equipo>();
            resultadosPorEquipo = new Dictionary<Equipo, string>();

            archivo = $"../../../Usuarios/{usuario.Nombre}/jornadas.txt";
            rutaNumeroJornada = $"../../../Usuarios/{usuario.Nombre}/numeroJornada.txt";

            // Leer número de jornada desde archivo si existe
            if (File.Exists(rutaNumeroJornada))
            {
                string texto = File.ReadAllText(rutaNumeroJornada).Trim();
                int.TryParse(texto, out numeroJornada); // Si no es válido, será 0
            }
            else
            {
                numeroJornada = 0;
            }
        }

        public List<Equipo> RellenarListaEquipos()
        {
            List<Equipo> equipos = new List<Equipo>();
            equipos.Add(new Equipo(usuario == null ? "salami" : usuario.Nombre));
            equipos.Add(new Equipo("Equipo2"));
            equipos.Add(new Equipo("Equipo3"));
            equipos.Add(new Equipo("Equipo4"));
            equipos.Add(new Equipo("Equipo5"));
            equipos.Add(new Equipo("Equipo6"));
            equipos.Add(new Equipo("Equipo7"));
            equipos.Add(new Equipo("Equipo8"));
            equipos.Add(new Equipo("Equipo9"));
            equipos.Add(new Equipo("Equipo10"));
            equipos.Add(new Equipo("Equipo11"));
            equipos.Add(new Equipo("Equipo12"));
            return equipos;
        }

        public string Resultado { get => resultado; set => resultado = value; }
        public int NumeroJornada { get => numeroJornada; set => numeroJornada = value; }
        internal List<Equipo> Equipos { get => equipos; set => equipos = value; }
        internal Dictionary<Equipo, Equipo> Partidos { get => partidos; set => partidos = value; }
        internal Dictionary<Equipo, string> ResultadosPorEquipo { get => resultadosPorEquipo; set => resultadosPorEquipo = value; }

        public string SimularResultado()
        {
            int goles1 = rand.Next(0, 9);
            int goles2 = rand.Next(0, 9);
            string resultadoSimulado = goles1 + "-" + goles2;
            return resultadoSimulado;
        }

        public void AnyadirPartidos()
        {
            List<Equipo> clubes = new List<Equipo>(equipos);

            while (clubes.Count >= 2)
            {
                int indice1 = rand.Next(clubes.Count);
                Equipo local = clubes[indice1];
                clubes.RemoveAt(indice1);

                int indice2 = rand.Next(clubes.Count);
                Equipo visitante = clubes[indice2];
                clubes.RemoveAt(indice2);

                partidos.Add(local, visitante);
            }
        }

        public void AnyadirEquiposAPartidos()
        {
            foreach (KeyValuePair<Equipo, Equipo> emparejamiento in partidos)
            {
                string resultadoPartido = SimularResultado();
                string[] partes = resultadoPartido.Split('-');
                string invertido = partes[1] + "-" + partes[0];

                resultadosPorEquipo.Add(emparejamiento.Key, resultadoPartido);
                resultadosPorEquipo.Add(emparejamiento.Value, invertido);
            }
        }

        public void MostrarNumeroJornada()
        {
            numeroJornada++;
            GuardarNumeroJornada(); // Guardar el nuevo número
            string titulo = "Partidos de la J" + numeroJornada;
            int posX = (Console.WindowWidth - titulo.Length) / 2;
            Console.SetCursorPosition(posX, Console.CursorTop);
            Console.WriteLine(titulo);
        }

        private void GuardarNumeroJornada()
        {
            try
            {
                File.WriteAllText(rutaNumeroJornada, numeroJornada.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Error al guardar el número de jornada: " + e.Message);
            }
        }

        public List<string> ObtenerPartidosComoTexto()
        {
            List<string> lineas = new List<string>();

            lineas.Add($"Jornada {numeroJornada}");

            foreach (KeyValuePair<Equipo, Equipo> emparejamiento in partidos)
            {
                string resultadoLocal = resultadosPorEquipo[emparejamiento.Key];
                string resultadoVisitante = resultadosPorEquipo[emparejamiento.Value];

                string linea = emparejamiento.Key.Nombre.PadRight(10)
                             + resultadoLocal.PadLeft(5).PadRight(9)
                             + emparejamiento.Value.Nombre.PadLeft(10);

                lineas.Add(linea);
            }

            return lineas;
        }
        public void GuardarPartidos()
        {
            using (StreamWriter stw = new StreamWriter(archivo, true))
            {
                stw.WriteLine($"Jornada {numeroJornada}");
                foreach (KeyValuePair<Equipo, Equipo> emparejamiento in partidos)
                {
                    string resultadoLocal = resultadosPorEquipo[emparejamiento.Key];
                    string resultadoVisitante = resultadosPorEquipo[emparejamiento.Value];

                    stw.WriteLine(emparejamiento.Key.Nombre + ";" + resultadoLocal);
                    stw.WriteLine(emparejamiento.Value.Nombre + ";" + resultadoVisitante);
                }
                stw.WriteLine("--------------");
            }
        }

        public void LimpiarArchivo()
        {
            using (StreamWriter stw = new StreamWriter(archivo))
            {
                stw.WriteLine();
            }
        }
    }
}
