using Adept_AIO.Champions.Jax.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jax.Update.Miscellaneous
{
    internal class SpellManager
    {
        private static bool _canUseE;
        private static Obj_AI_Base _unit;

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }
         
            switch (args.SpellData.Name)
            {
                case "JaxCounterStrike":
                    _canUseE = false;
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (_unit == null || !_canUseE || !_unit.IsValid || SpellConfig.SecondE)
            {
                return;
            }

            if (Game.TickCount - SpellConfig.E.LastCastAttemptT > 1700 || _unit.Distance(Global.Player) <= SpellConfig.E.Range + _unit.BoundingRadius)
            {
                SpellConfig.E.Cast(_unit);
            }
        }

        public static void CastE(Obj_AI_Base target)
        {
            _canUseE = true;
            _unit = target;
        }
    }
}
