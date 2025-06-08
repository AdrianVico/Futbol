using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Futbol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu menu = null;
            Usuario usuario = null;
            do
            {
                usuario = Menu.Iniciar();
                if (usuario != null) 
                {
                    menu = new Menu(usuario);
                }   
            }while (usuario == null);
            menu.MostrarMenuPrincipal();
            Console.Clear();
        }
    }
}
