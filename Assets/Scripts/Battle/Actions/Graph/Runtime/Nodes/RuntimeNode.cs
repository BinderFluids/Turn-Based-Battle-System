using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

[Serializable]
public abstract class RuntimeNode
{
    public List<int> NextNodeIndices = new();
    
    public abstract UniTask Execute(BattleActionDirector ctx, BattleEntity actor, BattleEntity target); 
}