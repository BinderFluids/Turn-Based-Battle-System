using System.Collections.Generic;

public class BattleEntityModifierFilterChain : FluentChain<List<BattleEntity>, List<BattleEntity>, BattleEntityModifierFilterChain>
{
    public BattleEntityModifierFilterChain(IProcessor<List<BattleEntity>, List<BattleEntity>> processor) : base(processor) { }
    
    static BattleEntityModifierFilterChain Create(IProcessor<List<BattleEntity>, List<BattleEntity>> processor)
    {
        return new BattleEntityModifierFilterChain(processor);
    }
    
    public BattleEntityModifierFilterChain Then<TProcessor>(TProcessor next) where TProcessor : class, IProcessor<List<BattleEntity>, List<BattleEntity>>
    {
        return Then<List<BattleEntity>, BattleEntityModifierFilterChain, TProcessor>(next, Create);
    }
}