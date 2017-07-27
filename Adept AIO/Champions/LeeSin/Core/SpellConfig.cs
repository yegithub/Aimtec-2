using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.LeeSin.Core
{
    class SpellConfig
    {
        public static Spell Q, W, E, R, R2;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            Q.SetSkillshot(0.25f, 60, 1800, false, SkillshotType.Circle, false, HitChance.Medium);

            W = new Spell(SpellSlot.W, 700);
         
            E = new Spell(SpellSlot.E, 350);

            R = new Spell(SpellSlot.R, 375);
            R2 = new Spell(SpellSlot.R, 1200);
            R2.SetSkillshot(0.25f, 100, 1600, true, SkillshotType.Line);
        }
    }
}
