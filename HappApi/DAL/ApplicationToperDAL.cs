
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
    public class ApplicationToperDAL : IApplicationToper
    {
        public int AddData(ApplicationToper model)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("insert into ApplicationToper values(@UserId,getdate(),@UserName,@UserTel,@ManageDescribe,0)");
                int result = conn.Execute(sql, model);
                return result;
            }
        }

        public int DelData(int id)
        {
            throw new NotImplementedException();
        }

        public List<ApplicationToper> SelectData()
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = "select * from ApplicationToper";
                List<ApplicationToper> list = conn.Query<ApplicationToper>(sql).ToList();
                return list;
            }
        }

        public int UpData(ApplicationToper model)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("update  ApplicationToper set UserId=@UserId,ApplicationTime=@ApplicationTime,UserName=@UserName,UserTel=@UserTel,ManageDescribe=@ManageDescribe,ApprovalState=@ApprovalState where Id=@Id");
                int result = conn.Execute(sql, model);
                return result;
            }
        }
    }
}
