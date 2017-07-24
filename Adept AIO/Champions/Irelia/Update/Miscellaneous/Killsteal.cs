using System.Linq;
using Adept_AIO.Champions.Irelia.Core;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Irelia.Update.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            foreach (var target in GameObjects.EnemyHeroes.Where(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.R.Range && x.HealthPercent() <= 40))
            {
                if (target == null || !target.IsValidTarget())
                {
                    return;
                }
             
                if (SpellConfig.Q.Ready && target.Health < Dmg.Damage(target) || (target.Health <
                    ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.Q) &&
                    target.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range &&
                    MenuConfig.Killsteal["Q"].Enabled))
                {
                    SpellConfig.Q.CastOnUnit(target);
                }
                else if (SpellConfig.E.Ready && target.Health <
                    ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.E) &&
                    target.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.E.Range &&
                    MenuConfig.Killsteal["E"].Enabled)
                {
                    SpellConfig.E.CastOnUnit(target);
                }
                else if (SpellConfig.R.Ready && target.Health <
                         ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.R) &&
                         MenuConfig.Killsteal["R"].Enabled)
                {
                    SpellConfig.R.CastOnUnit(target);
                }
            }
        }
    }
}
