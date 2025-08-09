

using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine;

public class NetworkedBarracks : NetworkBehaviour
{
    [SerializeField] private NetworkObject unitPrefab; // Prefab of the unit to spawn
    [SerializeField] private Transform spawnPoint; // Location where units will be spawned
    [SerializeField] private float spawnCooldown = 5f; // Cooldown time between spawns
    [SerializeField] private int teamId; // Customizable team ID for spawned units
    public bool spawnOnTimer = false;
    private float lastSpawnTime;

    // Spawns a unit immediately (can be called from the server).
    //[ObserversRpc(BufferLast = true, RunLocally = true)]
    public void SpawnUnitImmediately()
    {
        if(!IsServerInitialized) return;
        if (unitPrefab == null || spawnPoint == null)
        {
            Debug.LogError("Unit Prefab or Spawn Point is not set.");
            return;
        }
        NetworkObject unit = Instantiate(unitPrefab, spawnPoint.position, spawnPoint.rotation);
        
        ArmyGroup armyGroup = unit.GetComponent<ArmyGroup>();
        armyGroup.SetTeam(teamId);
        
        Spawn(unit, null);
        
    }
    //[ObserversRpc(BufferLast = true, RunLocally = true)]
    //void SetTeam(ArmyGroup armyGroup, int teamId)
    //{
    //    if (armyGroup != null)
    //    {

    //        armyGroup.enabled = true;
    //        armyGroup.teamId = teamId;
    //        armyGroup.SetTeam(teamId);
    //    }
    //}
    // Spawns a unit with a cooldown. Must be called by the server.
    
    public void ToggleSpawnOnTimer(bool toggle)
    {
        spawnOnTimer = toggle;
    }

    // Updates the spawning logic based on the timer
    private void Update()
    {
        if (!IsServerInitialized)
            return;
        if (!spawnOnTimer) return;

        if (Time.time - lastSpawnTime >= spawnCooldown)
        {
            SpawnUnitImmediately();
            lastSpawnTime = Time.time;
        }
    }

    // Requests spawning a unit immediately from the client


}