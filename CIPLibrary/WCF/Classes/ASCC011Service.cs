using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC011Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC011Service : IASCC011Service
    {
        public WSCC011Response WSCC011(WSCC011Request Request)
        {

            ASCC011Integrator client = new ASCC011Integrator();
            var retorno = client.WSCC011(Request);

            return retorno;
        }
    }
}
