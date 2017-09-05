using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Methods;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class Animation
    {
        public static float LastReset;
        public static bool AmSoTired;
      
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

            LastReset = Game.TickCount;
            AmSoTired = true;
        }

        public static float GetDelay()
        {
            var delay = Game.Ping / 2f;
            var level = Global.Player.Level;
            var castDelay = 380;

            switch (Extensions.CurrentQCount)
            {
                case 1:
                    castDelay -= 20;
                    break;
                case 2:
                case 3:
                    castDelay -= 40;
                    break;
               
            }

            delay += castDelay - 3.33f * level;

            var unit = ObjectManager.Get<Obj_AI_Base>().FirstOrDefault(x => x.Distance(Global.Player) <= Global.Player.AttackRange + x.BoundingRadius && !x.IsAlly && !x.IsMe);

            if (unit == null || !unit.UnitSkinName.Contains("Crab") && !unit.IsHero && !unit.IsBuilding())
            {
                return delay;
            }

            switch (Extensions.CurrentQCount)
            {
                case 1:
                    delay *= 1.3f;
                    break;
                case 2:
                case 3:
                    delay *= 1.15f;
                    break;
            }

            return delay;
        }

        public static void OnPlayAnimation(Obj_AI_Base sender, Obj_AI_BasePlayAnimationEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            switch (args.Animation)
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
