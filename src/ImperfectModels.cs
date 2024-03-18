using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using System.Drawing;


namespace ImperfectModels;

[MinimumApiVersion(80)]
public class ImperfectModels : BasePlugin
{
    public override string ModuleName => "Imperfect-Models";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "raz";
    public override string ModuleDescription => "A plugin for handling player models.";

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
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
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

            if (player.PlayerPawn.Value.MovementServices == null)
            {
                return;
            }

            int playerSlot = player.Slot;

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
    }

    // Command for changing the transparency alpha
    [ConsoleCommand("css_modelalpha", "Changes the alpha of the player models")]
    // The `CommandHelper` attribute can be used to provide additional information about the command.
    [CommandHelper(minArgs: 1, usage: "[number for alpha percentage ex. 50]", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
    [RequiresPermissions("@css/root")]
    public void ChangeModelAlphaCommand(CCSPlayerController? player, CommandInfo commandInfo)
    {
        /// This argument is 'css_modelalpha'
        commandInfo.GetArg(0);

        /// This argument is the number for the alpha
        var alphaPercentage = commandInfo.GetArg(1);

        player.PlayerPawn.Value.Render = Color.FromArgb(75, 255, 255, 255);
        Utilities.SetStateChanged(player.PlayerPawn.Value, "CBaseModelEntity", "m_clrRender");
    }
}