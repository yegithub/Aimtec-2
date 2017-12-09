namespace Adept_AIO.Champions.Riven.Core
{
    using Aimtec;
    using Aimtec.SDK.Prediction.Skillshots;
    using Spell = Aimtec.SDK.Spell;

    class SpellConfig
    {
        public static Spell Q, W, E, R, R2;

        public SpellConfig()
        {
            Q = new Spell(SpellSlot.Q, 275);

            W = new Spell(SpellSlot.W, 260);
            W.SetSkillshot(0.25f, 800, 1500, false, SkillshotType.Circle);

            E = new Spell(SpellSlot.E, 325);
            E.SetSkillshot(0.1f, 325, int.MaxValue, false, SkillshotType.Line);

            R = new Spell(SpellSlot.R);
          
            R2 = new Spell(SpellSlot.R, 1100);
            R2.SetSkillshot(0.25f, 100, 1600, false, SkillshotType.Cone, false, HitChance.VeryHigh);

            Enums.UltimateMode = UltimateMode.First;
        }
    }
}