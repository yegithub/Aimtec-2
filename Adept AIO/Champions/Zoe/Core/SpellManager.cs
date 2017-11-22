namespace Adept_AIO.Champions.Zoe.Core
{
    using System;
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using Aimtec.SDK.Util;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using SDK.Usables;
    using Geometry = SDK.Geometry_Related.Geometry;
    using Spell = Aimtec.SDK.Spell;

    class SpellManager
    {
        public static Spell Q, W, E, R;
        public static Vector3 PaddleStar;
        private static float lastCastTime;

        public SpellManager()
        {
            Q = new Spell(SpellSlot.Q, 800);
            Q.SetSkillshot(0.25f, 60, 1200, true, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 600);

            E = new Spell(SpellSlot.E, 800);
            E.SetSkillshot(0.25f, 40, 1700, true, SkillshotType.Line);

            R = new Spell(SpellSlot.R, 575);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null ||
                sender.IsEnemy ||
                args.SpellData.Name != "ZoeQ")
            {
                return;
            }

            DebugConsole.WriteLine($"NAME: {args.SpellData.Name}", MessageState.Debug);

            PaddleStar = args.End;
            lastCastTime = Environment.TickCount;
            DelayAction.Queue(1500, () => PaddleStar = Vector3.Zero, new CancellationToken(false));
        }

        public static Vector3 GeneratePaddleStarPrediction(Obj_AI_Base target, Spell spell)
        {
            if (PaddleStar != Vector3.Zero)
            {
                return Vector3.Zero;
            }

            var dir = target.Orientation.To2D(); // <- todo: This isn't what we're looking for. (Implement Quaterion?)
            var pos = Global.Player.ServerPosition + (Global.Player.ServerPosition - target.ServerPosition).Normalized();

            for (var i = 0; i < 360; i += 10)
            {
                var angleRad = Maths.DegreeToRadian(i);
                var rotated = (pos.To2D() + spell.Range * dir.Rotated((float) angleRad)).To3D();

                var rectBefore = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), rotated.To2D(), Q.Width + target.BoundingRadius);
                var rectAfter  = new Geometry.Rectangle(rotated.To2D(), target.ServerPosition.To2D(), Q.Width + target.BoundingRadius);

                if (GameObjects.Enemy.OrderBy(x => x.Distance(Global.Player)).Where(x => x.NetworkId != target.NetworkId).Any(x => x.MaxHealth > 20 && (rectAfter.IsInside(x.ServerPosition.To2D()) || rectBefore.IsInside(x.ServerPosition.To2D()))))
                {
                    continue;
                }
                return rotated;
            }

            return Vector3.Zero;
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(W.Range))
            {
                return;
            }

            W.CastOnUnit(target);
        }

        public static void CastQ(Obj_AI_Base target)
        {
            if (Environment.TickCount - lastCastTime < 1000 - Game.Ping / 2)
            {
                return;
            }

            if (PaddleStar != Vector3.Zero)
            {
                var pred = Q.GetPrediction(target, PaddleStar, Global.Player.ServerPosition);
                if (pred.CastPosition.IsZero)
                {
                    return;
                }
                Q.Cast(pred.CastPosition);
            }
            else if(target.IsValidTarget(Q.Range + 80))
            {
                var paddleStar = GeneratePaddleStarPrediction(target, Q);
                if (paddleStar.IsZero)
                {
                    return;
                }

                Q.Cast(paddleStar);
            }
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }
        }

        public static void CastR(Obj_AI_Base target)
        {
            var paddleStar = GeneratePaddleStarPrediction(target, R);
            if (paddleStar.IsZero)
            {
                return;
            }

            R.Cast(paddleStar);
            DelayAction.Queue(900, delegate
            {
                Q.Cast(paddleStar);
            }, new CancellationToken(false));
        }

        public static Geometry.Rectangle QRectBefore(Obj_AI_Base target)
        {
            return null;
        }

        public static Geometry.Rectangle QRectAfter(Obj_AI_Base target)
        {
            return null;
        }
    }
}