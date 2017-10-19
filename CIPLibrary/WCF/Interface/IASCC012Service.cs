using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC012Service
    {
        [OperationContract]
        SCC012Reference.WSCC012Response WSCC012(SCC012Reference.WSCC012Request Request);
    }
}

