using System;
using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jax.Update.Miscellaneous
{
    internal class SpellManager
    {
        private static bool CanUseE;
        private static Obj_AI_Base Unit;

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }
         
            switch (args.SpellData.Name)
            {
                case "JaxCounterStrike":
                    CanUseE = false;
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (Unit == null || !CanUseE || !Unit.IsValid || SpellConfig.SecondE)
            {
                return;
            }

            if (Environment.TickCount - SpellConfig.CounterStrikeTime > 2000 || Unit.Distance(GlobalExtension.Player) < 300)
            {
                SpellConfig.E.Cast(Unit);
            }
        }

        public static void CastE(Obj_AI_Base target)
        {
            CanUseE = true;
            Unit = target;
        }
    }
}
