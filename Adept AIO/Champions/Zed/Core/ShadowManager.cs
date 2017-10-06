using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Zed.Core
{
    internal class ShadowManager
    {
        public static List<Obj_AI_Minion> Shadows;

        public static bool CanSwitchToShadow()
        {
            return !CanCastW1()
                && GetShadowNearestTo(Global.Player.ServerPosition).Distance(Global.Player) <= 1300 
                && Global.Player.GetSpell(SpellSlot.W).ToggleState != 0
                && Global.Player.Mana > Global.Player.GetSpell(SpellSlot.W).Cost;
        }

        public static bool CanCastW1()
        {
            return Global.Player.GetSpell(SpellSlot.W).ToggleState == 0;
        }

        private static bool IsShadow(Obj_AI_Minion shadow)
        {
            if (shadow == null)
            {
                return false;
            }
            return shadow.IsAlly && !shadow.IsDead && shadow.IsValid && shadow.UnitSkinName.ToLower().Contains("shadow");
        }

        public static void OnDelete(GameObject sender)
        {
            if (Shadows.Any(x => x.NetworkId == sender.NetworkId))
                Shadows.RemoveAll(x => x.NetworkId == sender.NetworkId);
        }

        public static void OnCreate(GameObject sender)
        {
            var shadow = sender as Obj_AI_Minion;
            if (shadow != null && IsShadow(shadow))
            {
                if (Game.TickCount - SpellManager.LastR > 200 && Game.TickCount - SpellManager.LastR <= 1000)
                {
                    return;
                }
                Shadows.Add(shadow);
                DelayAction.Queue(5000 - Game.Ping / 2, () => Shadows.RemoveAll(x => x.NetworkId == shadow.NetworkId), new CancellationToken(false));
            }
        }

        public static Vector3 GetShadowNearestTo(Vector3 pos)
        {
            foreach (var minion in GameObjects.AllyMinions.Where(IsShadow))
            {
                var shadow = Shadows.OrderBy(x => x.Distance(pos)).FirstOrDefault(x => x.NetworkId == minion.NetworkId);
                return shadow != null ? shadow.ServerPosition : Vector3.Zero;
            }
            return Vector3.Zero;
        }
    }
}
