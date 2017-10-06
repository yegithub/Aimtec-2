using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Delegates;
using Adept_AIO.SDK.Spell_DB;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Miscellaneous
{
    internal class AntiGapcloser
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

            var minion = GameObjects.Minions.Where(x => x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper")).OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault();

            if (SpellConfig.E.Ready && minion != null)
            {
                SpellConfig.E.CastOnUnit(minion);
            }
        }
    }
}
