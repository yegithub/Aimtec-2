using Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Tristana.Update.Miscellaneous
{
    internal class Manager
    {
        private readonly Combo _combo;
        private readonly Harass _harass;
        private readonly LaneClear _laneClear;
        private readonly JungleClear _jungleClear;

        public Manager(Combo combo, Harass harass, LaneClear laneClear, JungleClear jungleClear)
        {
            _combo = combo;
            _harass = harass;
            _laneClear = laneClear;
            _jungleClear = jungleClear;
        }

        public void OnPostAttack(object sender, PostAttackEventArgs args)
        {
            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Laneclear:
                    _jungleClear.OnPostAttack(args.Target);
                    _laneClear.OnPostAttack();
                    break;
            }
        }

        public void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return; 
            }

            switch (Global.Orbwalker.Mode)
            {
                case OrbwalkingMode.Combo:
                    _combo.OnUpdate();
                    break;
                case OrbwalkingMode.Mixed:
                    _harass.OnUpdate();
                    break;
                    case OrbwalkingMode.Laneclear:
                    _laneClear.OnUpdate();
                    _jungleClear.OnUpdate();
                    break;
            }
        }
    }
}
