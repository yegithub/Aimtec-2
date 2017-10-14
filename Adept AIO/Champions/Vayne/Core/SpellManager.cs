using System;
using Adept_AIO.SDK.Generic;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Vayne.Core
{
    internal class SpellManager
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
            var wallPos = WallExtension.NearestWall(Global.Player.ServerPosition, (int)(Q.Range / 2 + 25));
            if (!wallPos.IsZero && modeIndex != 0 && target.Distance(wallPos) < Global.Player.Distance(target))
            {
                QPred = wallPos;
                Q.Cast(wallPos);
                return;
            }

            var pos = Vector3.Zero;

            var point = WallExtension.NearestWall(target.ServerPosition, 475);
          
            if (force && E.Ready && !point.IsZero)
            {
                point = target.ServerPosition + (target.ServerPosition - point).Normalized() * 100;
                if (point.Distance(Global.Player) < Q.Range)
                {
                    pos = point;
                }
            }
            else
            {
                switch (modeIndex)
                {
                    case 0:
                        pos = Game.CursorPos;
                        break;
                    case 1:
                        var toSide = ToSide(target.ServerPosition.To2D(), 90);
                        pos = (target.ServerPosition + (target.ServerPosition - toSide.To3D()).Normalized() * Global.Player.Distance(target)).To2D().Perpendicular().To3D();
                        break;
                }
            }

            if (target.IsValidAutoRange() && target.Distance(Global.Player) >= Global.Player.AttackRange - 10)
            {
                pos = target.ServerPosition;
            }

            QPred = pos;
            Q.Cast(pos);
        }

        private static Vector2 ToSide(Vector2 targetPos, double angle)
        {
            angle *= Math.PI / 180.0;
            var temp = Vector2.Subtract(targetPos, Global.Player.Position.To2D());
            var result = new Vector2(0)
            {
                X = (float)(temp.X * Math.Cos(angle) - temp.Y * Math.Sin(angle)) / 4,
                Y = (float)(temp.X * Math.Sin(angle) + temp.Y * Math.Cos(angle)) / 4
            };

            return Vector2.Add(result, Global.Player.Position.To2D());
        }
    }
}
