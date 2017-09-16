using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Azir.Core
{
    class SpellConfig
    {
        public static Spell Q, W, E, R;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 875);
            Q.SetSkillshot(0.25f, 70, 1600, false, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 450);
          
            E = new Spell(SpellSlot.E, 1100);
            E.SetSkillshot(0.25f, 100, 1700, false, SkillshotType.Line);

            R = new Spell(SpellSlot.R, 450);
            R.SetSkillshot(0.5f, 300, 1400, false, SkillshotType.Line, false, HitChance.VeryHigh);
        }

        public static void CastQ(Obj_AI_Base target)
        {
            var soldier = SoldierHelper.Soldiers.FirstOrDefault(x => x.Distance(target) <= Q.Range);
            if (soldier == null)
            {
                return;
            }

            Q.GetPrediction(target, soldier.ServerPosition, soldier.ServerPosition);
        }
    }
}
