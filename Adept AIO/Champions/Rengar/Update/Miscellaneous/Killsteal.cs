using System.Linq;
using Adept_AIO.Champions.Rengar.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Dmg = Adept_AIO.Champions.Rengar.Core.Dmg;
using MenuConfig = Adept_AIO.Champions.Rengar.Core.MenuConfig;

namespace Adept_AIO.Champions.Rengar.Update.Miscellaneous
{
    class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.E.Range && x.HealthPercent() <= 30);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellConfig.Q.Ready && target.Health < Dmg.Damage(target) || (target.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.Q) &&
                                                                              target.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range &&
                                                                              MenuConfig.Killsteal["Q"].Enabled))
            {
                SpellConfig.Q.Cast(target);
            }
            else if (SpellConfig.W.Ready && target.Health < Dmg.Damage(target) || (target.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.W) &&
                                                                                   target.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.W.Range &&
                                                                                   MenuConfig.Killsteal["W"].Enabled))
            {
                SpellConfig.W.Cast(target);
            }
            else if (SpellConfig.E.Ready && target.Health < Dmg.Damage(target) || (target.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.E) &&
                                                                                   target.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.E.Range &&
                                                                                   MenuConfig.Killsteal["E"].Enabled))
            {
                SpellConfig.E.Cast(target);
            }
        }
    }
}
