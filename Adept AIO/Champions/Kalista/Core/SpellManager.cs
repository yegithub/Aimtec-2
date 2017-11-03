namespace Adept_AIO.Champions.Kalista.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;
    using Spell = Aimtec.SDK.Spell;

    class SpellManager
    {
        public static Spell Q, W, E, R;

        private static readonly List<Vector3> Locations = new List<Vector3>
        {
            new Vector3(9827.56f, -71.2406f, 4426.136f),
            new Vector3(4951.126f, -71.2406f, 10394.05f),
            new Vector3(10998.14f, 51.72351f, 6954.169f),
            //new Vector3(7082.083f, 56.2041f, 10838.25f),
            new Vector3(3804.958f, 52.11121f, 7875.456f),
            new Vector3(7811.249f, 53.81299f, 4034.486f)
        };

        public SpellManager()
        {
            Q = new Spell(SpellSlot.Q, 1150f);
            Q.SetSkillshot(0.25f, 60f, 1750f, true, SkillshotType.Line);

            W = new Spell(SpellSlot.W, 3500f);
            E = new Spell(SpellSlot.E, 1000f);
            R = new Spell(SpellSlot.R, 1200f);
        }

        public static Geometry.Rectangle GetRectangle(Obj_AI_Base target)
        {
            var pred = Q.GetPrediction(target).CastPosition.To2D();

            return new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), Global.Player.ServerPosition.Extend(pred, Q.Range).To2D(), Q.Width);
        }

        public static void Kite(Obj_AI_Base target)
        {
            if (target.IsValidTarget(Global.Player.AttackRange + 70))
            {
                Global.Orbwalker.Move(DashManager.DashKite(target, 200, 450));
            }
        }

        public static void CastQ(Obj_AI_Base target, int minHit = -1)
        {
            var rect = GetRectangle(target);

            if (minHit == -1)
            {
                var actualCount = GameObjects.EnemyMinions.Count(x => rect.IsInside(x.ServerPosition.To2D()) && x.Health > Global.Player.GetSpellDamage(x, SpellSlot.Q));

                var killableCount = GameObjects.EnemyMinions.Count(x => x.HasBuff("kalistaexpungemarker") && rect.IsInside(x.ServerPosition.To2D()) &&
                                                                        x.Health <= Global.Player.GetSpellDamage(x, SpellSlot.Q));

                if (actualCount > killableCount)
                {
                    return;
                }

                Q.Cast(Q.GetPrediction(target).CastPosition);
            }
            else if (GameObjects.EnemyMinions.Count(x => rect.IsInside(x.ServerPosition.To2D()) && x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q)) >= minHit)
            {
                Q.Cast(target);
            }
        }

        public static void CastW()
        {
            var loc = Locations.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.Distance(Global.Player) <= W.Range);
            if (!loc.IsZero)
            {
                W.Cast(loc);
            }
        }
    }
}