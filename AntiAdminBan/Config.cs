using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace AntiAdminBan
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = true;
        [Description("\r\nnumber of bans to block an administrator")]
        public int AdminBanCount = 5;
        [Description("Administrator ban duration (in seconds)")]
        public int AdminBanDuration { get; set; } = 1577000000;
        [Description("Administrator ban reason")]
        public string AdminBanReason { get; set; } = "Подозрение на рейд сервера!";
        [Description("Minus the number of bans after time (in seconds)")]
        public float AdminBanClearDuration { get; set; } = 60f;
        [Description("Prohibited reasons for ban")]
        public List<string> AdminBanProhibitedReasons { get; set; } = new List<string>() { "лох", "даун" };
        [Description("Show admin name when banning a player (%reason% - player banned reason)")]
        public string AdminName { get; set; } = "%reason% banned by %adminname%";

    }
}
