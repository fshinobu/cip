using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC006Service
    {
        [OperationContract]
        SCC006Reference.WSCC006Response WSCC006(SCC006Reference.WSCC006Request Request);
    }
}

