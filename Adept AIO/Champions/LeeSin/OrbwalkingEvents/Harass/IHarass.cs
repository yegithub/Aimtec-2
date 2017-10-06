using Aimtec;

namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Harass
{
    internal interface IHarass
    {
        void OnPostAttack(AttackableUnit target);
        void OnUpdate();
    }
}
