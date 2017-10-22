namespace Adept_AIO.Champions.Yasuo.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Prediction.Collision;
    using Core;
    using SDK.Spell_DB;
    using SDK.Unit_Extensions;

    class Evade
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

            var minion = GameObjects.Minions.Where(x => x.Distance(Global.Player) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper")).
                OrderBy(x => x.Distance(Game.CursorPos)).
                FirstOrDefault();

            if (args.End.Distance(Global.Player.ServerPosition) <= 140 && SpellConfig.E.Ready && minion != null)
            {
                SpellConfig.E.CastOnUnit(minion);
            }

            if (args.End.Distance(Global.Player.ServerPosition) <= 300 &&
                SpellConfig.W.Ready &&
                missile.CollisionObjects.Any(x => x.HasFlag(CollisionableObjects.YasuoWall)))
            {
                SpellConfig.W.Cast(sender.ServerPosition);
            }
        }
    }
}