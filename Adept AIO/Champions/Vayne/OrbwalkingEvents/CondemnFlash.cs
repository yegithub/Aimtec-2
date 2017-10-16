namespace Adept_AIO.Champions.Vayne.OrbwalkingEvents
{
    using System.Threading;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class CondemnFlash
    {
        public static void OnKeyPressed()
        {
            var target = Global.TargetSelector.GetSelectedTarget();
            if (target == null || !SpellManager.E.Ready)
            {
                return;
            }

            if (target.IsValidTarget(SpellManager.E.Range))
            {
                SpellManager.CastE(target);
            }

            var point = WallExtension.NearestWall(target);
            if (point.IsZero)
            {
                return;
            }

            var pos = target.ServerPosition + (target.ServerPosition - point).Normalized() * 200;
            SpellManager.DrawingPred = pos;

            if (pos.Distance(Global.Player) <= SummonerSpells.Flash.Range &&
                SummonerSpells.IsValid(SummonerSpells.Flash))
            {
                SpellManager.E.CastOnUnit(target);
                DelayAction.Queue(100, () => SummonerSpells.Flash.Cast(pos), new CancellationToken(false));
            }
        }
    }
}