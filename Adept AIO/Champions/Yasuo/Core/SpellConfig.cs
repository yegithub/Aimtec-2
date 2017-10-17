namespace Adept_AIO.Champions.Yasuo.Core
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;
    using Spell = Aimtec.SDK.Spell;

    class SpellConfig
    {
        public static Spell Q, W, E, R;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 520);
            Q.SetSkillshot(0.25f, 60, 1600, false, SkillshotType.Line, false, HitChance.None);
            Extension.CurrentMode = Mode.Normal;

            W = new Spell(SpellSlot.W, 400);

            E = new Spell(SpellSlot.E, 475);

            R = new Spell(SpellSlot.R, 1400);
        }

        public static void SetSkill(Mode mode)
        {
            if (mode == Mode.Tornado)
            {
                Q.SetSkillshot(0.25f, 90, 1200, false, SkillshotType.Line, false, HitChance.None);
                Q.Range = 1100;
            }
            else
            {
                Q.SetSkillshot(0.25f, 60, 1600, false, SkillshotType.Line, false, HitChance.None);
                Q.Range = 520;
            }
        }

        public static void CastQ(Obj_AI_Base target)
        {
            var rect = Q3Rect(target);
            if (rect == null || rect.IsOutside(target.ServerPosition.To2D()))
            {
                return;
            }

            Q.Cast(rect.End);
        }

        public static void CastE(Obj_AI_Base target, bool gapclose = false, int rangeGapclose = 0)
        {
            if (!gapclose && MinionHelper.IsDashable(target))
            {
                E.CastOnUnit(target);
                return;
            }

            var minion = MinionHelper.GetDashableMinion(target);

            var positionBehindMinion = MinionHelper.WalkBehindMinion(target);
            var closestToPlayer = MinionHelper.GetClosest(target);

            if (closestToPlayer != null && MinionHelper.IsDashable(closestToPlayer) && closestToPlayer.Distance(Global.Player) <= rangeGapclose)
            {
                Global.Orbwalker.Move(positionBehindMinion);

                if (closestToPlayer.Distance(Global.Player) <= 65)
                {
                    E.CastOnUnit(closestToPlayer);
                }
            }
            else if (minion != null && MinionHelper.IsDashable(minion))
            {
                E.CastOnUnit(minion);
            }
        }

        public static Geometry.Rectangle Q3Rect(Obj_AI_Base target) => new Geometry.Rectangle(Global.Player.ServerPosition.To2D(),
            Global.Player.ServerPosition.Extend(Q.GetPrediction(target).CastPosition, Q.Range).To2D(),
            Q.Width);
    }
}