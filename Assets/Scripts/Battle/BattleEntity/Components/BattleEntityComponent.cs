using System;
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

    private void Start() => entity ??= GetComponent<BattleEntity>();
}