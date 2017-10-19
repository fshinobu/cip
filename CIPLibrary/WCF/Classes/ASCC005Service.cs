using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC005Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC005Service : IASCC005Service
    {
        public WSCC005Response WSCC005(WSCC005Request Request)
        {

            ASCC005Integrator client = new ASCC005Integrator();
            var retorno = client.WSCC005(Request);

            return retorno;
        }
    }
}


