using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model
{
    public class ClientInfo
    {
        public int Id { get; set; }
        public string  OpenId { get; set; }
        public string session_key { get; set; }
        public string  UserName { get; set; }
        
        public DateTime ComeTime { get; set; }
        public bool IsUse { get; set; }
    }
}