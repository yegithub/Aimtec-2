namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.KickFlash
{
    using Aimtec;

    interface IKickFlash
    {
        bool Enabled { get; set; }
        void OnKeyPressed();
        void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args);
    }
}