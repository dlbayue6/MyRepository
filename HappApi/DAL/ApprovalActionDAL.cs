using IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using System.Data.SqlClient;
using Dapper;

namespace DAL
{
    public class ApprovalActionDAL : IApprovalAction
    {
        public int AddData(ApprovalAction model)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("insert into ApprovalAction values(@WorkId,@StepIds,@StepIndex,@StepOrder,@ApprovalState,@NowApprover,@NextApprover,@ApplicationId)");
                int result = conn.Execute(sql, model);
                return result;
            }
        }

        public int DelData(int id)
        {
            throw new NotImplementedException();
        }

        public List<ApprovalAction> SelectData()
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = "select * from ApprovalAction";
                List<ApprovalAction> list = conn.Query<ApprovalAction>(sql).ToList();
                return list;
            }
        }

        public int UpData(ApprovalAction model)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("update  ApprovalAction set StepIndex=@StepIndex,StepOrder=@StepOrder,ApprovalState=@ApprovalState,NowApprover=@NowApprover,NextApprover=@NextApprover where Id=@Id");
                int result = conn.Execute(sql, model);
                return result;
            }
        }
    }
}
