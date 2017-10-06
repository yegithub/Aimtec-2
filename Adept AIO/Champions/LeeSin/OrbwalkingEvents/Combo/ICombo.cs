using Aimtec;

namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.Combo
{
    internal interface ICombo
    {
        void OnPostAttack(AttackableUnit target);
        void OnUpdate();
    }
}