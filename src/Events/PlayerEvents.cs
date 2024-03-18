using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImperfectModels
{
    public partial class ImperfectModels
    {
        public void RegisterPlayerEvents()
        {
            RegisterEventHandler<EventPlayerConnectFull>((@event, info) =>
            {
                if (@event.Userid.IsValid)
                {
                    var player = @event.Userid;

                    if (!player.IsValid || player.IsBot)
                    {
                        return HookResult.Continue;
                    }
                    else
                    {
                        OnPlayerConnect(player);
                        return HookResult.Continue;
                    }
                }
                else
                {
                    return HookResult.Continue;
                }
            });
        }
        private void OnPlayerConnect(CCSPlayerController? player, bool isForBot = false)
        {
            try
            {
                if (player == null)
                {
                    return;
                }

                if (player.PlayerPawn == null)
                {
                    return;
                }

                try
                {
                    player.PlayerPawn.Value.Render = Color.FromArgb(Config.DefaultAlpha, 255, 255, 255);
                    Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Something bad happened: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something bad happened: {ex.Message}");
            }
        }
    }
}
