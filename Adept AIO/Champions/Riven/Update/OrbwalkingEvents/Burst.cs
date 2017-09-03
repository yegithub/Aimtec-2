using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Burst
    {
        public static void OnPostAttack(Obj_AI_Base target)
        {
            if (SpellConfig.R2.Ready)
            {
                SpellConfig.R2.CastOnUnit(target);
            }
            else if (SpellConfig.Q.Ready)
            {
                SpellManager.CastQ(target);
            }
        }

        public static void OnUpdate()
        {
            var target = MenuConfig.BurstMode.GetTarget() as Obj_AI_Hero;
            if (target == null || !MenuConfig.BurstMenu[target.ChampionName].Enabled)
            {
                return;
            }
  
            var distance = target.Distance(Global.Player);

            Extensions.AllIn = SummonerSpells.IsValid(SummonerSpells.Flash);

            if (Extensions.AllIn)
            {
                if (SpellConfig.W.Ready && SpellManager.InsideKiBurst(target))
                {
                    SpellManager.CastW(target);
                }

                if (SpellConfig.R.Ready &&
                    Extensions.UltimateMode == UltimateMode.First &&
                    SpellConfig.E.Ready && distance < Extensions.FlashRange())
                {
                    SpellConfig.E.Cast(target.ServerPosition);
                    SpellConfig.R.Cast();
                }

                if (distance < Extensions.FlashRange() && SpellConfig.W.Ready && SpellConfig.R.Ready && SummonerSpells.IsValid(SummonerSpells.Flash))
                {
                    Global.Player.SpellBook.CastSpell(SpellSlot.W);
                    SummonerSpells.Flash.Cast(target.ServerPosition.Extend(Global.Player.ServerPosition, target.BoundingRadius));
                }
                else if (SpellConfig.E.Ready)
                {
                    SpellConfig.E.Cast(target);
                }
            }
            else if(target.IsValidTarget(SpellConfig.E.Range + Global.Player.AttackRange))
            {
                if (SpellConfig.E.Ready)
                {
                    SpellConfig.E.Cast(target.ServerPosition);

                    if (SpellConfig.R.Ready && Extensions.UltimateMode == UltimateMode.First)
                    {
                        SpellConfig.R.Cast();
                    }
                }

                if (SpellConfig.W.Ready)
                {
                    SpellManager.CastW(target);
                }
            }
        }
    }
}
