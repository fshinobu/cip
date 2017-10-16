using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC013Service
    {
        [OperationContract]
        SCC013Reference.WSCC013Response WSCC013(SCC013Reference.WSCC013Request Request);
    }
}
