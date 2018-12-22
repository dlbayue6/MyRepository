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
    using IDAL;
    using Model;

    public class ApplicationController : ApiController
    {
        [Dependency]
        public IApprovalAction approvalActionDAL { get; set; }
        /// <summary>
        /// 根据角色获取审批任务
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        public List<ApprovalAction> GetApprovalActionList(int roleId)
        {
            List<ApprovalAction> list = approvalActionDAL.SelectData().Where(m => m.ApprovalState != 1 && m.NowApprover == roleId).ToList();
            return list;
        }
    }
}
