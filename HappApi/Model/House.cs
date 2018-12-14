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
        public int UserId { get; set; }
        public DateTime PublishTime { get; set; }    
        public int HabitableRoom_ID { get; set; }
        public string  House_Address { get; set; }
        public string HouseLocation { get; set; }
        public int HouseFacilityId { get; set; }
        public string  House_OwnerTel { get; set; }
        public decimal House_RentMoney { get; set; }
        public int ApprovalState { get; set; }

        
    }
}
