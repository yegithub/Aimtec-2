namespace Adept_AIO.Champions.Jhin.OrbwalkerEvents
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Combo
    {
        public static void OnUpdate()
        {
            var target = Global.TargetSelector.GetTarget(SpellManager.R.Range);
            if (target == null)
            {
                return;
            }

            if (SpellManager.Q.Ready && MenuConfig.Combo["Q"].Enabled)
            {
                SpellManager.CastQ(target);
            }

            if (SpellManager.E.Ready && MenuConfig.Combo["E"].Enabled && Game.TickCount - SpellManager.E.LastCastAttemptT > 5000)
            {
                SpellManager.CastE(target);
            }

            if (MenuConfig.Combo["R"].Enabled && (SpellManager.R.Ready && Global.Player.GetSpellDamage(target, SpellSlot.R) * 4 > target.Health || Global.Player.SpellBook.GetSpell(SpellSlot.R).Name.Equals("JhinRShot")))
            {
                SpellManager.CastR(target);
            }
        }
    }
}
