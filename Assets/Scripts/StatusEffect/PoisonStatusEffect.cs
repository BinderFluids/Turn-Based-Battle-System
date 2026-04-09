public class PoisonStatusEffect : IStatusEffect
{
    public void OnApply()
    {
        //subscribe to the turn events
    }

    void OnTurnStart()
    {
        //if the active entity is the entity that this effect is applied to
        //damage entity
    }

    public void OnRemove()
    {
        throw new System.NotImplementedException();
    }
}

public class BlueMushroom : IStatusEffect
{
    private PercentageStatModifier hpDebuff; 
    private StatBlock test;
    
    public void OnApply()
    {
        //get the hp of the entity that this effect is applied to
        //add a -10% hp buff to the entity
        test = new StatBlock();
        
        hpDebuff = new PercentageStatModifier(.9f);
        test.HP.AddModifier(hpDebuff);
    }

    public void OnRemove()
    {
        //remove the previous hp
        test.HP.RemoveModifier(hpDebuff);
    }
}