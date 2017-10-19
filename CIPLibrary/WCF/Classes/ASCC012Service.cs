using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC012Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC012Service : IASCC012Service
    {
        public WSCC012Response WSCC012(WSCC012Request Request)
        {

            ASCC012Integrator client = new ASCC012Integrator();
            var retorno = client.WSCC012(Request);

            return retorno;
        }
    }
}
