using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Wrappers;
using UncomplicatedCustomRoles.API.Features;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SCP662;

public class EventsHandler : CustomEventsHandler
{
    private bool IsPlayerMrDeeds(Player player)
    {
        var DeedsPlayers = SummonedCustomRole.Get(MrDeeds.Instance);
        if (DeedsPlayers is null) return false;

        foreach (var i in DeedsPlayers)
        {
            if (i.Player == player)
            {
                return true;
            }
        }
        return false;
    }
    
    public override void OnPlayerHurting(PlayerHurtingEventArgs ev)
    {
        var victim = ev.Player;
        var attacker = ev.Attacker;
        if (attacker == null) return;

        if (IsPlayerMrDeeds(attacker) && victim == MrDeeds.Instance.Master)
        {
            ev.IsAllowed = false;
            return;
        }
    }

    public override void OnPlayerDeath(PlayerDeathEventArgs ev)
    {
        if (IsPlayerMrDeeds(ev.Player))
        {
            MrDeeds.Instance.Master = null;
            return;
        }
        if (ev.Player == MrDeeds.Instance.Master)
        {
            MrDeeds.Instance.Master = null;
            var deeds = MrDeeds.Instance.GetAliveMrDeeds();
            if (deeds == null) return;
            
            
        }
    }

    public override void OnPlayerLeft(PlayerLeftEventArgs ev)
    {
        if (IsPlayerMrDeeds(ev.Player))
        {
            MrDeeds.Instance.Master = null;
            return;
        }
    }


    public override void OnPlayerThrowingItem(PlayerThrowingItemEventArgs ev)
    {
        if (ev.Pickup.Serial != SCP662.Instance.Serial)
        {
            return;
        }
        
        ev.IsAllowed = false;
        
        if (IsPlayerMrDeeds(ev.Player)) return;
        
        if (MrDeeds.Instance.Master != null)
        {
            ev.Player.SendHint("<color=yellow>Mr. Deeds</color> is already present");
            return;
        }

        var pickedPlayer = SCP662.Instance.GetRandomSpectator();
        if (pickedPlayer == null)
        {
            ev.Player.SendHint("<color=yellow>Mr. Deeds</color> can't be spawned");
            return;
        }
        
        if (SCP662.Instance.IsOnCooldown())
        {
            ev.Player.SendHint("The bell is on cooldown");
            return;
        }
        // Play sound
        MrDeeds.Instance.Master = ev.Player;

        Vector3 spawnPos = GetSafeSpawnPosition(ev.Player.Position);
        SummonedCustomRole.Summon(pickedPlayer, MrDeeds.Instance);
        pickedPlayer.Position = spawnPos;
        ev.Player.SendHint("<color=yellow>Mr. Deeds</color> has been spawned!");
    }

    private Vector3 GetSafeSpawnPosition(Vector3 origin, float minDist = 2f, float maxDist = 4f, float clearance = 0.6f)
    {
        for (int i = 0; i < 15; i++)
        {
            float angle = Random.Range(0f, 360f);
            float dist = Random.Range(minDist, maxDist);
            Vector3 dir = new(Mathf.Cos(angle * Mathf.Deg2Rad), 0f, Mathf.Sin(angle * Mathf.Deg2Rad));
            Vector3 pos = origin + dir * dist;

            if (!Physics.CheckSphere(pos + Vector3.up * 1f, clearance))
                return pos;
        }
        return origin + Vector3.forward * 2f + Vector3.up * 0.5f;
    }

    public override void OnPlayerChangedItem(PlayerChangedItemEventArgs ev)
    {
        var newItem = ev.NewItem;
        if (newItem == null) return;
        
        if (newItem.Serial == SCP662.Instance.Serial)
        {
            ev.Player.SendHint("<color=red>SCP-662</color>\nPress T to use it");
        }
    }

    public override void OnServerRoundStarted()
    {
        SCP662.Instance.Spawn();
    }
}