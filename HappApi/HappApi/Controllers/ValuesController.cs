using IDAL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HappApi.Controllers
{
    using Unity.Attributes;
    // [Authorize]
    public class ValuesController : ApiController
    {
        [Dependency]
        public IHouseDAL houseDAL { get; set; }
        [Dependency]
        public IConcern concernDAL { get; set; }
        [Dependency]
        public IClientInfo clientInfoDAL { get; set; }
        [HttpGet]
        public ClientInfo Login(string code,string userName)
        {

            ClientInfo clientinfo = new ClientInfo();
            HttpClient httpclient = new HttpClient();
            string appid = "wxb511496bc0508934";//注意与小程序一致
            string secret = "6d4452980e38a8b4658fac86703b1ba2";
            httpclient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpclient.PostAsync("https://api.weixin.qq.com/sns/jscode2session?appid=" + appid + "&secret=" + secret + "&js_code=" + code.ToString() + "&grant_type=authorization_code", null).Result;
            var result = "";
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;
            }
            httpclient.Dispose();
            var results = JsonConvert.DeserializeObject<ClientInfo>(result);
            clientinfo.OpenId = results.OpenId;//唯一标识
            clientinfo.session_key = results.session_key;//密钥
            clientinfo.UserName = userName;
            clientinfo.ComeTime = DateTime.Now;
            clientinfo.IsUse = true;
         
                ClientInfo user = clientInfoDAL.SelectData().Where(m => clientinfo.OpenId == m.OpenId).SingleOrDefault();
                if (user == null)
                {
                    int i = clientInfoDAL.AddData(clientinfo);
                }
           
           
            RedisHelper.Set<ClientInfo>(clientinfo.session_key, clientinfo, DateTime.Now.AddMinutes(10));
            return clientinfo;
        }
        [HttpGet]
        public List<House> HouseList()
        {
            List<House> list = houseDAL.SelectData();
            return list;
        }

        //[AcceptVerbs("GET","POST")]
        /// <summary>
        /// 获取目标房源 以及推荐房屋 
        /// </summary>
        /// <param name="houseRentMoney"></param>
        /// <param name="habitableRoom_ID"></param>
        /// <param name="house_Address"></param>
        /// <returns></returns>
        [HttpGet]

        public object SelectHouseList(decimal houseRentMoney, int habitableRoom_ID, string houseLocation, int houseId)
        {
            //目标房源
            List<House> list = houseDAL.SelectData().Where(m => string.Concat(m.HouseLocation, m.House_Address).Contains(houseLocation) && m.HabitableRoom_ID == habitableRoom_ID && (houseRentMoney - 500) <= m.House_RentMoney && m.House_RentMoney <= (houseRentMoney + 500) && m.ApprovalState == 1 && m.RentState == false).ToList();
            House house1 = list.Where(m => m.RecommendState == true).FirstOrDefault();//推荐住房
            House house2 = list.Where(m => m.RecommendState == false).OrderBy(m => m.House_RentMoney).FirstOrDefault();//无推荐时，显示租金最低的
            House house3 = houseDAL.SelectData().Where(m => m.Id == houseId).FirstOrDefault();//目标房源查看详情时，所选房子
            //推荐住房
            House recommendHouse;
            if (houseId != 0)
            {
                recommendHouse = house3;
            }
            else
            {
                if (house1 != null)
                {
                    recommendHouse = house1;
                }
                else
                {
                    recommendHouse = house2;
                }
            }
            //var recommendHouse = list.Select(m => m.RecommendState ? house1 : house2);//循环筛选获得一个新集合
            //var q = list.Where(m=>m.RecommendState?true:false).OrderBy(m => m.House_RentMoney).FirstOrDefault();
            var returnData = new
            {
                list,
                recommendHouse
            };
            return returnData;
        }
        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public int AddConcern(Concern model)
        {
            Concern conHouse = concernDAL.SelectData().Where(m => m.HouseId == model.HouseId && m.UserOpenId == model.UserOpenId).FirstOrDefault();
            if (conHouse!=null)
            {
                return 2;
            }
            int i = concernDAL.AddData(model);
            return i;
        }
        /// <summary>
        /// 用户关注收藏的房源
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<House> ConcernHouse(string  userOpenId)
        {
            List<Concern> list = concernDAL.SelectData().Where(m => m.UserOpenId == userOpenId).ToList();
            IEnumerable<Int32> linq = from m in list
                                      select m.HouseId;
            int[] houseIds = linq.ToArray();

            List<House> houseList = new List<House>();
            foreach (var item in houseIds)
            {
                House house = houseDAL.SelectData().Where(m => m.Id == item).FirstOrDefault();
                houseList.Add(house);
            }

            return houseList;

        }
        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="houseId"></param>
        /// <returns></returns>
        [HttpGet]
        public int cancelConcern(int houseId)
        {
            int i = concernDAL.DelData(houseId);
            return i;
        }
    }
}
