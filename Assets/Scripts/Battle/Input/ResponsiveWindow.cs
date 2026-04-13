namespace Battle.Input
{
    /// <summary>
    /// Parameters for a responsive window (closed explicitly by id, not by duration).
    /// </summary>
    public class ResponsiveWindow : Window
    {
        public ResponsiveWindow(string id, InputType expectedInputs)
            : base(id, expectedInputs)
        { }

        public override void HandleInput(WindowInputEvent evt)
        {
            throw new System.NotImplementedException();
        }
    }
}
