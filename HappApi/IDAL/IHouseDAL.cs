using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    using Model;
    public interface IHouseDAL
    {
        List<House> SelectData();
        int AddData(House model);
        int DelData(House model);
        int UpData(House model);
    }
}
