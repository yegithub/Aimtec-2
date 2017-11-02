namespace Adept_AIO.Champions.Azir.Core
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using Spell = Aimtec.SDK.Spell;

    class SpellConfig
    {
        public static Spell Q, W, E, R;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 740);
            Q.SetSkillshot(0.25f, 70, 1600, false, SkillshotType.Line, false, HitChance.VeryHigh);

            W = new Spell(SpellSlot.W, 500);

            E = new Spell(SpellSlot.E, 1100);
            E.SetSkillshot(0.25f, 100, 1700, false, SkillshotType.Line);

            R = new Spell(SpellSlot.R, 450);
            R.SetSkillshot(0.5f, 300, 1400, false, SkillshotType.Line);
        }

        public static void CastQ(Obj_AI_Base target, bool extend = true)
        {
            foreach (var soldier in SoldierManager.Soldiers)
            {
                if (soldier == null || soldier.ServerPosition == Vector3.Zero)
                {
                    return;
                }

                var pred = Q.GetPrediction(target, soldier.ServerPosition);
                Q.Cast(extend ? soldier.ServerPosition.Extend(pred.CastPosition, target.Distance(soldier) + 200) : pred.CastPosition);
            }
        }

        public static PredictionOutput GetQPred(Obj_AI_Base target)
        {
            var soldier = SoldierManager.GetSoldierNearestTo(target.ServerPosition);
            return soldier == Vector3.Zero ? null : Q.GetPrediction(target, soldier, soldier);
        }
    }
}