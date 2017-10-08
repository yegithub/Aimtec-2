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

            var extended = target.ServerPosition + (target.ServerPosition - Game.CursorPos).Normalized() * 410;
            var point = WallExtension.GeneratePoint(target.ServerPosition, extended);
            if (point.IsZero)
            {
                return;
            }

            var pos = target.ServerPosition + (target.ServerPosition - point).Normalized() * 200;
            if (pos.Distance(Global.Player) <= 425 && SummonerSpells.IsValid(SummonerSpells.Flash))
            {
                SummonerSpells.Flash.Cast(pos);
                SpellManager.E.Cast(target);
            }
        }
    }
}
