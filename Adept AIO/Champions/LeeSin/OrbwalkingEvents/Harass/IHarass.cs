namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Harass
{
    using Aimtec;

    interface IHarass
    {
        void OnPostAttack(AttackableUnit target);
        void OnUpdate();
    }
}