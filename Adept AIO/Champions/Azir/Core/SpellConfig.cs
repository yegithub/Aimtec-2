using System;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Azir.Core
{
    class SpellConfig
    {
        public static Spell Q, W, E, R;
        public static double RSqrt => 450;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 875);
            Q.SetSkillshot(0.25f, 70, 1600, false, SkillshotType.Line, false, HitChance.VeryHigh);

            W = new Spell(SpellSlot.W, 450);
          
            E = new Spell(SpellSlot.E, 1100);
            E.SetSkillshot(0.25f, 100, 1700, false, SkillshotType.Line);

            R = new Spell(SpellSlot.R, 450);
            R.SetSkillshot(0.5f, 300, 1400, false, SkillshotType.Line);
        }

        public static void CastQ(Obj_AI_Base target, bool extend = true)
        {
            foreach (var soldier in SoldierHelper.Soldiers)
            {
                if (soldier == null || soldier.ServerPosition == Vector3.Zero)
                {
                    return;
                }

                var pred = Q.GetPrediction(target, soldier.ServerPosition);
                Q.Cast(extend ? soldier.ServerPosition.Extend(pred.CastPosition, target.Distance(soldier) + 200) : pred.CastPosition);
            }
          
        }

        public static Vector3 GetQPred(Obj_AI_Base target)
        {
            var soldier = SoldierHelper.GetSoldierNearestTo(target.ServerPosition);
            return soldier == Vector3.Zero ? Vector3.Zero : Q.GetPrediction(target, soldier, soldier).CastPosition;
        }
    }
}
