using CounterStrikeSharp.API.Core;
using System.Text.Json.Serialization;

namespace ImperfectModels.Configuration
{
    public class Config : BasePluginConfig
    {
        [JsonPropertyName("ConfigVersion")]
        public override int Version { get; set; } = 1;

        public int DefaultAlpha { get; set; } = 255;
    }
}
