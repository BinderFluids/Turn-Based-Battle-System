namespace Selectable
{
    public interface ISelectableInput
    {
        BoolInputData Confirm { get; }
        Vector2InputData Navigate { get; }
    }
}