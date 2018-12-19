using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    using IDAL;
    using Model;
    using Dapper;
    using System.Data.SqlClient;

    public class ConcernDAL : IConcern
    {
        public int AddData(Concern model)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql =string.Format("insert into Concern values(@UserOpenId,@HouseId)");
                int result = conn.Execute(sql,model);
                return result;
            }
        }

        public int DelData(int id)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("delete from Concern where HouseId=@houseId");
                int result = conn.Execute(sql, new { houseId=id });
                return result;
            }
        }

        public List<Concern> SelectData()
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = "select * from Concern";
                List<Concern> list =conn.Query<Concern>(sql).ToList();
                return list;
            }
        
        }

        public int UpData(int id)
        {
            throw new NotImplementedException();
        }
    }
}
