using System.Linq;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Spell_DB;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Yasuo.Update.Miscellaneous
{
    internal class SafetyMeasure
    {
        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!MenuConfig.Combo["Dodge"].Enabled || sender == null || !sender.IsHero || !sender.IsEnemy)           
            {
                return;
            }

            var missile = SpellDatabase.GetByName(args.SpellData.Name);

            if (missile == null)
            {
               
                return;
            }

            var minion = GameObjects.Minions.Where(x => x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper")).OrderBy(x => x.Distance(Game.CursorPos)).FirstOrDefault();

            if (args.End.Distance(Global.Player.ServerPosition) <= 200 && SpellConfig.E.Ready && minion != null)
            {
                SpellConfig.E.CastOnUnit(minion);
            }
            else if (args.End.Distance(Global.Player.ServerPosition) <= 300 && SpellConfig.W.Ready)
            {
                SpellConfig.W.Cast(sender.ServerPosition);
            }
        }
    }
}
