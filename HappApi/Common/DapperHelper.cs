using System.Data.SqlClient;
using System.Net.Configuration;
//using MySql.Data.MySqlClient;


    public class DapperHelper
    {
        private static DapperHelper _dapperHelper = null;

        private readonly string _cnnstr = "";

        private DapperHelper()
        {
            _cnnstr = "server=.;database=HappyTenement;uid=sa;pwd=111111";
			// //_cnnstr= "server=localhost;port=; database=test;username=root;password=dinglinBAyue^;";//MySQL
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DapperHelper Instance()
        {
            if (_dapperHelper == null)
            {
                _dapperHelper = new DapperHelper();
            }
            return _dapperHelper;
        }

        public SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_cnnstr);
            //var conn = new MySqlConnection(_cnnstr);
            conn.Open();
            return conn;
        }
    }

