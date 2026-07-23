using InventorySystem.Items;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;
using Random = System.Random;

namespace SCP662;

public class SCP662
{
    private static readonly Lazy<SCP662> _instance =
        new (() => new SCP662());

    private SCP662()
    {
    }
    
    public static SCP662 Instance => _instance.Value;
    
    private DateTime _nextAllowedTime = DateTime.MinValue;
    private readonly TimeSpan _cooldownDuration = TimeSpan.FromSeconds(SCP662Plugin.PluginConfig.SpawnCooldown);
    
    public ushort Serial { get; private set; }

    public Player? GetRandomSpectator()
    {
        var totalConnected = Player.ReadyList.Count();

        if (totalConnected == 0)
            return null;

        var spectators = Player.ReadyList
            .Where(p => p.Role == RoleTypeId.Spectator)
            .ToList();

        return spectators.RandomItem();
    }
    
    public bool IsOnCooldown()
    {
        if (DateTime.Now >= _nextAllowedTime)
        {
            _nextAllowedTime = DateTime.Now + _cooldownDuration;
            return false;
        } else {
            TimeSpan timeRemaining = _nextAllowedTime - DateTime.Now;
            return true;
        }
    }
    
    public void Spawn()
    {
        var room = Room.List.FirstOrDefault(r => r.Name == RoomName.LczGlassroom);
        if (room == null) return;

        Vector3 localOffset = new Vector3(4.9f, 0f, 2.5f);
        Vector3 spawnPosition = room.Transform.TransformPoint(localOffset);
        
        var pickup = Pickup.Create(ItemType.Lantern, spawnPosition, room.Rotation, new Vector3(1f,1f,1f), networkSpawn: true);
        if (pickup == null) return;
        Serial = pickup.Serial;
    }
}