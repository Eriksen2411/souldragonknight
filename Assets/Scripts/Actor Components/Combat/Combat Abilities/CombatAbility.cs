using UnityEngine;

public enum CombatAbilityIdentifier
{
    ATTACK_MELEE,
    ATTACK_RANGED,
    BLOCK,
    DODGE
}

public abstract class CombatAbility : MonoBehaviour
{
    public abstract void Execute(Combat combat, params object[] parameters);

    public virtual void End(Combat combat) { }
}
