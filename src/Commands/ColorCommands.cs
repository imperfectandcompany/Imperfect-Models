using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using Microsoft.Extensions.Logging;

namespace ImperfectModels
{
    public partial class ImperfectModels
    {
        // Command for changing all players model alpha (transparency)
        [ConsoleCommand("css_setmodelalpha", "Set the alpha of all player models")]
        [CommandHelper(minArgs: 1, usage: "<number for alpha ex. 50>", whoCanExecute: CommandUsage.CLIENT_AND_SERVER)]
        [RequiresPermissions("@css/root")]
        public void ChangeModelAlphaCommand(CCSPlayerController? player, CommandInfo commandInfo)
        {
            /// This argument is the number for the alpha
            var newModelAlpha = commandInfo.GetArg(1);
            int newModelAlphaInt = 0;

            var intParseSuccess = int.TryParse(newModelAlpha, out newModelAlphaInt);

            if (intParseSuccess)
            {
                try
                {
                    foreach (var connectedPlayer in ConnectedPlayers)
                    {
                        ModelAlpha = newModelAlphaInt;
                        connectedPlayer.Value.PlayerPawn.Value.Render = SetPlayerPawnColor(connectedPlayer.Value, ModelAlpha);
                    }

                    commandInfo.ReplyToCommand($"All player models alpha set to {newModelAlpha}");
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
