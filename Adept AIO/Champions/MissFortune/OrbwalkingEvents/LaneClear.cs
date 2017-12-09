namespace Adept_AIO.Champions.MissFortune.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Damage.JSON;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using SDK.Unit_Extensions;

    class LaneClear
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            var target = args.Target as Obj_AI_Base;
            if (target == null)
            {
                return;
            }

            if (SpellManager.W.Ready && MenuConfig.LaneClear["W"].Enabled && Global.Player.ManaPercent() >= MenuConfig.LaneClear["W"].Value)
            {
                SpellManager.CastW(target);
            }
        }

        public static void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0)
            {
                return;
            }

            var minion = GameObjects.EnemyMinions
                .OrderBy(x => x.Health)
                .ThenBy(x => x.Distance(Global.Player))
                .FirstOrDefault(x => x.IsValidAutoRange() && x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q, DamageStage.Empowered));

            if (minion == null)
            {
                return;
            }

            if (SpellManager.Q.Ready &&
                MenuConfig.LaneClear["Q"].Enabled &&
                Global.Player.ManaPercent() >= MenuConfig.LaneClear["Q"].Value)
            {
                SpellManager.CastExtendedQ(minion);
            }

            if (SpellManager.E.Ready &&
                MenuConfig.LaneClear["E"].Enabled &&
                Global.Player.ManaPercent() >= MenuConfig.LaneClear["E"].Value)
            {
                SpellManager.E.Cast(minion, true, 3);
            }
        }
    }
}