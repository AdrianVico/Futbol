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
            Usuario u = null;
            do
            {
                u = Menu.Iniciar();
                if (u != null) 
                {
                    m = new Menu(u);
                }   
            }while (u == null);
            m.MostrarMenuPrincipal();
            Menu.DibujarCuadro(m.Usuario.Equipo.MostrarCamisetas().ToList());
        }
    }
}
