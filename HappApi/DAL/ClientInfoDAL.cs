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
    public class ClientInfoDAL : IClientInfo
    {
        public int AddData(ClientInfo model)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("insert into UserInfo values(@OpenId,@session_key,@UserName,@ComeTime,@IsUse)");
                int result = conn.Execute(sql,model);
                return result;
            }
        }

        public int DelData(ClientInfo model)
        {
            throw new NotImplementedException();
        }

        public List<ClientInfo> SelectData()
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = "select * from UserInfo";
                List<ClientInfo> list = conn.Query<ClientInfo>(sql).ToList();
                return list;
            }

        }

        public int UpData(int id)
        {
            throw new NotImplementedException();
        }
    }
}
