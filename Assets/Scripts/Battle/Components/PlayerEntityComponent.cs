using UnityEngine;

public class PlayerEntityComponent : BattleEntityComponent
{
    enum PlayerInputData {PlayerOne, PlayerTwo}
    
    [SerializeField] private PlayerInputData playerInputData;
    [SerializeField] private BattleInputReader input; 
    [SerializeField] private BoolInputData inputData;
    public BoolInputData InputData => inputData;

    protected override void Start()
    {
        base.Start();
        inputData = playerInputData switch
        {
            PlayerInputData.PlayerOne => input.PlayerOne,
            PlayerInputData.PlayerTwo => input.PlayerTwo,
            _ => throw new System.NotImplementedException()
        };
    }
}
