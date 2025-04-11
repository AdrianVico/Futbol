using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal class Partido
    {
        List<Equipo> equipos;
        string resultado;
        public Partido(string resultado)
        {
            this.resultado = resultado;
            equipos = new List<Equipo>();
        }
        public string Resultado { get => resultado; set => resultado = value; }
        
        public override string ToString()
        {
            string equiposString = string.Join(", ", equipos.Select(e => e.Nombre));
            return $"Resultado: {resultado}, Equipos: {equiposString}";
        }
    }
}
