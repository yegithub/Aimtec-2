using System.Threading;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Azir.Core
{
    class AzirHelper
    {
        public static OrbwalkerMode JumpMode, InsecMode;

        public static int LastR, LastQ, LastW, LastE;

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.SpellSlot)
            {
                case SpellSlot.Q:
                    LastQ = Game.TickCount;
                    break;
                case SpellSlot.W:
                    LastW = Game.TickCount;
                    break;
                case SpellSlot.E:
                    LastE = Game.TickCount;
                    break;
                case SpellSlot.R:
                    LastR = Game.TickCount;
                    break;
            }
        }

        public static void Jump(Vector3 pos)
        {
            if(SpellConfig.W.Ready || Game.TickCount - LastW <= 1000)
            {
                var extend = Global.Player.ServerPosition.Extend(pos, SpellConfig.W.Range);
                SpellConfig.W.Cast(extend);
                SpellConfig.E.Cast(extend);
                DelayAction.Queue(100, ()=> SpellConfig.Q.Cast(pos), new CancellationToken(false));
            }
            else if (SoldierHelper.GetSoldierNearestTo(pos).Distance(pos) <= 300)
            {
                var extend = SoldierHelper.GetSoldierNearestTo(pos).Extend(pos, SpellConfig.Q.Range);
                SpellConfig.W.Cast(extend);
                SpellConfig.E.Cast(extend);
                DelayAction.Queue(100, () => SpellConfig.Q.Cast(pos), new CancellationToken(false));
            }
        }
    }
}
