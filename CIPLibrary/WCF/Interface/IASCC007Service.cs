using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC007Service
    {
        [OperationContract]
        SCC007Reference.WSCC007Response WSCC007(SCC007Reference.WSCC007Request Request);
    }
}