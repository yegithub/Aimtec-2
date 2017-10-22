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

        public SpellManager()
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

            var endPos = target.ServerPosition + (target.ServerPosition - Global.Player.ServerPosition).Normalized() * 475;
            return new Geometry.Rectangle(target.ServerPosition.To2D(), endPos.To2D(), target.BoundingRadius);
        }

        public static Geometry.Rectangle PredRect(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(E.Range))
            {
                return null;
            }

            var pred = E.GetPrediction(target).CastPosition;
            var endPos = pred + (pred - Global.Player.ServerPosition).Normalized() * 475;
            return new Geometry.Rectangle(target.ServerPosition.To2D(), endPos.To2D(), target.BoundingRadius);
        }

        public static bool CanStun(Obj_AI_Base target)
        {
            var rect = Rect(target);
            var predRect = PredRect(target);

            if (WallExtension.IsWall(rect.Start.To3D(), rect.End.To3D()) && WallExtension.IsWall(predRect.Start.To3D(), predRect.End.To3D()))
            {
                return true;
            }
            return false;
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(E.Range))
            {
                return;
            }

            if (CanStun(target))
            {
                E.CastOnUnit(target);
            }
        }

        public static void CastQ(Obj_AI_Base target, int modeIndex = 0, bool force = true)
        {
            var wallPos = WallExtension.NearestWall(Global.Player, 130);
            if (!wallPos.IsZero)
            {
                DebugConsole.Write("[DASH] TO WALL", ConsoleColor.Green);
                Q.Cast(wallPos);
                return;
            }

            var point = WallExtension.NearestWall(target, 475);           

            if (force && E.Ready && !point.IsZero)
            {
                point = target.ServerPosition + (target.ServerPosition - point).Normalized() * 100;

                if (point.Distance(Global.Player) <= Q.Range)
                {
                    DebugConsole.Write("[DASH] TO E POS", ConsoleColor.Green);
                    Q.Cast(point);
                }
            }

            switch (modeIndex)
            {
                case 0:
                    DebugConsole.Write("[DASH] TO CURSOR", ConsoleColor.Green);
                    Q.Cast(Game.CursorPos);
                    break;
                case 1:
                    DebugConsole.Write("[DASH] KITING", ConsoleColor.Green);
                    Q.Cast(DashManager.DashKite(target, Q.Range));
                    break;
            }


          
        }
    }
}