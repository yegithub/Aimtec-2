namespace Adept_AIO.Champions.Ezreal.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy || !SpellConfig.E.Ready || args.EndPosition.Distance(Global.Player) > SpellConfig.E.Range)
            {
                return;
            }

            var allyTurret = GameObjects.AllyTurrets.FirstOrDefault(x => x.IsValid);

            SpellConfig.E.Cast(allyTurret != null ? allyTurret.ServerPosition : Game.CursorPos);
        }
    }
}