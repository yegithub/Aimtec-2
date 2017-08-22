using Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Tristana.Update.Miscellaneous
{
    internal class Manager
    {
        private readonly Combo Combo;
        private readonly Harass Harass;
        private readonly LaneClear LaneClear;
        private readonly JungleClear JungleClear;

        public Manager(Combo combo, Harass harass, LaneClear laneClear, JungleClear jungleClear)
        {
            Combo = combo;
            Harass = harass;
            LaneClear = laneClear;
            JungleClear = jungleClear;
        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Laneclear:
                    JungleClear.OnPostAttack(args.Target);
                    LaneClear.OnPostAttack();
                    break;
            }
        }

        public void OnUpdate()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    Combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    Harass.OnUpdate();
                    break;
                    case OrbwalkingMode.Laneclear:
                    LaneClear.OnUpdate();
                    JungleClear.OnUpdate();
                    break;
            }
        }
    }
}
