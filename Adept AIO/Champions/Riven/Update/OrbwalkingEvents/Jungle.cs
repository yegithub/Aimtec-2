using System;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Jungle
    {
        public static void OnPostAttack()
        {
            foreach (var mob in GameObjects.Jungle.Where(x => x.Distance(ObjectManager.GetLocalPlayer()) < Extensions.GetRange() && 
                                                              x.MaxHealth > 5 && 
                                                              x.Health > ObjectManager.GetLocalPlayer().GetAutoAttackDamage(x)))
            {
                if (MenuConfig.Jungle["Check"].Enabled && ObjectManager.GetLocalPlayer().CountEnemyHeroesInRange(1500) >= 1)
                {
                    return;
                }

                if (SpellConfig.Q.Ready && MenuConfig.Jungle["Q"].Enabled)
                {
                    SpellManager.CastQ(mob);
                }
                else
                {
                    if (!MenuConfig.Jungle["W"].Enabled || !SpellConfig.W.Ready || Extensions.CurrentQCount > 1)
                    {
                        return;
                    }

                    SpellManager.CastW(mob);
                }

                if (SpellConfig.E.Ready && MenuConfig.Jungle["E"].Enabled)
                {
                    SpellConfig.E.Cast(mob);
                }
            }
        }

        public static void OnUpdate()
        {
            if (SpellConfig.Q.Ready)
            {
                var legendary = GameObjects.JungleLegendary.FirstOrDefault(x => x.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(x, SpellSlot.Q));
                if (legendary == null)
                {
                    return;
                }

                SpellManager.CastQ(legendary);
            }
        }
    }
}
