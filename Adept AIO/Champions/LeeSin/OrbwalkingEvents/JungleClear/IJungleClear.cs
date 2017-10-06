using Aimtec;

namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.JungleClear
{
    internal interface IJungleClear
    {
        void OnPostAttack(AttackableUnit mob);

        void OnUpdate();

        void SmiteMob();
    }
}