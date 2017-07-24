using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Yasuo.Update.Miscellaneous
{
    class Killsteal
    {
        public static void OnUpdate()
        {
            foreach (var target in GameObjects.EnemyHeroes.Where(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.R.Range && x.HealthPercent() <= 40))
            {
                if (target == null || !target.IsValidTarget())
                {
                    return;
                }

                if (SpellConfig.Q.Ready && target.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.Q) &&
                    target.IsValidTarget(SpellConfig.Q.Range) &&
                    MenuConfig.Killsteal["Q"].Enabled)
                {
                    if (Extension.CurrentMode == Mode.Tornado && !MenuConfig.Killsteal["Q3"].Enabled)
                    {
                        return;
                    }

                    SpellConfig.Q.Cast(target);
                }
                else if (SpellConfig.E.Ready && target.Health <
                         ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.E) &&
                         target.IsValidTarget(SpellConfig.E.Range) &&
                        !target.HasBuff("YasuoDashWrapper") &&
                         MenuConfig.Killsteal["E"].Enabled)
                {
                    SpellConfig.E.CastOnUnit(target);
                }
            }
        }
    }
}
