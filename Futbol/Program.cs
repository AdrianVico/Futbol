using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace Futbol
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Usuario usuario = InicioSesion.Inicio();
            Mercado m = new Mercado(usuario);
            m.LeerFicheroJugadores();
            m.AgregarJugadorMercado();
            m.SeleccionarJugador();
        }
    }
}
