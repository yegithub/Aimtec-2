namespace Adept_AIO.Champions._1._Template.Core
{
    using Aimtec;
    using Aimtec.SDK.Prediction.Skillshots;
    using Spell = Aimtec.SDK.Spell;

    class SpellManager
    {
        public static Vector3 WallForQ;
        public static Spell Q, W, E, R;

        public SpellManager()
        {
            Q = new Spell(SpellSlot.Q, 950);
            Q.SetSkillshot(0.25f, 70f, 900f, false, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 850f);
            W.SetSkillshot(0.25f, 250f, 1650f, false, SkillshotType.Circle);

            E = new Spell(SpellSlot.E, 425f);
        
            R = new Spell(SpellSlot.R, 1000f);
            R.SetSkillshot(0.25f, 100f, 2100, false, SkillshotType.Line);
        }

        public static void CastQ(Obj_AI_Base target)
        {
          
        }

        public static void CastW(Obj_AI_Base target)
        {
          
        }

        public static void CastE(Obj_AI_Base target)
        {
         
        }

        public static void CastR(Obj_AI_Base target)
        {
           
        }
    }
}