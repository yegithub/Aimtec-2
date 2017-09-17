using System;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.Champions.Azir.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;

namespace Adept_AIO.Champions.Azir.Update.Miscellaneous
{
    class Manager
    {
        public static void OnUpdate()
        {
            try
            {
                if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
                {
                    return;
                }

                SpellConfig.R.Width = 133 * (3 + Global.Player.GetSpell(SpellSlot.R).Level);

                switch (Global.Orbwalker.Mode)
                {
                    case OrbwalkingMode.Combo:
                        Combo.OnUpdate();
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
