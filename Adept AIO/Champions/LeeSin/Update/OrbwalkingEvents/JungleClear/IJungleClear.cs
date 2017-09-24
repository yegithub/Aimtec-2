using Aimtec;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.JungleClear
{
    internal interface IJungleClear
    {
        void OnPostAttack(AttackableUnit mob);

        void OnUpdate();

        void SmiteMob();
    }
}