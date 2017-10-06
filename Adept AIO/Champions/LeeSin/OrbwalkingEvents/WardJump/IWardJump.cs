namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.WardJump
{
    internal interface IWardJump
    {
        void OnKeyPressed();
        bool Enabled { get; set; }
    }
}