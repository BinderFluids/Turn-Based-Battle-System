using UnityEngine;

public interface IBattleEntityComponent
{
    BattleEntity Entity { get; }
}

[RequireComponent(typeof(BattleEntity))]
public abstract class BattleEntityComponent : MonoBehaviour, IBattleEntityComponent
{
    [SerializeField] private BattleEntity entity;
    public BattleEntity Entity => entity;


    protected virtual void Awake() { }

    protected virtual void Start() => entity ??= GetComponent<BattleEntity>();
}