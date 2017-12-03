namespace Adept_AIO.Champions.Yorick.Core
{
    using Aimtec;
    using Aimtec.SDK.Prediction.Skillshots;
    using SDK.Unit_Extensions;
    using Spell = Aimtec.SDK.Spell;

    class SpellManager
    {
        public static Spell Q, W, E, R;

        public SpellManager()
        {
            Q = new Spell(SpellSlot.Q);

            W = new Spell(SpellSlot.W, 600f);
            W.SetSkillshot(0.25f, 580, int.MaxValue, false, SkillshotType.Circle);

            E = new Spell(SpellSlot.E, 700);
            E.SetSkillshot(0.25f, 80f, 1800f, false, SkillshotType.Line);

            R = new Spell(SpellSlot.R, 600);
        }

        public static void CastQ(Obj_AI_Base target)
        {
            Q.Cast();
            Global.Orbwalker.ForceTarget(target);
            Global.Orbwalker.ResetAutoAttackTimer();
        }

        public static void CastW(Obj_AI_Base target)
        {
            W.Cast(target.ServerPosition); // Pred with this is garbage.
        }

        public static void CastE(Obj_AI_Base target)
        {
            E.Cast(target); // Pred here is alright. 
        }

        public static void CastR(Vector3 position)
        {
            R.Cast(position);
        }
    }
}