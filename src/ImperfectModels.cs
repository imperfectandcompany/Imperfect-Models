using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Admin;
using CounterStrikeSharp.API.Modules.Commands;
using ImperfectModels.Configuration;
using Microsoft.Extensions.Logging;
using System.Drawing;


namespace ImperfectModels;

[MinimumApiVersion(80)]
public partial class ImperfectModels : BasePlugin, IPluginConfig<Config>
{
    public override string ModuleName => "Imperfect-Models";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "Imperfect Gamers - raz";
    public override string ModuleDescription => "A plugin for handling player models.";

    public Config Config { get; set; } = new Config();

    public override void Load(bool hotReload)
    {
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

        Config = config;
    }
}