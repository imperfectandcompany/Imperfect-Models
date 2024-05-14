using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using Microsoft.Extensions.Logging;
using System.Drawing;

namespace ImperfectModels
{
    public partial class ImperfectModels
    {
        public void RegisterServerEvents()
        {
            RegisterListener<Listeners.OnMapStart>((mapName) =>
            {
                AddTimer(1.0f, () =>
                {
                    ModelAlpha = Config.DefaultAlpha;
                });
            });
        }
    }
}
