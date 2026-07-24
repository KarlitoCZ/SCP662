using InventorySystem.Items;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using SecretLabNAudio.Core;
using UncomplicatedCustomRoles.API.Features;

namespace SCP662
{
  public class SCP662Plugin : Plugin<PluginConfig>
  {
    public static Plugin Instance { get; set; } = null!;
    public static PluginConfig PluginConfig { get; set; } = null!;
    public override string Name { get; } = "SCP-662";
    public override string Description { get; } = "Add SCP-662 to your server.";
    public override string Author { get; } = "Karlito";
    public override Version Version { get; } = new (1, 1, 0, 0);
    public override Version RequiredApiVersion { get; } = new (LabApiProperties.CompiledVersion);
    
    private static EventsHandler Events = new();

    public List<ItemBase> DeedsSpawnedItems = new List<ItemBase>();
    
    public override void Enable()
    {
      Instance = this;
      PluginConfig = Config;
      CustomHandlersManager.RegisterEventsHandler(Events);
      CustomRole.Register(MrDeeds.Instance);
      SaveConfig();
    }

    public override void Disable()
    {
      Instance = null;
      PluginConfig = null;
    }
    
  }
}