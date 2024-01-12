using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using System.Collections.Generic;

namespace AntiAdminBan
{
    public class Plugin : Plugin<Config>
    {
        public override string Prefix => "AntiAdminBan";
        public override string Name => "AntiAdminBan";
        public override string Author => "Пряничный Комплекс";
        public Dictionary<Player, int> AdminBanCount = new Dictionary<Player, int>();
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Banning += OnBanning;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            base.OnEnabled();
        }
        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Player.Banning -= OnBanning;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            base.OnDisabled();
        }
        public void OnWaitingForPlayers()
        {
            AdminBanCount.Clear();
        }
        public void OnBanning(BanningEventArgs ev)
        {
            foreach(string s in Config.AdminBanProhibitedReasons)
            {
                if(ev.Reason.ToLower().Contains(s))
                {
                    ev.IsAllowed = false;
                    ev.Player.Ban(Config.AdminBanDuration, Config.AdminBanReason);
                }
            }
            if(ev.Player != null && ev.Target != null && ev.Reason != null)
            {
                ev.Reason = Config.AdminBanReason.Replace("%reason%", ev.Reason).Replace("%adminname%", ev.Player.Nickname);
            }
            if (!AdminBanCount.ContainsKey(ev.Player))
            {
                AdminBanCount[ev.Player] = 1;
                Timing.CallDelayed(1f, () => Log.Info($"{ev.Player} ban count set to {AdminBanCount[ev.Player]}"));

            }
            else
            {
                AdminBanCount[ev.Player]++;
                Timing.CallDelayed(1f, () => Log.Info($"{ev.Player} ban count set to {AdminBanCount[ev.Player]}"));
            }
            if (AdminBanCount.TryGetValue(ev.Player, out var count))
            {
                if (count >= Config.AdminBanCount)
                {
                    ev.IsAllowed = false;
                    ev.Player.Ban(Config.AdminBanDuration, Config.AdminBanReason);
                    Timing.CallDelayed(1f, () => Log.Info($"{ev.Player.Nickname} has been banned! ban count: {AdminBanCount[ev.Player]}"));
                }
            }
            Timing.CallDelayed(Config.AdminBanClearDuration, () => ClearBans(ev.Player));
        }
        public void ClearBans(Player player)
        {
            AdminBanCount[player]--;
            Timing.CallDelayed(1f, () => Log.Info($"{player} ban count set to {AdminBanCount[player]}"));
        }
    }
}
