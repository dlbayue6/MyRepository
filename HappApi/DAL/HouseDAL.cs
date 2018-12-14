
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    using Model;
    using IDAL;
    using Dapper;
    using System.Data.SqlClient;
    public class HouseDAL : IHouseDAL
    {
        public int AddData(House model)
        {
            throw new NotImplementedException();
        }

        public int DelData(House model)
        {
            throw new NotImplementedException();
        }

        public List<House> SelectData()
        {
            using (SqlConnection conn=DapperHelper.Instance().GetConnection())
            {
                string sql = "select * from House";
                List<House> list = conn.Query<House>(sql).ToList();
                return list;
            }
           
        }

        public int UpData(int id)
        {
            throw new NotImplementedException();
        }
    }
}
