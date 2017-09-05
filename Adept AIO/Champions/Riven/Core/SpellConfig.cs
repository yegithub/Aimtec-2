using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class SpellConfig
    {
        public static Spell Q, W, E, R, R2;
     
        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 275);

            W = new Spell(SpellSlot.W, 125);

            E = new Spell(SpellSlot.E, 325);
            E.SetSkillshot(0.1f, 325, int.MaxValue, false, SkillshotType.Line);

            R = new Spell(SpellSlot.R);

            R2 = new Spell(SpellSlot.R, 1100);
            R2.SetSkillshot(0.25f, 100, 1600, false, SkillshotType.Cone);
            Enums.UltimateMode = UltimateMode.First;
        }
    }
}
