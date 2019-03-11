using System;
using System.Collections.Generic;

/// <summary>
/// ÕË»§ÊµÌå
/// </summary>
    public class RetAccountEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public int YuanBao { get; set; }
        public int LastServerId { get; set; }
        public string LastServerName { get; set; }
        public string LastServerIp { get; set; }
        public int LastServerPort { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

    
}