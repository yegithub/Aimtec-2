namespace Adept_AIO.Champions.Yasuo.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Delegates;
    using SDK.Spell_DB;
    using SDK.Unit_Extensions;

    class AntiGapcloser
    {
        public static void OnGapcloser(Obj_AI_Hero sender, GapcloserArgs args)
        {
            if (sender.IsMe || !sender.IsEnemy || args.EndPosition.Distance(Global.Player) > SpellConfig.E.Range)
            {
                return;
            }
            var missile = SpellDatabase.GetByName(args.SpellName);
            if (missile == null)
            {
                return;
            }

            if (missile.CollisionObjects.Any() && missile.IsDangerous && SpellConfig.W.Ready)
            {
                SpellConfig.W.Cast(args.StartPosition);
            }

            var minion = GameObjects.Minions.Where(x => x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper")).
                OrderBy(x => x.Distance(Game.CursorPos)).
                FirstOrDefault();

            if (SpellConfig.E.Ready && minion != null)
            {
                SpellConfig.E.CastOnUnit(minion);
            }
        }
    }
}