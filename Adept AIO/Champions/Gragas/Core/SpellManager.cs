namespace Adept_AIO.Champions.Gragas.Core
{
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Skillshots;
    using Aimtec.SDK.Util;
    using SDK.Unit_Extensions;
    using SDK.Usables;
    using Geometry = SDK.Geometry_Related.Geometry;
    using Spell = Aimtec.SDK.Spell;

    class SpellManager
    {
        public static Spell Q, W, E, R;

        public static int KnockBackRange = 600;

        public static int RHitboxRadius = 400;
        public static int EHitboxRadius = 180;
        public static int WHitboxRadius = 250;

        public static Geometry.Rectangle BodySlam;

        public static Geometry.Circle Barrel;

        public static int QRadius = 300;

        public SpellManager()
        {
            Q = new Spell(SpellSlot.Q, 850);
            Q.SetSkillshot(0.25f, QRadius, 1000, false, SkillshotType.Circle);

            W = new Spell(SpellSlot.W);

            E = new Spell(SpellSlot.E, 600);
            E.SetSkillshot(0.05f, EHitboxRadius, 1600, true, SkillshotType.Line);

            R = new Spell(SpellSlot.R, 1000);
            R.SetSkillshot(0.25f, RHitboxRadius, 1600, false, SkillshotType.Circle);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SpellSlot == SpellSlot.Q && args.SpellData.Name == "GragasQ")
            {
                Barrel = new Geometry.Circle(args.End.To2D(), QRadius);
            }
        }

        public static void OnDestroy(GameObject sender)
        {
            if (sender.Name != "Gragas_Base_Q_Ally.troy")
            {
                return;
            }

            Barrel = null;
        }

        public static void CastQ(Obj_AI_Base target, bool insec = false)
        {
            if (Barrel != null)
            {
                return;
            }

            if (insec)
            {
                Q.Cast(InsecManager.QInsecPos(target));
            }
            else
            {
                var pred = Q.GetPrediction(target);
                if (pred.HitChance >= HitChance.High)
                {
                    Q.Cast(target);
                }
            }
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target.Distance(Global.Player) > 500)
            {
                return;
            }

            W.Cast();
        }

        public static void CastE(Obj_AI_Base target, bool flash = false)
        {
            var canFlash = flash && SummonerSpells.IsValid(SummonerSpells.Flash) && target.Distance(Global.Player) > E.Range && target.Distance(Global.Player) < E.Range + 425;

            var pred = E.GetPrediction(target);
            BodySlam = new Geometry.Rectangle(canFlash ? Global.Player.ServerPosition.Extend(target.ServerPosition, 425 + E.Range - target.BoundingRadius).To2D() : Global.Player.ServerPosition.To2D(),
                                              Global.Player.ServerPosition.Extend(pred.CastPosition, E.Range).To2D(), EHitboxRadius);

            if (BodySlam.IsOutside(target.ServerPosition.To2D()))
            {
                return;
            }

            if (GameObjects.EnemyMinions.Any(x => BodySlam.IsInside(x.ServerPosition.To2D()) &&
                                                  x.Distance(BodySlam.Start) <= target.Distance(BodySlam.Start) &&
                                                  x.Distance(target) >= Global.Player.BoundingRadius))
            {
                return;
            }

            if (canFlash)
            {
                E.Cast(target.ServerPosition);
                DelayAction.Queue(300, () => SummonerSpells.Flash.Cast(target.ServerPosition), new CancellationToken(false));
                return;
            }

            E.Cast(target.ServerPosition);
        }

        public static void CastR(Obj_AI_Base target)
        {
            var insecPos = InsecManager.InsecPosition(target);

            if (insecPos.Distance(Global.Player) > R.Range)
            {
                return;
            }

            R.Cast(insecPos);
        }
    }
}