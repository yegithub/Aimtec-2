namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Combo
{
    using Aimtec;

    interface ICombo
    {
        void OnPostAttack(AttackableUnit target);
        void OnUpdate();
    }
}