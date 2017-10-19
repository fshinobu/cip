using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC007Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC007Service : IASCC007Service
    {
        public WSCC007Response WSCC007(WSCC007Request Request)
        {
            ASCC007Integrator client = new ASCC007Integrator();

            var retorno = client.WSCC007(Request);

            return retorno;
        }
    }
}
