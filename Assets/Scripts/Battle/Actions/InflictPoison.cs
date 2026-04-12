using StatusEffectSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Battle Action/Inflict Status", fileName = "InflictStatus")]
public class InflictPoison : ScriptableBattleAction
{
    [SerializeField] private int damage;
    [SerializeField] private int duration; 
    
    public override void StartAction(BattleEntity actor, BattleEntity target)
    {
        var statusEffectInstance = new PoisonStatusEffect(damage, duration);
        target.AddStatus(statusEffectInstance);
        EndAction(actor);
    }
}