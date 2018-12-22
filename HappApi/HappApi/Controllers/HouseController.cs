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
    using System.Data.SqlClient;
    using System.Web;
    using Unity.Attributes;
    using Dapper;
    public class HouseController : ApiController
    {
        [Dependency]
        public IHouseDAL houseDAL { get; set; }
        [Dependency]
        public IApprovalAction approvalActionDAL { get; set; }
        [Dependency]
        public IApplicationToper applicationToperDAL { get; set; }
        /// <summary>
        /// 发布房源
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="HabitableRoom"></param>
        /// <param name="House_Area"></param>
        /// <param name="House_Address"></param>
        /// <param name="HouseLocation"></param>
        /// <param name="HouseFacility"></param>
        /// <param name="House_OwnerTel"></param>
        /// <param name="House_RentMoney"></param>
        /// <param name="ImageUrls"></param>
        /// <param name="ExteriorImage"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
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
                if (i == 1)
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

                house.ImageUrls = ImageUrls.TrimEnd(',');
                house.ExteriorImage = ExteriorImage;

                int i = houseDAL.AddData(house);
                if (i == 1)
                {
                    House modelHouse = houseDAL.SelectData().Where(m => m.UserId == UserId && m.ApprovalState == 0).OrderByDescending(m=>m.Id).FirstOrDefault();
                    AddApprovalAction(1, modelHouse.Id);
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
                string fileName = DateTime.Now.ToString("yyyyMMdd") + System.Guid.NewGuid().ToString("N").Substring(20) + extensionName;
                //保存文件路径(可以把用户id、用户昵称之类的传过来，作为文件夹进行存储)
                string filePathName = HttpContext.Current.Request.MapPath("/Images/");
                if (!System.IO.Directory.Exists(filePathName))
                {
                    System.IO.Directory.CreateDirectory(filePathName);
                }

                string newPath = filePathName + fileName;
                file.SaveAs(newPath);

                //file.SaveAs(System.IO.Path.Combine(filePathName, fileName));

                return "/Images/" + fileName;
            }
            return "";
        }

        int stepIndex = 0;
        int stepOrder = 1;
        int approvalState = 0;
        /// <summary>
        /// 添加审批任务
        /// </summary>
        /// <param name="workId"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        private int AddApprovalAction(int workId, int dataId)
        {
            using (SqlConnection conn = DapperHelper.Instance().GetConnection())
            {
                //获取节点ids
                string sql = "select StepIds from ApprovalStep where WorkId=" + workId;
                var stepIds = conn.Query<string>(sql).ToArray();//查询出来的是集合，需转成数组取出其中的值
      
                //获取审批角色id
                var ids = stepIds[0].Split(',');
                var nowstepId =ids[stepIndex]; //获取节点id
                string sql2 = "select RoleId from Step where Id=" + nowstepId;
                int nowroleId = conn.Query<int>(sql2).ToArray()[0];

                //获取下一审批角色id
                int nextroleId = 0;
                if (ids.Length >= stepIndex + 2)
                {
                    var nextstepId = ids[stepIndex+1]; //获取下一节点id
                    string sql3 = "select RoleId from Step where Id=" + nextstepId;
                    nextroleId = conn.Query<int>(sql3).ToArray()[0];
                }

                //初始化审批状态
                approvalState = ids.Length + 1;

                ApprovalAction approvalAction = new Model.ApprovalAction();
                approvalAction.WorkId = workId;
                approvalAction.StepIds = stepIds[0];
                approvalAction.StepIndex = stepIndex;
                approvalAction.StepOrder = stepOrder;
                approvalAction.ApprovalState = approvalState;
                approvalAction.NowApprover = nowroleId;
                approvalAction.NextApprover = nextroleId;
                approvalAction.ApplicationId = dataId;

                int i = approvalActionDAL.AddData(approvalAction);

                return i;

            }


        }
        /// <summary>
        /// 进行审批
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public int ApprovalAction(int id)
        {
            ApprovalAction model = approvalActionDAL.SelectData().Where(m => m.Id == id).FirstOrDefault();
            if (model.ApprovalState != 1)
            {

                //approvalState--;
                stepIndex++;
                stepOrder++;
                using (SqlConnection conn = DapperHelper.Instance().GetConnection())
                {
                    //获取节点ids

                    var  stepIdArray = model.StepIds.Split(',');//数组集合

                    ApprovalAction approvalAction = new Model.ApprovalAction();
                    approvalAction.Id = id;


                    approvalAction.StepIndex = stepIndex;
                    approvalAction.StepOrder = stepOrder;
                    var approvalState = model.ApprovalState - 1;
                    approvalAction.ApprovalState = approvalState;
                  //判断是否存在下一审批角色
                    if (model.NextApprover != 0)
                    {
                        //获取审批角色id
                        int nowroleId = model.NextApprover;
                        approvalAction.NowApprover = nowroleId;
                        //获取下一审批角色id
                        int nextroleId = 0;
                        if (stepIdArray.Length >= stepIndex + 2)
                        {
                            var nextstepId = stepIdArray[stepIndex+1]; //获取下一节点id
                            string sql3 = "select RoleId from Step where Id=" + nextstepId;
                            nextroleId = conn.Query<int>(sql3).ToArray()[0];
                            approvalAction.NextApprover = nextroleId;
                        }
                    }
                    else
                    {
                        // 后面修改需要
                        approvalAction.NowApprover = model.NowApprover;
                        approvalAction.NextApprover = model.NextApprover;
                    }               
                  
                    int i = approvalActionDAL.UpData(approvalAction);

                    if (approvalState==1)
                    {
                        if (model.WorkId == 1)
                        {
                            //修改房屋审批状态
                            House hous = houseDAL.SelectData().Where(m => m.Id == model.ApplicationId).FirstOrDefault();
                            hous.ApprovalState = 1;
                            int j = houseDAL.UpData(hous);
                        }
                        if (model.WorkId == 4)
                        {
                            //修改高级管理员审批状态
                            ApplicationToper applicationToper = applicationToperDAL.SelectData().Where(m => m.Id == model.ApplicationId).FirstOrDefault();
                            applicationToper.ApprovalState = 1;
                            int j = applicationToperDAL.UpData(applicationToper);
                            if (j == 1)
                            {                            
                                //把用户角色修改为高级管理员
                                string sql = string.Format("insert into UserRole values(@UserId,@RoleId)");
                                var ii = conn.Execute(sql, new { UserId = applicationToper.UserId, RoleId = 3 });
                            }
                        }
                        stepIndex = 0;
                        stepOrder = 1;
                        approvalState = 0;
                        return 1;
                    }
                    return 2;
                }
            }
            else
            {
                
                return 3;
            }
        }

        /// <summary>
        /// 申请高级管理员
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/House/ApplicationToper")]
        public int ApplicationToper(string  userId, string userName, string userTel, string manageDescribe)
        {
            //应该是审批任务表里加一个申请人id，这里加上业务类型判断审批任务表是否有数据，下面去数据id时，只取降序后第一条,不然，如果失败，申请过就不能再申请
            ApplicationToper ifModel = applicationToperDAL.SelectData().Where(m => m.UserId == userId).SingleOrDefault();
            if (ifModel!=null)
            {
                return 2;//已申请
            }
            else
            {
                ApplicationToper model = new Model.ApplicationToper();
                model.UserId = userId;
                model.UserName = userName;
                model.UserTel = userTel;
                model.ManageDescribe = manageDescribe;
                int i = applicationToperDAL.AddData(model);
                if (i == 1)
                {
                    ApplicationToper dataModel = applicationToperDAL.SelectData().Where(m => m.UserId == model.UserId).SingleOrDefault();
                    AddApprovalAction(4,dataModel.Id);
                    return 1;//提交申请成功
                }
                else
                {
                    return 0;//提交失败
                }
            }
        }

    }
}
