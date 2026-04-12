using System.Collections.Generic;
using System.Linq;
using StatusEffectSystem;

public partial class BattleEntity
{
    private List<StatusEffect> statuses = new();
    public IReadOnlyList<StatusEffect> Statuses => statuses;
    public void AddStatus(StatusEffect status)
    {
        if (statuses.Any(s => s.GetType() == status.GetType())) return; 
    
        statuses.Add(status);
        status.Apply(this); 
    }
    public void RemoveStatus(StatusEffect status)
    {
        if (!statuses.Contains(status)) return;
        if (status is null) return;
    
        statuses.Remove(status);
        status.Remove(this);
    }
}