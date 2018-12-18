using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
   public  interface IClientInfo
    {
        List<ClientInfo> SelectData();
        int AddData(ClientInfo model);
        int DelData(ClientInfo model);
        int UpData(int id);
    }
}
