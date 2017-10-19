using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC022Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC022Service : IASCC022Service
    {
        public WSCC022Response WSCC022(WSCC022Request Request)
        {
            ASCC022Integrator client = new ASCC022Integrator();

            var retorno = client.WSCC022(Request);

            return retorno;
        }
    }
}
