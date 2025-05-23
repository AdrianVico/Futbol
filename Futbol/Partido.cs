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
            equipos.Add(new Equipo("Barcelona"));
            equipos.Add(new Equipo("Madrid"));
            equipos.Add(new Equipo("Liverpool"));
            equipos.Add(new Equipo("Arsenal"));
            equipos.Add(new Equipo("PSG"));
            equipos.Add(new Equipo("Monaco"));
            equipos.Add(new Equipo("Napoles"));
            equipos.Add(new Equipo("Milan"));
            equipos.Add(new Equipo("Bayern"));
            equipos.Add(new Equipo("Leipzig"));
            equipos.Add(new Equipo("Elche"));
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
            if (numeroJornada == 8)
            {
                numeroJornada = 1;
            }
            else
            {
                numeroJornada++;
                GuardarNumeroJornada();
                string titulo = "Partidos de la J" + numeroJornada;
                int posX = (Console.WindowWidth - titulo.Length) / 2;
                Console.SetCursorPosition(posX, Console.CursorTop);
                Console.WriteLine(titulo);
            }
            
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
    }
}
