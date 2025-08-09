using UnityEngine;

[CreateAssetMenu(fileName = "ArmyStats", menuName = "Scriptable Objects/ArmyStats")]
public class ArmyStats : ScriptableObject
{
    [Header("Health")]
    public int maxHealth = 100_000;

    [Header("Combat")]
    public float attackRadius = 50f;
    public int damagePerSecond = 200;
    public int customizableDamage = 100;
    public float customizableDamageCooldown = 10f;

    [Header("Healing")]
    [Tooltip("Health per second when out of combat")]
    public int passiveHealing = 50;
}
