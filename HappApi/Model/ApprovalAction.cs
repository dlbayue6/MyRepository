using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
   public class ApprovalAction
    {
      
        public int Id { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public int WorkId { get; set; }
        /// <summary>
        /// 节点IDs
        /// </summary>
        public string StepIds { get; set; }
        /// <summary>
        /// 节点id下标
        /// </summary>
        public int StepIndex { get; set; }
        /// <summary>
        /// 步骤序号
        /// </summary>
        public int StepOrder { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApprovalState { get; set; }
        /// <summary>
        /// 审批角色
        /// </summary>
        public int NowApprover { get; set; }
        /// <summary>
        /// 下一审批角色
        /// </summary>
        public int NextApprover { get; set; }
        /// <summary>
        /// 申请数据ID
        /// </summary>
        public int ApplicationId { get; set; }
         

    }
}
