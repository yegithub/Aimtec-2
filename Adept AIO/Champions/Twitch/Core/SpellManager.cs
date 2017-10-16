namespace Adept_AIO.Champions.Twitch.Core
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
        public static Spell Q, W, E, R;

        public SpellManager()
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W, 950f);
            W.SetSkillshot(0.25f, 120f, 1400f, false, SkillshotType.Circle);

            E = new Spell(SpellSlot.E, 1200f);
            R = new Spell(SpellSlot.R, 850);
            R.SetSkillshot(0.25f, 70, 1750, false, SkillshotType.Line);
        }

        public static bool HasUltBuff() => Global.Player.HasBuff("TwitchUlt") || Global.Player.HasBuff("TwitchFullAutomatic");

        public static Geometry.Rectangle GetRectangle(Obj_AI_Base target)
        {
            var pred = R.GetPrediction(target).CastPosition.To2D();
            return new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), Global.Player.ServerPosition.Extend(pred, R.Range).To2D(), R.Width);
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (Dmg.EDmg(target) > target.Health)
            {
                E.Cast();
            }
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (R.Ready)
            {
                R.Cast();
            }
            else if (Global.Orbwalker.CanAttack())
            {
                var enemy = GameObjects.Enemy.FirstOrDefault(x => x.IsTargetable && x.IsValidAutoRange());
                var rect = GetRectangle(target);

                if (enemy == null)
                {
                    return;
                }

                if (rect.IsInside(enemy.ServerPosition.To2D()))
                {
                    Global.Orbwalker.ForceTarget(enemy);
                }
            }
        }
    }
}