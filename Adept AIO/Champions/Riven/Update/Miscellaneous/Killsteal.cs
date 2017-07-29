using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.HealthPercent() <= 40 && x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.R2.Range);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            if (SpellConfig.W.Ready
                && MenuConfig.Killsteal["W"].Enabled
                && target.Health <= ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.W)
                && target.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.W.Range)
            {
                SpellManager.CastW(target);
            }
            else if (SpellConfig.Q.Ready
                && MenuConfig.Killsteal["Q"].Enabled
                && target.Health <= ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.Q)
                && target.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.Q.Range)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellConfig.R2.Ready
                     && Extensions.UltimateMode == UltimateMode.Second
                     && MenuConfig.Killsteal["R2"].Enabled
                     && target.Health <= ObjectManager.GetLocalPlayer().GetSpellDamage(target, SpellSlot.R)
                     && target.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.R2.Range)
            {
                SpellManager.CastR2(target);
            }
            else if (MenuConfig.Killsteal["Items"].Enabled && ObjectManager.GetLocalPlayer().HealthPercent() <= 5)
            {
                Items.CastTiamat();
            }
            else if (MenuConfig.Killsteal["Ignite"].Enabled && SummonerSpells.Ignite != null && SummonerSpells.Ignite.Ready && target.Health < SummonerSpells.IgniteDamage(target))
            {
                SummonerSpells.Ignite.Cast(target);
            }
        }
    }
}
