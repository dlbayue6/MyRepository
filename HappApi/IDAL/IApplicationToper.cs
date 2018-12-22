using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
   public  interface IApplicationToper
    {
        List<ApplicationToper> SelectData();
        int AddData(ApplicationToper model);
        int DelData(int id);
        int UpData(ApplicationToper model);
    }
}
