using LabApi.Features.Wrappers;
using PlayerRoles;
using UncomplicatedCustomRoles.API.Features;
using UncomplicatedCustomRoles.API.Features.Behaviour;
using UncomplicatedCustomRoles.API.Interfaces;
using UncomplicatedCustomRoles.Manager;
using UnityEngine;

namespace SCP662;

public class MrDeeds : ICustomRole
{
    
    private static readonly Lazy<MrDeeds> _instance =
        new (() => new MrDeeds());

    private MrDeeds()
    {
    }

    public Player? Master = null!;

    public static MrDeeds Instance => _instance.Value;


    public Player? GetAliveMrDeeds()
    {
   
        var DeedsPlayers = SummonedCustomRole.Get(Instance);
        if (DeedsPlayers is null) return null;

        foreach (var i in DeedsPlayers)
        {
            return i.Player;
        }
        
        return null;
    }
    
    public int Id { get; set; } = 662;
    public string Name { get; set; } = "Mr. Deeds";
    public bool OverrideRoleName { get; set; }
    public string? Nickname { get; set; }
    public string CustomInfo { get; set; } = "<color=#C50000>SCP-662</color><color=#FFFFFF> - Mr. Deeds</color>";
    public string BadgeName { get; set; } = null!;
    public string BadgeColor { get; set; } = null!;
    public RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
    public Team? Team { get; set; } = PlayerRoles.Team.OtherAlive;
    public RoleTypeId RoleAppearance { get; set; } = RoleTypeId.Tutorial;
    public List<Team> IsFriendOf { get; set; } = new List<Team> { };
    public HealthBehaviour Health { get; set; } = new HealthBehaviour
    {
        Amount = SCP662Plugin.PluginConfig.HealthMrDeeds,
        Maximum = SCP662Plugin.PluginConfig.HealthMrDeeds
    };
    public AhpBehaviour Ahp { get; set; } = new AhpBehaviour();
    public HumeShieldBehaviour HumeShield { get; set; } = new HumeShieldBehaviour
    {
        Amount = SCP662Plugin.PluginConfig.HealthShieldMrDeeds,
        Maximum = SCP662Plugin.PluginConfig.HealthShieldMrDeeds,
        RegenerationAmount = 2.5f,
        RegenerationDelay = 5f,
        RegenerationSpeed = 0.5f
    };

    public List<Effect>? Effects { get; set; } = new List<Effect> { new Effect() };
    public StaminaBehaviour Stamina { get; set; } = new StaminaBehaviour();
    public int MaxScp330Candies { get; set; }
    public bool CanEscape { get; set; } = false;
    public Dictionary<string, string> RoleAfterEscape { get; set; } = new Dictionary<string, string>();
    public Vector3 Scale { get; set; }
    public string SpawnBroadcast { get; set; } = null!;
    public ushort SpawnBroadcastDuration { get; set; }
    public string SpawnHint { get; set; } = "You are <color=red>Mr. Deeds</color>\nFollow orders given by your master";
    public float SpawnHintDuration { get; set; } = 10f;
    public Dictionary<ItemCategory, sbyte> CustomInventoryLimits { get; set; } = new Dictionary<ItemCategory, sbyte>();

    public List<ItemType> Inventory { get; set; } = new List<ItemType>
    {
        ItemType.GunCOM18,
        ItemType.Flashlight,
        ItemType.KeycardMTFOperative
    };
    public List<uint> CustomItemsInventory { get; set; } = new List<uint>();

    public Dictionary<ItemType, ushort> Ammo { get; set; } = new Dictionary<ItemType, ushort>
    {
        [ItemType.Ammo9x19] = 255
    };

    public float DamageMultiplier { get; set; } = 1.2f;
    public SpawnBehaviour? SpawnSettings { get; set; } = new SpawnBehaviour();

    public List<object>? CustomFlags { get; set; } = new List<object>()
    {
        
    };
    public bool IgnoreSpawnSystem { get; set; } = true;
}