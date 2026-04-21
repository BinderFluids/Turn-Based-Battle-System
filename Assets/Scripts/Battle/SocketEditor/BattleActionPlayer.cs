using Battle.Actions.Graph.Runtime;
using UnityEngine;

public class BattleActionPlayer : MonoBehaviour
{
    [SerializeField] private BattleEntity actor; 
    [SerializeField] private BattleEntity target;
    [SerializeField] private ScriptableBattleAction action;

    public void SetActor(BattleEntity actor) => this.actor = actor;

    public void PlayGraph(BattleActionRuntimeGraph graph)
    {
        Debug.Log("try play graph");
        
        if (actor.TryGetComponent(out BattleActionDirector director))
        {
            Debug.Log("Playing graph");
            
            director.RuntimeGraph = graph;
            director.StartAction(actor, target);
        }
    }

    public void PlayAction(ScriptableBattleAction action)
    {
        Debug.Log("try play action");   
        if (actor.TryGetComponent(out ActorComponent actorComponent))
        {
            Debug.Log("Playing action");
            
            actorComponent.StartAction(action, target);
        }
    }
    
    
}
