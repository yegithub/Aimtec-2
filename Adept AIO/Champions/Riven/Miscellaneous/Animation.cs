namespace Adept_AIO.Champions.Riven.Miscellaneous
{
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Unit_Extensions;

    class Animation
    {
        public static void Reset()
        {
            var qDelay = (int)(((Extensions.CurrentQCount == 1 ? 450 : 380) - 3 * Global.Player.Level) * (Global.Player.HasBuff("RivenFengShuiEngine") ? 1.5 : 1));
       
            var moveDelay = (int)(qDelay * 0.7);
            var ping = Game.Ping / 2;

            DelayAction.Queue(moveDelay,
                () =>
                {
                    Global.Orbwalker.Move(Game.CursorPos);
                });

            DelayAction.Queue(qDelay + ping,
                () =>
                {
                    Global.Orbwalker.ResetAutoAttackTimer();
                });
        }
    }
}