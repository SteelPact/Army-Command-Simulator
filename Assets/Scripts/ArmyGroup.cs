using FishNet.CodeGenerating;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Pathfinding;
using System;
using UnityEngine;

public class ArmyGroup : NetworkBehaviour
{
    [Header("Army Properties")]
    [AllowMutableSyncType]
    [SerializeField]
    private SyncVar<int> teamId = new SyncVar<int>();

    [AllowMutableSyncType]
    [SerializeField]
    private SyncVar<int> currentHealth = new SyncVar<int>(new SyncTypeSettings(1f));

    [Tooltip("ScriptableObject holding all the configurable stats for this army group")]
    public ArmyStats stats;

    [Header("Runtime State (read?only)")]
    private float customizableDamageTimer;
    private float _healAccumulator;
    private bool isEngaged = false;

    [Header("References")]
    public AIPath ai;
    [SerializeField] private LayerMask armyLayerMask;

    #region Server Lifecycle

    public override void OnStartServer()
    {
        base.OnStartServer();

        // Initialize health, burst timer and healing accumulator
        currentHealth.Value = stats.maxHealth;
        customizableDamageTimer = stats.customizableDamageCooldown;
        _healAccumulator = 0f;
        isEngaged = false;
    }

    private void Awake()
    {
        // Watch for health hitting zero on server
        currentHealth.OnChange += OnHealthChanged;
    }

    #endregion

    #region Public API

    /// <summary>
    /// Assigns this group's team.
    /// </summary>
    public void SetTeam(int team)
    {
        teamId.Value = team;
    }

    /// <summary>
    /// Returns the team this group belongs to.
    /// </summary>
    public int RequestTeam()
    {
        return teamId.Value;
    }

    #endregion

    #region Update Loop

    private void Update()
    {
        // Only run combat & healing logic on server
        if (!IsServerInitialized)
            return;

        // Countdown the burst?damage cooldown
        customizableDamageTimer -= Time.deltaTime;

        // Detect an enemy inside our attack radius
        ArmyGroup enemy = DetectEnemy();
        if (enemy != null)
        {
            // Just entered combat? reset healing accumulator
            if (!isEngaged)
                _healAccumulator = 0f;

            isEngaged = true;
            EngageEnemy(enemy);
        }
        else
        {
            // No enemy ? out of combat
            isEngaged = false;
            PassiveHealing();
        }
    }

    #endregion

    #region Movement & Detection

    [ServerRpc(RequireOwnership = false, RunLocally = true)]
    public void SetDestination(Vector3 destination)
    {
        isEngaged = false;
        ai.destination = destination;
    }

    /// <summary>
    /// Returns one enemy in range, or null.
    /// </summary>
    private ArmyGroup DetectEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, stats.attackRadius, armyLayerMask);
        foreach (var c in hits)
        {
            ArmyGroup other = c.GetComponent<ArmyGroup>();
            if (other != null && other.teamId.Value != teamId.Value)
                return other;
        }
        return null;
    }

    #endregion

    #region Combat

    private void EngageEnemy(ArmyGroup enemy)
    {
        bool canBurst = customizableDamageTimer <= 0f;

        int myDamage = Mathf.FloorToInt(stats.damagePerSecond * Time.deltaTime);
        if (canBurst)
        {
            myDamage += stats.customizableDamage;
            customizableDamageTimer = stats.customizableDamageCooldown;
        }

        int enemyDamage = Mathf.FloorToInt(enemy.stats.damagePerSecond * Time.deltaTime);

        // Apply damage to synced health values
        currentHealth.Value -= enemyDamage;
        enemy.currentHealth.Value -= myDamage;
    }

    #endregion

    #region Healing

    private void PassiveHealing()
    {
        if (currentHealth.Value >= stats.maxHealth)
            return;

        // accumulate fractional healing
        _healAccumulator += stats.passiveHealing * Time.deltaTime;
        int healAmount = Mathf.FloorToInt(_healAccumulator);
        if (healAmount > 0)
        {
            currentHealth.Value = Mathf.Min(stats.maxHealth, currentHealth.Value + healAmount);
            _healAccumulator -= healAmount;
        }
    }

    #endregion

    #region Health & Despawn

    private void OnHealthChanged(int previous, int next, bool asServer)
    {
        if (!asServer || base.IsClientOnlyStarted)
            return;
        if (next <= 0)
            DestroyArmyGroup();
    }

    private void DestroyArmyGroup()
    {
        Debug.Log("Army Group Destroyed");
        Despawn();
    }

    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.attackRadius);
    }
}