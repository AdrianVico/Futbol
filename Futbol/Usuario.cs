using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Usuario
    {
        string nombre;
        string password;
        Plantilla plantilla;
        public Usuario(string nombre, string password)
        {
            this.nombre = nombre;
            this.password = password;
            plantilla = new Plantilla(nombre);
        }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
        public Plantilla Plantilla { get => plantilla; set => plantilla = value; }

        public override string ToString()
        {
            return $"Nombre: {nombre}, Password: {password}, Plantilla: {plantilla.Nombre}";
        }

    }
}
