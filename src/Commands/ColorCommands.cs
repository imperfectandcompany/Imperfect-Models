using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API;
using Microsoft.Extensions.Logging;
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
        // Command for changing the player model alpha (transparency)
        [ConsoleCommand("css_selfmodelalpha", "Changes the alpha of your player model")]
        [CommandHelper(minArgs: 1, usage: "<number for alpha ex. 50>", whoCanExecute: CommandUsage.CLIENT_ONLY)]
        [RequiresPermissions("@css/root")]
        public void ChangeSelfModelAlphaCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
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

                    commandInfo.ReplyToCommand($"Player model alpha set to {alphaPercentage}");
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("Something went wrong when setting the player model alpha {message}", ex.Message);

                    commandInfo.ReplyToCommand("Something went wrong when setting the player model alpha. Check error logs for more info.");
                }
            }
            else
            {
                commandInfo.ReplyToCommand("The number that you input was not correct. Try a number between 1 and 255.");
            }
        }

        // Command for changing all players model alpha (transparency)
        [ConsoleCommand("css_modelalpha", "Changes the alpha of all player models")]
        [CommandHelper(minArgs: 1, usage: "<number for alpha ex. 50>", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        [RequiresPermissions("@css/root")]
        public void ChangeModelAlphaCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            /// This argument is the number for the alpha
            var alphaPercentage = commandInfo.GetArg(1);
            int alphaPercentageInt = 0;

            var connectedPlayers = Utilities.GetPlayers();

            var intParseSuccess = int.TryParse(alphaPercentage, out alphaPercentageInt);

            if (intParseSuccess)
            {
                try
                {
                    foreach (var connectedPlayer in connectedPlayers)
                    {
                        connectedPlayer.PlayerPawn.Value.Render = Color.FromArgb(alphaPercentageInt, 255, 255, 255);
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
                commandInfo.ReplyToCommand("The number that you input was not correct. Try a number between 1 and 255.");
            }
        }
    }
}
