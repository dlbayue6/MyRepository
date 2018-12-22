
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
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("insert into House values(@UserId,getdate(),@HabitableRoom,@House_Area,@House_Address,@HouseLocation,@HouseFacility,@House_OwnerTel,@House_RentMoney,@ApprovalState,0,0,@ImageUrls,@ExteriorImage)");
                int result = conn.Execute(sql, model);
                return result;
            }
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

        public int UpData(House model)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                string sql = string.Format("update  House set UserId=@UserId,PublishTime=getdate(),HabitableRoom=@HabitableRoom,House_Area=@House_Area,House_Address=@House_Address,HouseLocation=@HouseLocation,HouseFacility=@HouseFacility,House_OwnerTel=@House_OwnerTel,House_RentMoney=@House_RentMoney,ApprovalState=@ApprovalState,RecommendState=@RecommendState,RentState=@RentState,ImageUrls=@ImageUrls,ExteriorImage=@ExteriorImage where Id=@Id");
                int result = conn.Execute(sql, model);
                return result;
            }
        }
    }
}
