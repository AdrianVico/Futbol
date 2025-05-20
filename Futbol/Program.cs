using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Futbol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu m = null;
            do{
                m = new Menu(Menu.Iniciar());
            }while (m.Usuario == null);
            m.MostrarMenuPrincipal();
            Menu.DibujarCuadro(m.Usuario.Equipo.MostrarCamisetas().ToList());
        }
    }
}
