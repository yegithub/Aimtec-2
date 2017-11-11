namespace Adept_AIO.Champions.Vayne.Core
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
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

        public static Geometry.Rectangle Rect(Vector3 position)
        {
            var endPos = position + (position - Global.Player.ServerPosition).Normalized() * 475;
            return new Geometry.Rectangle(position.To2D(), endPos.To2D(), 65);
        }

        public static Geometry.Rectangle RectAfterDelay(Obj_AI_Base target)
        {
            if (!target.IsMoving)
            {
                return null;
            }
            var temp = 0.5f * target.MoveSpeed;
            var pred = target.Position + (target.Position - target.Path.FirstOrDefault()).Normalized() * temp;
            return Rect(pred);
        }

        public static bool CanStun(Obj_AI_Base target)
        {
            var rect = Rect(target.ServerPosition);

            return WallExtension.IsWall(rect.Start.To3D(), rect.End.To3D());
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(E.Range))
            {
                return;
            }
            var rect = RectAfterDelay(target);

            if (!CanStun(target) || rect != null && !WallExtension.IsWall(rect.Start.To3D(), rect.End.To3D()))
            {
                return;
            }

            E.CastOnUnit(target);
        }

        public static void CastQ(Obj_AI_Base target, int modeIndex = 0, bool force = true)
        {
            var wallPos = WallExtension.NearestWall(Global.Player, 130);
            if (!wallPos.IsZero)
            {
                Q.Cast(wallPos);
                return;
            }

            var point = WallExtension.NearestWall(target, 475);

            if (force && E.Ready && !point.IsZero)
            {
                point = target.ServerPosition + (target.ServerPosition - point).Normalized() * 100;

                if (point.Distance(Global.Player) <= Q.Range)
                {
                    Q.Cast(point);
                }
            }

            switch (modeIndex)
            {
                case 0:
                    Q.Cast(Game.CursorPos);
                    break;
                case 1:
                    Q.Cast(DashManager.DashKite(target, Q.Range));
                    break;
            }
        }
    }
}