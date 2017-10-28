namespace Adept_AIO.Champions.Gnar.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        public AntiGapcloser()
        {
            Gapcloser.OnGapcloser += OnGapcloser;
        }

        private static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (!sender.IsEnemy || !SpellManager.E.Ready || args.EndPosition.Distance(Global.Player) > SpellManager.E.Range)
            {
                return;
            }

            var enemy = GameObjects.EnemyMinions.OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault(x => x.IsValidTarget(SpellManager.E.Range));
            if (enemy != null)
            {
                SpellManager.CastE(enemy);
            }
        }
    }
}