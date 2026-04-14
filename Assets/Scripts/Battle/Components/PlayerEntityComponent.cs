using Battle.BattleEntity;
using UnityEngine;

public class PlayerEntityComponent : BattleEntityComponent
{
    [SerializeField] private PlayerId playerID;
    public PlayerId PlayerID => playerID;
    [SerializeField] private BattleInputReader input; 
    private BoolInputData inputData;
    public BoolInputData InputData => inputData;

    protected override void Start()
    {
        base.Start();
        inputData = playerID switch
        {
            PlayerId.PlayerOne => input.PlayerOne,
            PlayerId.PlayerTwo => input.PlayerTwo,
            _ => throw new System.NotImplementedException()
        };
    }
}
