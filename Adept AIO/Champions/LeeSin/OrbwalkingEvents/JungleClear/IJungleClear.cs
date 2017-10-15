namespace Adept_AIO.Champions.LeeSin.OrbwalkingEvents.JungleClear
{
    using Aimtec;

    interface IJungleClear
    {
        void OnPostAttack(AttackableUnit mob);

        void OnUpdate();

        void SmiteMob();
    }
}