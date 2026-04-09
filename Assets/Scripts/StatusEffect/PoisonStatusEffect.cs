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