using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using ImperfectModels.Configuration;
using Microsoft.Extensions.Logging;


namespace ImperfectModels;

[MinimumApiVersion(199)]
public partial class ImperfectModels : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Imperfect-Models";
    public override string ModuleVersion => "1.3.1";
    public override string ModuleAuthor => "Imperfect Gamers - raz";
    public override string ModuleDescription => "A plugin for handling player models.";

    public Config Config { get; set; } = new Config();
    public Dictionary<int, CCSPlayerController> ConnectedPlayers = new Dictionary<int, CCSPlayerController>();
    public int ModelAlpha {  get; set; }
    

    public override void Load(bool hotReload)
    {
        RegisterServerEvents();

        RegisterPlayerEvents();
    }

    public override void Unload(bool hotReload)
    {
        base.Unload(hotReload);
    }

    public void OnConfigParsed(Config config)
    {
        /// Parsed config version needs to match the current config version in case of addition/removal of fields
        if (config.Version < Config.Version)
        {
            Logger.LogWarning("The config version does not match current version: Expected: {0} | Current: {1}", Config.Version, config.Version);
        }

        ModelAlpha = Config.DefaultAlpha;

        Config = config;
    }
}