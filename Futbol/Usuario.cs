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
        Equipo equipo;
        long dinero;
        int puntos;
        public Usuario(string nombre, string password)
        {
            this.nombre = nombre;
            this.password = password;
            equipo = new Equipo(nombre);
            dinero = 100000000;
            puntos = 0;
        }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Password { get => password; set => password = value; }
        public Equipo Equipo { get => equipo; set => equipo = value; }
        public long Dinero { get => dinero; set => dinero = value; }
        public int Puntos { get => puntos; set => puntos = value; }

        public override string ToString()
        {
            return $"Nombre: {nombre}, Password: {password}, Equipo: {equipo.Nombre}, Dinero: {dinero}" +
                $", Puntos: {puntos}.";
        }

    }
}
