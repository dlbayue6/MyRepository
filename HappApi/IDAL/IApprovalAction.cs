using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
   public interface IApprovalAction
    {
        List<ApprovalAction> SelectData();
        int AddData(ApprovalAction model);
        int DelData(int id);
        int UpData(ApprovalAction model);
    }
}
