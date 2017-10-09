using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Vayne.OrbwalkingMode
{
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

            var point = WallExtension.NearestWall(target.ServerPosition, 475);
            if (point.IsZero)
            {
                return;
            }

            var pos = target.ServerPosition + (target.ServerPosition - point).Normalized() * 200;
            SpellManager.DrawingPred = pos;

            if (pos.Distance(Global.Player) <= SummonerSpells.Flash.Range && SummonerSpells.IsValid(SummonerSpells.Flash))
            {
                SummonerSpells.Flash.Cast(pos);
                SpellManager.E.Cast(target);
            }
        }
    }
}
