using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC005Service
    {
        [OperationContract]
        SCC005Reference.WSCC005Response WSCC005(SCC005Reference.WSCC005Request Request);
    }
}

