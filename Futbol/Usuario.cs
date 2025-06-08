using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace Futbol
{
    internal class Usuario: ISerializableUsuario
    {
        string nombre;
        string password;
        Equipo equipo;
        long dinero;
        int puntos;

        public Usuario(string nombre, string password, Equipo equipo, long dinero, int puntos)
        {
            this.nombre = nombre;
            this.password = password;
            this.equipo = equipo;
            this.dinero = dinero;
            this.puntos = puntos;
        }

        public Usuario(string nombre, string password, Equipo equipo):this(nombre, password, equipo, 100000000, 0)
        {  }

        public Usuario(string nombre, string password):this(nombre, password, null) { }

        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
        public Equipo Equipo { get => equipo; set => equipo = value; }
        public long Dinero { get => dinero; set => dinero = value; }
        public int Puntos { get => puntos; set => puntos = value; }


        public void ActualizarFicheroDatos()
        {
            string ruta = $"../../../Usuarios/{nombre}/{nombre}_datos.txt";
            string alineacion = string.Join(",", Equipo.Alineacion);

            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine(dinero+";"+puntos+";"+alineacion);
            }
        }

        public void Serializar(Dictionary<string, string> credenciales)
        {
            string ruta = $"../../../Usuarios/Usuarios.txt";
            JsonSerializerOptions options = new JsonSerializerOptions {WriteIndented = true};  
            string jsonString = JsonSerializer.Serialize(credenciales, options);
            File.WriteAllText(ruta, jsonString);
        }

        public Dictionary<string, string> Deserializar()
        {
            Dictionary<string, string> credenciales = new Dictionary<string, string>();
            string ruta = $"../../../Usuarios/Usuarios.txt";
            string jsonString = File.ReadAllText(ruta);
            try
            {
                credenciales = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return credenciales;
        }

        public override string ToString()
        {
            return $"Nombre: {nombre}, Password: {password}, Equipo: {equipo.Nombre}, Dinero: {dinero}" +
                $", Puntos: {puntos}.";
        }

    }
}
