using UnityEngine;

public class BattleActionPlayer : MonoBehaviour
{
    [SerializeField] private ActorComponent actor;
    [SerializeField] private BattleEntity target; 
    [SerializeField] private ScriptableBattleAction action;

    public void SetAction(ScriptableBattleAction action) => this.action = action;
    public void SetTarget(BattleEntity target) => this.target = target;
    
    public void StartAction()
    {
        actor.StartAction(action, target); 
    }
}
