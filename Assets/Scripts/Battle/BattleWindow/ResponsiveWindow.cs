using System.Collections.Generic;

namespace Battle
{
    /// <summary>
    /// Parameters for a responsive window (closed explicitly by id, not by duration).
    /// </summary>
    public class ResponsiveWindow : Window
    {
        public ResponsiveWindow(string id,  List<PlayerId> expectedPlayerInputs)
            : base(id, expectedPlayerInputs)
        { }

        public override void HandleInput(PlayerId playerId, bool isPressed)
        {
            throw new System.NotImplementedException();
        }
    }
}
