using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC022Service
    {
        [OperationContract]
        SCC022Reference.WSCC022Response WSCC022(SCC022Reference.WSCC022Request Request);
    }
}