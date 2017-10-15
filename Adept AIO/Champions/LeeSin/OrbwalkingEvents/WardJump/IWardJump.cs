namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.WardJump
{
    interface IWardJump
    {
        bool Enabled { get; set; }
        void OnKeyPressed();
    }
}