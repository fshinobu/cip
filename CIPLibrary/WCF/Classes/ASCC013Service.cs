using CIPLibrary.WCF.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIPLibrary.SCC013Reference;
using System.IO;
using CIPLibrary.Classes.SCC;
using CIPLibrary.Classes;

namespace CIPLibrary.WCF.Classes
{
    public class ASCC013Service : IASCC013Service
    {
        public WSCC013Response WSCC013(WSCC013Request Request)
        {
            //StringBuilder a = new StringBuilder();
            //            LogHelper.AddLog("Entrou");
            //WSCC013Response retorno = new WSCC013Response();
            ASCC013Integrator client = new ASCC013Integrator();
            var retorno = client.WSCC013(Request);

            return retorno;
        }
    }
}
