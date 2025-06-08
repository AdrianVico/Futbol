using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Futbol
{
    internal interface ISerializableUsuario
    {
        void Serializar(Dictionary<string, string> credenciales);
        Dictionary<string, string> Deserializar();
    }
}
