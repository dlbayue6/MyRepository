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
        [HttpGet]
        public ClientInfo Login(string code)
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

            RedisHelper.Set<ClientInfo>(clientinfo.session_key, clientinfo, DateTime.Now.AddMinutes(10));
            return clientinfo;
        }
        [HttpGet]
        public List<House> GetHouseList()
        {
            List<House> list = houseDAL.SelectData();
            return list;
        }

        [HttpGet]
        public List<House> SelectHouseList(string houseRentMoney,string habitableRoom_ID,string house_Address)
        {
            List<House> list = houseDAL.SelectData();
            return list;
        }
    }
}
