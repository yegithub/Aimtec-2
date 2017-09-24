using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Ezreal.Core
{
    internal class SpellConfig
    {
        public static Spell Q, W, E, R;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 1200);
            Q.SetSkillshot(0.25f, 60, 2000, true, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 1050);
            W.SetSkillshot(0.5f, 80, 1600, false, SkillshotType.Line, false, HitChance.None);

            E = new Spell(SpellSlot.E, 900);
           
            R = new Spell(SpellSlot.R, int.MaxValue);
            R.SetSkillshot(1, 160, 2000, false, SkillshotType.Line);
        }
    }
}
