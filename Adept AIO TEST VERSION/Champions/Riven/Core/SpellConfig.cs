using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Riven.Core
{
    internal class SpellConfig
    {
        public static Spell Q, W, E, R, R2;
      
        /// <summary>
        /// Instances the spells
        /// </summary>
        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 275); // Radius: [112.5 | 150 | 162.5 | 200] --> Q1/Q2 | Q3 | R(Q1/Q2) | R(Q3) ~(I THINK!)~
           
            W = new Spell(SpellSlot.W, 125);

            E = new Spell(SpellSlot.E, 325);
            E.SetSkillshot(0.25f, 325, int.MaxValue, false, SkillshotType.Line);

            R = new Spell(SpellSlot.R);

            R2 = new Spell(SpellSlot.R, 900);
            R2.SetSkillshot(0.25f, 45, 1600, false, SkillshotType.Cone);
            Extensions.UltimateMode = UltimateMode.First;
        }
    }
}
