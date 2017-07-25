using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Killsteal
    {
        public static void OnUpdate()
        {
            foreach (var viableTarget in GameObjects.EnemyHeroes.Where(x => x.HealthPercent() <= 40 && x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.R2.Range))
            {
                if (viableTarget == null || !viableTarget.IsValidTarget())
                { 
                    continue;
                }

                if (SpellConfig.W.Ready
                    && MenuConfig.Killsteal["W"].Enabled
                    && viableTarget.Health <= ObjectManager.GetLocalPlayer().GetSpellDamage(viableTarget, SpellSlot.W)
                    && viableTarget.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.W.Range)
                {
                    SpellManager.CastW(viableTarget);
                }
                else if (SpellConfig.Q.Ready
                    && MenuConfig.Killsteal["Q"].Enabled
                    && viableTarget.Health <= ObjectManager.GetLocalPlayer().GetSpellDamage(viableTarget, SpellSlot.Q)
                    && viableTarget.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.Q.Range)
                {
                    SpellManager.CastQ(viableTarget);
                }
                else if (SpellConfig.R2.Ready
                         && Extensions.UltimateMode == UltimateMode.Second
                         && MenuConfig.Killsteal["R2"].Enabled
                         && viableTarget.Health <= ObjectManager.GetLocalPlayer().GetSpellDamage(viableTarget, SpellSlot.R)
                         && viableTarget.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.R2.Range)
                {
                    SpellManager.CastR2(viableTarget);
                }
                else if (MenuConfig.Killsteal["Items"].Enabled && ObjectManager.GetLocalPlayer().HealthPercent() <= 5)
                {
                    Items.CastTiamat();
                }
                else if (MenuConfig.Killsteal["Ignite"].Enabled && SummonerSpells.Ignite != null && SummonerSpells.Ignite.Ready && viableTarget.Health < SummonerSpells.IgniteDamage)
                {
                    SummonerSpells.Ignite.Cast(viableTarget);
                }
            }
        }
    }
}
