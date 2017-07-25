using System;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
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
                    SpellManager.CastW(target);
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
          
            var target = TargetSelector.GetSelectedTarget();
            if (target == null || !MenuConfig.BurstMenu[target.ChampionName].Enabled)
            {
                return;
            }

            var distance = target.Distance(ObjectManager.GetLocalPlayer());

            if (Orbwalker.Implementation.CanAttack() && distance <= ObjectManager.GetLocalPlayer().AttackRange + 65)
            {
                Orbwalker.Implementation.Attack(target);
            }
            
            Extensions.AllIn = SummonerSpells.Flash != null && SummonerSpells.Flash.Ready;

            if (SpellConfig.W.Ready && SpellManager.InsideKiBurst(target))
            {
                SpellManager.CastW(target);
            }

            if (SpellConfig.R.Ready &&
                Extensions.UltimateMode == UltimateMode.First &&
                SpellConfig.E.Ready && distance < Extensions.GetRange())
            {
                SpellConfig.E.Cast(target.ServerPosition);
                SpellConfig.R.Cast();
            }

            if (Environment.TickCount - Extensions.LastETime >= 180 && Extensions.AllIn && distance < 720 && SpellConfig.W.Ready && SpellConfig.R.Ready)
            {
                ObjectManager.GetLocalPlayer().SpellBook.CastSpell(SpellSlot.W);
                SummonerSpells.Flash?.Cast(target.ServerPosition.Extend(ObjectManager.GetLocalPlayer().ServerPosition, target.BoundingRadius));
            }   
            else if (SpellConfig.E.Ready)
            {
                SpellConfig.E.Cast(target);
            }
        }
    }
}
