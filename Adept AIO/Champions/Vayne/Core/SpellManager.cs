namespace Adept_AIO.Champions.Vayne.Core
{
    using System;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using SDK.Generic;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;
    using Spell = Aimtec.SDK.Spell;

    class SpellManager
    {
        public static Vector3 DrawingPred;

        public static Spell Q, W, E, R;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 300);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 615);
            E.SetSkillshot(0.5f, 50f, 1200f, false, SkillshotType.Line);
            R = new Spell(SpellSlot.R);
        }

        public static Geometry.Rectangle Rect(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(E.Range))
            {
                return null;
            }

            var pred = E.GetPrediction(target).CastPosition;
            var endPos = target.ServerPosition + (target.ServerPosition - Global.Player.ServerPosition).Normalized() * 430; // nerfed
            return new Geometry.Rectangle(target.ServerPosition.To2D(), endPos.To2D(), target.BoundingRadius);
        }

        public static Geometry.Rectangle Rect(Vector3 target)
        {
            var endPos = Global.Player.ServerPosition + (Global.Player.ServerPosition - target).Normalized() * 430;
            return new Geometry.Rectangle(target.To2D(), endPos.To2D(), 65);
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(E.Range))
            {
                return;
            }

            var rect = Rect(target);

            if (WallExtension.IsWall(rect.Start.To3D(), rect.End.To3D()))
            {
                E.CastOnUnit(target);
            }
        }

        public static void CastQ(Obj_AI_Base target, int modeIndex = 0, bool force = true)
        {
            var wallPos = WallExtension.NearestWall(Global.Player, 150);
            if (!wallPos.IsZero)
            {
                DebugConsole.Write("[DASH] TO WALL", ConsoleColor.Green);
                Q.Cast(wallPos);
                return;
            }

            var pos = Vector3.Zero;

            var point = WallExtension.NearestWall(target, 475);
            point = target.ServerPosition + (target.ServerPosition - point).Normalized() * 100;

            if (force && E.Ready && !point.IsZero && point.Distance(Global.Player) <= Q.Range)
            {
                DebugConsole.Write("[DASH] TO E POS", ConsoleColor.Green);
                pos = point;
            }
            else
            {
                switch (modeIndex)
                {
                    case 0:
                        DebugConsole.Write("[DASH] TO CURSOR", ConsoleColor.Green);
                        pos = Game.CursorPos;
                        break;
                    case 1: 

                        pos = DashManager.DashKite(target, Q.Range);

                        break;
                }
            }

            if (target.Distance(Global.Player) >= Global.Player.AttackRange && Global.Player.CountEnemyHeroesInRange(2000) == 1 && !target.IsFacingUnit(Global.Player))
            {
                DebugConsole.Write("[DASH] FORWARD TO TARGET POSITION", ConsoleColor.Yellow);
                pos = target.ServerPosition;
            }

            Q.Cast(pos);
        }
    }
}