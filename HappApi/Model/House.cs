using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class House
    {
        public int Id { get; set; }
        /// <summary>
        /// 申请/发布人
        /// </summary>
        public string  UserId { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }
        /// <summary>
        /// 居室户型
        /// </summary>
        public string  HabitableRoom { get; set; }
        /// <summary>
        /// 面积
        /// </summary>
        public float House_Area { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string  House_Address { get; set; }
        /// <summary>
        /// 地段
        /// </summary>
        public string HouseLocation { get; set; }
        /// <summary>
        /// 房屋设施
        /// </summary>
        public string  HouseFacility { get; set; }
        /// <summary>
        /// 房源所属人电话
        /// </summary>
        public string  House_OwnerTel { get; set; }
        /// <summary>
        ///租金
        /// </summary>
        public decimal House_RentMoney { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public int ApprovalState { get; set; }

        /// <summary>
        /// 推荐状态
        /// </summary>
        public bool RecommendState { get; set; }
 
        /// <summary>
        /// 出租状态
        /// </summary>
        public bool RentState { get; set; }
        /// <summary>
        /// 内景图片
        /// </summary>
        public string ImageUrls { get; set; }
        /// <summary>
        /// 外景图片
        /// </summary>
        public string ExteriorImage { get; set; }
    }
}
