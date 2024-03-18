using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;
using System.Drawing;


namespace ImperfectModels;

[MinimumApiVersion(80)]
public class ImperfectModels : BasePlugin
{
    public override string ModuleName => "Imperfect-Models";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Imperfect Gamers - raz";
    public override string ModuleDescription => "A plugin for handling player models.";

    private Dictionary<int, CCSPlayerController> ConnectedPlayers = new Dictionary<int, CCSPlayerController>();

    public override void Load(bool hotReload)
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
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }

    private void OnPlayerConnect(CCSPlayerController? player, bool isForBot = false)
    {
        int playerSlot = player.Slot;

        ConnectedPlayers[playerSlot] = new CCSPlayerController(player.Handle);

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

            if (player.PlayerPawn.Value.MovementServices == null)
            {
                return;
            }

            try
            {
                ///  TODO: Set the default alpha from the config file
                player.PlayerPawn.Value.Render = Color.FromArgb(75, 254, 254, 254);
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
        finally
        {
            if (ConnectedPlayers[playerSlot] == null)
            {
                ConnectedPlayers.Remove(playerSlot);
            }
        }
    }

    private void OnPlayerDisconnect(CCSPlayerController? player, bool isForBot = false)
    {
        if (ConnectedPlayers.TryGetValue(player.Slot, out var connectedPlayer))
        {
            ConnectedPlayers.Remove(player.Slot);
        }
    }


    // Command for changing the player model alpha (transparency)
    [ConsoleCommand("css_selfmodelalpha", "Changes the alpha of your player model")]
    [CommandHelper(minArgs: 1, usage: "<number for alpha ex. 50>", whoCanExecute: CommandUsage.CLIENT_ONLY)]
    [RequiresPermissions("@css/root")]
    public void ChangeSelfModelAlphaCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        /// This argument is 'css_modelalpha'
        commandInfo.GetArg(0);

        /// This argument is the number for the alpha
        var alphaPercentage = commandInfo.GetArg(1);
        int alphaPercentageInt = 0;

        var intParseSuccess = int.TryParse(alphaPercentage, out alphaPercentageInt);

        if (intParseSuccess)
        {
            try
            {
                player.PlayerPawn.Value.Render = Color.FromArgb(alphaPercentageInt, 255, 255, 255);
                Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Something went wrong when setting the player model alpha {message}", ex.Message);

                commandInfo.ReplyToCommand("Something went wrong when setting the player model alpha. Check error logs for more info.");
            }
        }
        else
        {
            Logger.LogWarning("The number that was input was not correct.");
            commandInfo.ReplyToCommand("The number that you input was not correct. Try a number between 1 and 100.");
        }
    }

    // Command for changing all players model alpha (transparency)
    [ConsoleCommand("css_modelalpha", "Changes the alpha of all player models")]
    [CommandHelper(minArgs: 1, usage: "<number for alpha ex. 50>", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/root")]
    public void ChangeModelAlphaCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        /// This argument is 'css_modelalpha'
        commandInfo.GetArg(0);

        /// This argument is the number for the alpha
        var alphaPercentage = commandInfo.GetArg(1);
        int alphaPercentageInt = 0;

        var intParseSuccess = int.TryParse(alphaPercentage, out alphaPercentageInt);

        if (intParseSuccess)
        {
            try
            {
                foreach (var connectedPlayer in ConnectedPlayers.Values)
                {
                    connectedPlayer.PlayerPawn.Value.Render = Color.FromArgb(alphaPercentageInt, 254, 254, 254);
                    Utilities.SetStateChanged(connectedPlayer.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
                }

                commandInfo.ReplyToCommand($"All player models alpha set to {alphaPercentage}");
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Something went wrong when setting the player model alpha {message}", ex.Message);

                commandInfo.ReplyToCommand("Something went wrong when setting the player model alpha. Check error logs for more info.");
            }
        }
        else
        {
            Logger.LogWarning("The number that was input was not correct.");
            commandInfo.ReplyToCommand("The number that you input was not correct. Try a number between 1 and 255.");
        }
    }
}