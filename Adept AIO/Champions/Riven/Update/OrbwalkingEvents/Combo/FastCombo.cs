using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents.Combo
{
    internal class FastCombo
    {
        public static void OnPostAttack(Obj_AI_Base target)
        {
            if (SpellConfig.R2.Ready
             && Enums.UltimateMode == UltimateMode.Second
             && MenuConfig.Combo["R2"].Enabled
             && target.HealthPercent() <= 40)
            {
                SpellManager.CastR2(target);
            }

            if (SpellManager.InsideKiBurst(target) && SpellConfig.W.Ready)
            {
                SpellConfig.W.Cast();
            }

            if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }
        }

        public static void OnUpdate(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            if (SpellConfig.Q.Ready && target.IsInRange(Global.Player.AttackRange))
            {
                Global.Orbwalker.Attack(target); // Prevents E WQ (1s delay) -> AA. (BUG)
            }

            if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target.ServerPosition);
            }

            if (ComboManager.CanCastR1(target))
            {
                SpellConfig.R.Cast();
            }
        }
    }
}
