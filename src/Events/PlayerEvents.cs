using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using System.Drawing;

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

            RegisterEventHandler<EventPlayerDisconnect>((@event, info) =>
            {
                var player = @event.Userid;

                if (player.IsBot || !player.IsValid)
                {
                    return HookResult.Continue;
                }
                else
                {
                    OnPlayerDisconnect(player);
                    return HookResult.Continue;
                }
            });

            RegisterEventHandler<EventPlayerSpawned>((@event, info) =>
            {
                if (@event.Userid == null) return HookResult.Continue;

                var player = @event.Userid;

                if (player.IsBot || !player.IsValid || player == null)
                {
                    return HookResult.Continue;
                }
                else
                {
                    player.PlayerPawn.Value.Render = SetPlayerPawnColor(player, ModelAlpha);
                    return HookResult.Continue;
                }
            });

            RegisterListener<Listeners.OnTick>(PlayerOnTick);
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

                int playerSlot = player.Slot;

                try
                {
                    ConnectedPlayers[playerSlot] = new CCSPlayerController(player.Handle);

                    player.PlayerPawn.Value.Render = SetPlayerPawnColor(player, ModelAlpha);
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Something bad happened: {ex.Message}");
                }
                finally
                {
                    if (ConnectedPlayers[playerSlot] == null)
                    {
                        ConnectedPlayers.Remove(playerSlot);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Something bad happened: {ex.Message}");
            }
        }

        private void OnPlayerDisconnect(CCSPlayerController? player, bool isForBot = false)
        {
            if (player == null) return;

            try
            {
                if (ConnectedPlayers.TryGetValue(player.Slot, out var connectedPlayer))
                {
                    ConnectedPlayers.Remove(player.Slot);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Something bad happened: {ex.Message}");
            }
        }

        private void PlayerOnTick()
        {
            try
            {
                foreach (CCSPlayerController player in ConnectedPlayers.Values)
                {
                    player.PlayerPawn.Value.Render = SetPlayerPawnColor(player, ModelAlpha);
                }
            }
            catch (Exception ex)
            {
                Logger.LogWarning($"Something bad happened: {ex.Message}");
            }
        }

        private Color SetPlayerPawnColor(CCSPlayerController player, int alpha)
        {
            return Color.FromArgb(alpha, 255, 255, 255);
        }
    }
}
