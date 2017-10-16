namespace Adept_AIO.Champions.Lucian.Core
{
    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Events;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using SDK.Generic;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;
    using Spell = Aimtec.SDK.Spell;

    class SpellManager
    {
        public static Spell Q, W, E, R;
        public static float ExtendedRange = 900;

        public SpellManager()
        {
            Q = new Spell(SpellSlot.Q, 500);
            Q.SetSkillshot(0.25f, 65, float.MaxValue, false, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 900);
            W.SetSkillshot(0.30f, 80, 1600, false, SkillshotType.Line, false, HitChance.None);

            E = new Spell(SpellSlot.E, 425);

            R = new Spell(SpellSlot.R, 1200);
            R.SetSkillshot(0.25f, 110f, 2500, false, SkillshotType.Line, false, HitChance.None);
        }

        public static Geometry.Rectangle GetQRectangle(Obj_AI_Base target) => new Geometry.Rectangle(
            Global.Player.ServerPosition.To2D(),
            Global.Player.ServerPosition.Extend(target.ServerPosition, ExtendedRange).To2D(),
            Q.Width);

        public static Geometry.Rectangle GetRRectangle(Obj_AI_Base target)
        {
            var pred = R.GetPrediction(target).CastPosition.To2D();
            return new Geometry.Rectangle(Global.Player.ServerPosition.To2D(),
                Global.Player.ServerPosition.Extend(pred, R.Range).To2D(),
                R.Width);
        }

        public static void CastQ(Obj_AI_Base target, int minHit = -1)
        {
            if (minHit == -1)
            {
                Q.CastOnUnit(target);
            }
            else if (!Global.Player.IsDashing())
            {
                var rect = GetQRectangle(target);
          
                if (Q.GetPrediction(target).HitChance >= HitChance.High &&
                    GameObjects.EnemyMinions.Count(x => x.IsValidTarget() && rect.IsInside(x.ServerPosition.To2D())) >= minHit)
                {
                    Q.CastOnUnit(target);
                }
            }
        }

        public static void CastQExtended(Obj_AI_Base target)
        {
            var rect = GetQRectangle(target);
            var m = GameObjects.EnemyMinions.FirstOrDefault(x => x.IsValidTarget(ExtendedRange) && rect.IsInside(x.ServerPosition.To2D()));
            if (m == null)
            {
                return;
            }

            if (rect == null || Q.GetPrediction(target, m.ServerPosition, m.ServerPosition).HitChance != HitChance.High)
            {
                return;
            }

            Q.CastOnUnit(m);
        }

        public static void CastE(Obj_AI_Base target, int modeIndex = 0)
        {
            var pos = Vector3.Zero;

            switch (modeIndex)
            {
                case 0:
                    pos = Game.CursorPos;
                    break;
                default:
                    for (var i = 140; i < 360; i += 20)
                    {
                        var dir = Global.Player.Orientation.To2D();
                        var angleRad = Maths.DegreeToRadian(i);
                        var rot = (Global.Player.ServerPosition.To2D() + 300 * dir.Rotated((float) angleRad)).To3D();
                        if (rot.CountEnemyHeroesInRange(400) != 0 || rot.PointUnderEnemyTurret())
                        {
                            continue;
                        }
                        DebugConsole.Write("TO SIDE", ConsoleColor.Green);
                        pos = rot;
                    }
                    break;
            }

            E.Cast(pos);
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (GameObjects.EnemyMinions.Count(x =>
                    x.IsValidTarget() && GetRRectangle(x).IsInside(x.ServerPosition.To2D())) <=
                2 &&
                Game.TickCount - R.LastCastAttemptT >= 10000)
            {
                R.Cast(target);
            }
        }
    }
}