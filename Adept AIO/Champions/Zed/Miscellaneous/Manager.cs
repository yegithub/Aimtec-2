namespace Adept_AIO.Champions.Zed.Miscellaneous
{
    using System.Linq;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using OrbwalkingEvents;
    using SDK.Generic;
    using SDK.Unit_Extensions;

    class Manager
    {
        public static void OnUpdate()
        {
            if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            foreach (var gameObject in GameObjects.Minions.Where(x => x.Distance(Global.Player) <= 1000))
            {
                DebugConsole.Write(gameObject.Name);
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
                case OrbwalkingMode.Lasthit:
                    Lasthit.OnUpdate();
                    break;
            }

            if (Global.Orbwalker.Mode != OrbwalkingMode.Mixed && !Global.Player.IsRecalling())
            {
                PermaSpells();
            }
        }

        private static void PermaSpells()
        {
            foreach (var enemy in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget() && x.IsHero))
            {
                if (MenuConfig.Misc["Q"].Enabled && SpellManager.Q.Ready)
                {
                    SpellManager.CastQ(enemy);
                }

                if (MenuConfig.Misc["E"].Enabled && SpellManager.E.Ready)
                {
                    SpellManager.CastE(enemy);
                }
            }
        }
    }
}