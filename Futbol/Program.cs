using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Futbol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Usuario usuario = InicioSesion.Inicio();
            //Menu menu = new Menu(usuario);
            while (true) { 
                Menu m = new Menu(Menu.Iniciar());
                m.MostrarMenuPrincipal();
            }
        }
    }
}
