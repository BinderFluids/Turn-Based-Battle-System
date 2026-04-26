namespace Battle.Window
{
    public struct PlayerHoldData
    {
        public int PressFrame;
        public int ReleaseFrame;
        public bool HasPressed => PressFrame > 0;
        public bool HasReleased => ReleaseFrame > 0;
        public int Duration => ReleaseFrame - PressFrame;
    }
}