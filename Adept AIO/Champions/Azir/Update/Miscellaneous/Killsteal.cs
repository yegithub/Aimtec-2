using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Update.Miscellaneous
{
    class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range));

            if (target == null || Global.Orbwalker.IsWindingUp || Global.Player.IsDead)
            {
                return;
            }

            if (SpellConfig.Q.Ready
                && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.Q)
                && MenuConfig.Killsteal["Q"].Enabled)
            {
                SpellConfig.CastQ(target);
            }
            else if(SpellConfig.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E)
                    && MenuConfig.Killsteal["E"].Enabled)
            {
                SpellConfig.E.Cast(target);
            }
        }
    }
}
