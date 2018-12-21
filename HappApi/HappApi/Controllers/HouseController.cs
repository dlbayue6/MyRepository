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
    using System.Web;
    using Unity.Attributes;
    public class HouseController : ApiController
    {
        [Dependency]
        public IHouseDAL houseDAL { get; set; }
        [HttpGet]
        public int AddHouse(string UserId, string HabitableRoom, float House_Area, string House_Address, string HouseLocation, string HouseFacility, string House_OwnerTel, decimal House_RentMoney, string ImageUrls, string ExteriorImage, int roleId)
        {
            if (roleId != 1)
            {
                House house = new House();
                house.UserId = UserId;
                house.HabitableRoom = HabitableRoom;
                house.House_Area = House_Area;
                house.House_Address = House_Address;
                house.HouseLocation = HouseLocation;
                house.HouseFacility = HouseFacility;
                house.House_OwnerTel = House_OwnerTel;
                house.House_RentMoney = House_RentMoney;
                house.ApprovalState = 1;

                house.ImageUrls = ImageUrls.TrimEnd(',');
                house.ExteriorImage = ExteriorImage;

                int i = houseDAL.AddData(house);
                if (i==1)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
               
            }
            else
            {
                House house = new House();
                house.UserId = UserId;
                house.HabitableRoom = HabitableRoom;
                house.House_Area = House_Area;
                house.House_Address = House_Address;
                house.HouseLocation = HouseLocation;
                house.HouseFacility = HouseFacility;
                house.House_OwnerTel = House_OwnerTel;
                house.House_RentMoney = House_RentMoney;
                house.ApprovalState = 0;

                house.ImageUrls = ImageUrls.Trim('"').TrimEnd(',');
                house.ExteriorImage = ExteriorImage.Trim('"');

                int i = houseDAL.AddData(house);
                if (i == 1)
                {
                    return 2;
                }
                else
                {
                    return 0;
                }
            }
            
        }

        [HttpPost]
        public string UploadFileNew()
        {

            HttpPostedFile file = HttpContext.Current.Request.Files["uploadfile_ant"];

            if (file != null)
            {


                //获取文件后缀
                string extensionName = System.IO.Path.GetExtension(file.FileName);

                //文件名
                // string fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + System.Guid.NewGuid().ToString("N") + extensionName;
                string fileName = DateTime.Now.ToString("yyyyMMdd")+ System.Guid.NewGuid().ToString("N").Substring(20) + extensionName;
                //保存文件路径(可以把用户id、用户昵称之类的传过来，作为文件夹进行存储)
                string filePathName = HttpContext.Current.Request.MapPath("/Images/") ;
                if (!System.IO.Directory.Exists(filePathName))
                {
                    System.IO.Directory.CreateDirectory(filePathName);
                }

                string newPath = filePathName + fileName;
                file.SaveAs(newPath);

                //file.SaveAs(System.IO.Path.Combine(filePathName, fileName));

                return "/Images/"+ fileName;
            }
            return "";
        }
    }
}
