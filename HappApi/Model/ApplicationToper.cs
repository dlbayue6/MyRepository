using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public  class ApplicationToper
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户openID
        /// </summary>
        public string  UserId { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplicationTime { get; set; }
        
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string  UserName { get; set; }
        /// <summary>
        /// 用户电话
        /// </summary>
        public string  UserTel { get; set; }
        /// <summary>
        /// 对管理的描述
        /// </summary>
        public string  ManageDescribe { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApprovalState { get; set; }
      
        
    }
}
