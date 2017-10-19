using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CIPLibrary.WCF.Interface
{
    [ServiceContract]
    public interface IASCC016Service
    {
        [OperationContract]
        SCC016Reference.WSCC016Response WSCC016(SCC016Reference.WSCC016Request Request);
    }
}