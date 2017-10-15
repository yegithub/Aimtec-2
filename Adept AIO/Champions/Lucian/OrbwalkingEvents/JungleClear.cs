namespace Adept_AIO.Champions.Lucian.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Orbwalking;
    using Core;
    using SDK.Unit_Extensions;

    class JungleClear
    {
        public static void PostAttack(object sender, PostAttackEventArgs args)
        {
            var target = args.Target as Obj_AI_Base;

            if (SpellManager.E.Ready && MenuConfig.JungleClear["E4"].Value == 0)
            {
                SpellManager.CastE(target, MenuConfig.JungleClear["Mode4"].Value);
            }
            else if (SpellManager.Q.Ready && MenuConfig.JungleClear["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellManager.W.Ready && MenuConfig.JungleClear["W"].Enabled)
            {
                SpellManager.W.Cast(target);
            }
        }

        public static void OnUpdate()
        {
            var target = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(SpellManager.ExtendedRange));
            if (target != null)
            {
                if (SpellManager.E.Ready && MenuConfig.JungleClear["E4"].Value == 1)
                {
                    SpellManager.CastE(target, MenuConfig.JungleClear["Mode4"].Value);
                }
            }
        }
    }
}