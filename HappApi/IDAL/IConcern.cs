﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    using Model;
   public  interface IConcern
    {
        List<Concern> SelectData();
        int AddData(Concern model);
        int DelData(int id);
        int UpData(int id);
    }
}
