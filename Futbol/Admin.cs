using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Admin : Usuario
    {
        public Admin(string nombre, string password): base(nombre, password)
        {
        }
        public override void ActualizarFicheroDatos()
        {
            string ruta = $"../../../Usuarios/{Nombre}/{Nombre}_datos.txt";
            string alineacion = string.Join(",", Equipo.Alineacion);

            using (StreamWriter sw = new StreamWriter(ruta))
            {
                sw.WriteLine($"999999999;{Puntos};{alineacion};admin");
            }
        }
    }
}
