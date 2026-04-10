using System.Collections.Generic;
using UnityEngine;

public abstract class BattleSelectionFilter : ScriptableObject, INestableAsset
{
    public abstract string GetListDisplayName();
    
    public abstract List<BattleEntity> Filter(BattleEntity actor, List<BattleEntity> context);
}