using System;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Animation
    {
        public static float lastReset;
        public static bool IAmSoTired;
      
        public static void Reset()
        {
            if (Global.Orbwalker.Mode == OrbwalkingMode.None)
            {
                Global.Orbwalker.AttackingEnabled = true;
                return;
            }

            Global.Orbwalker.ResetAutoAttackTimer();
            Global.Orbwalker.AttackingEnabled = false;
            Global.Orbwalker.Move(Game.CursorPos);

            lastReset = Game.TickCount;
            IAmSoTired = true;
        }

        public static float GetDelay()
        {
            var isQ3 = Extensions.CurrentQCount == 1;
            var delay = Game.Ping / 2f;
            var level = Global.Player.Level;
            delay += (isQ3 ? 380 : 330) - 3.33f * level;

            var unit = ObjectManager.Get<Obj_AI_Base>().FirstOrDefault(x => x.Distance(Global.Player) <= Global.Player.AttackRange + x.BoundingRadius && !x.IsAlly && !x.IsMe);

            if (unit != null && (unit.UnitSkinName.Contains("Crab") || unit.IsHero || unit.IsBuilding()))
            {
                delay *= isQ3 ? 1.3f : 1.15f;
            }

            return delay;
        }

        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs objAiBasePlayAnimationEventArgs)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            switch (objAiBasePlayAnimationEventArgs.Animation)
            {
                case "Spell1a":
                    Extensions.CurrentQCount = 2;
                    break;
                case "Spell1b":
                    Extensions.CurrentQCount = 3;
                    break;
                case "Spell1c":
                    Extensions.CurrentQCount = 1;
                    break;
            }
        }
    }
}
