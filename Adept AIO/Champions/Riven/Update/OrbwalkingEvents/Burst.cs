using System;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Burst
    {
        public static void OnPostAttack(Obj_AI_Base target)
        {
            if (SpellConfig.R.Ready && Extensions.UltimateMode == UltimateMode.Second)
            {
                SpellConfig.R2.Cast(target);
                DelayAction.Queue(1, () =>
                {
                    SpellManager.CastQ(target); 
                    SpellManager.CastW(target); // Extra check
                });
            }
            else if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }
            else if (SpellConfig.W.Ready)
            {
                SpellManager.CastW(target);
            }
        }


        public static void OnUpdate()
        {
            var target = GlobalExtension.TargetSelector.GetSelectedTarget();
            if (target == null || !MenuConfig.BurstMenu[target.ChampionName].Enabled)
            {
                return;
            }

            var distance = target.Distance(GlobalExtension.Player);

            if (GlobalExtension.Orbwalker.CanAttack() && distance <= GlobalExtension.Player.AttackRange + 65)
            {
                GlobalExtension.Orbwalker.Attack(target);
            }
            
            Extensions.AllIn = SummonerSpells.Flash != null && SummonerSpells.Flash.Ready;

            if (SpellConfig.W.Ready && SpellManager.InsideKiBurst(target))
            {
                SpellManager.CastW(target);
            }

            if (SpellConfig.R.Ready &&
                Extensions.UltimateMode == UltimateMode.First &&
                SpellConfig.E.Ready && distance < Extensions.FlashRange(target))
            {
                SpellConfig.E.Cast(target.ServerPosition);
                SpellConfig.R.Cast();
            }

            if (Environment.TickCount - Extensions.LastETime >= 180 && Extensions.AllIn && distance < Extensions.FlashRange(target) && SpellConfig.W.Ready && SpellConfig.R.Ready)
            {
                GlobalExtension.Player.SpellBook.CastSpell(SpellSlot.W);
                SummonerSpells.Flash?.Cast(target.ServerPosition.Extend(GlobalExtension.Player.ServerPosition, target.BoundingRadius));
            }   
            else if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target);
            }
        }
    }
}
