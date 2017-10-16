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
        public static Vector3 DrawingPred, QPred;

        public static Spell Q, W, E, R;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 300);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 615);
            E.SetSkillshot(0.5f, 50f, 1200f, false, SkillshotType.Line);
            R = new Spell(SpellSlot.R);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SpellSlot == SpellSlot.E)
            {
                Maths.DisableAutoAttack(550 + Game.Ping / 2);
            }
        }

        public static Geometry.Rectangle Rect(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(E.Range))
            {
                return null;
            }

            var pred = E.GetPrediction(target).CastPosition;
            var endPos = pred + (pred - Global.Player.ServerPosition).Normalized() * 475;
            return new Geometry.Rectangle(target.ServerPosition.To2D(), endPos.To2D(), target.BoundingRadius);
        }

        public static Geometry.Rectangle Rect(Vector3 target)
        {
            var endPos = target + (target - Global.Player.ServerPosition).Normalized() * 475;
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
            var wallPos = WallExtension.NearestWall(Global.Player.ServerPosition);
            if (!wallPos.IsZero)
            {
                DebugConsole.Write("TO WALL", ConsoleColor.Green);
                QPred = wallPos;
                Q.Cast(wallPos);
                return;
            }

            var pos = Vector3.Zero;

            var point = WallExtension.NearestWall(target.ServerPosition);

            if (force && E.Ready && !point.IsZero && point.Distance(Global.Player) <= Q.Range)
            {
                point = target.ServerPosition + (target.ServerPosition - point).Normalized() * 200;
                DebugConsole.Write("TO E POS", ConsoleColor.Green);
                pos = point;
            }
            else
            {
                switch (modeIndex)
                {
                    case 0:
                        DebugConsole.Write("TO CURSOR", ConsoleColor.Green);
                        pos = Game.CursorPos;
                        break;
                    case 1:

                        for (var i = 140; i < 360; i += 20)
                        {
                            var dir = Global.Player.Orientation.To2D();
                            var angleRad = Maths.DegreeToRadian(i);
                            var rot = (Global.Player.ServerPosition.To2D() + 300 * dir.Rotated((float) angleRad)).
                                To3D();
                            if (rot.CountEnemyHeroesInRange(400) != 0 || rot.PointUnderEnemyTurret())
                            {
                                continue;
                            }
                            DebugConsole.Write("TO SIDE", ConsoleColor.Green);
                            pos = rot;
                        }

                        break;
                }
            }

            if (target.Distance(Global.Player) >= Global.Player.AttackRange && !target.IsFacingUnit(Global.Player))
            {
                DebugConsole.Write("TO TARGET", ConsoleColor.Green);
                pos = target.ServerPosition;
            }

            QPred = pos;
            Q.Cast(pos);
        }
    }
}