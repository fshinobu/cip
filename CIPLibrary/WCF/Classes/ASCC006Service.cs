using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC006Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC006Service : IASCC006Service
    {
        public WSCC006Response WSCC006(WSCC006Request Request)
        {
            ASCC006Integrator client = new ASCC006Integrator();

            var retorno = client.WSCC006(Request);

            return retorno;
        }
    }
}
