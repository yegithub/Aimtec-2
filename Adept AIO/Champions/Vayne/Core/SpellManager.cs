using System;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Vayne.Core
{
    class SpellManager
    {
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
            var endPos = pred + (pred - Global.Player.ServerPosition).Normalized() * 410;
            return new Geometry.Rectangle(target.ServerPosition.To2D(), endPos.To2D(), target.BoundingRadius);
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

        public static void CastQ(Obj_AI_Base target, int modeIndex = 0)
        {
            if (!target.IsValidTarget())
            {
                return;
            }

            switch (modeIndex)
            {
                case 0:
                    Q.Cast(Game.CursorPos);
                    break;
                case 1:
                    Q.Cast(ToSide(target.ServerPosition.To2D(), -60));
                    break;
            }
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
