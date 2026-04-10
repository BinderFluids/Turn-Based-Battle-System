using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class BuildableBattleActionStep : ScriptableObject, INestableAsset
{
    private BuildableBattleAction parentAction;
    public BuildableBattleAction ParentAction => parentAction;
    
    private bool isInitialized;
    public bool IsInitialized => isInitialized;
    
    public void Initialize(BuildableBattleAction parentAction)
    {
        if (isInitialized) return;
        
        this.parentAction = parentAction; 
        
        OnInitialize();
        
        isInitialized = true;
    }
    protected virtual void OnInitialize() { }
    
    public abstract UniTask Execute(BattleEntity actor, BattleEntity target);
    public abstract string GetListDisplayName();
}