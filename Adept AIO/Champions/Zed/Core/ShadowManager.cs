namespace Adept_AIO.Champions.Zed.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using SDK.Unit_Extensions;

    class ShadowManager
    {
        public static List<Obj_AI_Minion> Shadows;

        public static bool CanSwitchToShadow()
        {
            return Global.Player.GetSpell(SpellSlot.W).ToggleState != 0 && Shadows.Any(x => x.Distance(Global.Player) <= 1300);
        }

        public static bool CanCastW1()
        {
            return Global.Player.GetSpell(SpellSlot.W).ToggleState == 0;
        }

        public static bool IsShadow(Obj_AI_Minion shadow)
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
            {
                Shadows.RemoveAll(x => x.NetworkId == sender.NetworkId);
            }
        }

        public static void OnCreate(GameObject sender)
        {
            var shadow = sender as Obj_AI_Minion;
            if (shadow == null || !IsShadow(shadow) || Environment.TickCount - SpellManager.LastR > 200 && Environment.TickCount - SpellManager.LastR <= 1000)
            {
                return;
            }

            Shadows.Add(shadow);
            DelayAction.Queue(5000, () => Shadows.RemoveAll(x => x.NetworkId == shadow.NetworkId), new CancellationToken(false));
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