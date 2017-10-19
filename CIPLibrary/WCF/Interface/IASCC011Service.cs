using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC011Service
    {
        [OperationContract]
        SCC011Reference.WSCC011Response WSCC011(SCC011Reference.WSCC011Request Request);
    }
}

