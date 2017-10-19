using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC016Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC016Service : IASCC016Service
    {
        public WSCC016Response WSCC016(WSCC016Request Request)
        {
            ASCC016Integrator client = new ASCC016Integrator();

            var retorno = client.WSCC016(Request);

            return retorno;
        }
    }
}
