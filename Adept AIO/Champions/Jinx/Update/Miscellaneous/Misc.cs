using System.Linq;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jinx.Update.Miscellaneous
{
    class Misc
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;

        public Misc(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            SpellConfig = spellConfig;
            MenuConfig = menuConfig;
        }

        public void OnUpdate()
        {
            if (SpellConfig.E.Ready)
            {
                if (MenuConfig.Combo["Teleport"].Enabled)
                {
                    var enemyTeleport = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.IsEnemy && 
                                                                                               x.Distance(Global.Player) <= SpellConfig.E.Range &&
                                                                                               x.Buffs.Any(y => y.IsActive && y.Name.ToLower().Contains("teleport")));
                    if (enemyTeleport != null)
                    {
                        SpellConfig.E.Cast(enemyTeleport.ServerPosition);
                    }
                }
            }

            var target = Global.TargetSelector.GetTarget(MenuConfig.Killsteal["Range"].Value);

            if (target == null || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellConfig.R.Ready && MenuConfig.Killsteal["Range"].Enabled && MenuConfig.Whitelist[target.ChampionName].Enabled && (target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) && target.Distance(Global.Player) > Global.Player.AttackRange || MenuConfig.Combo["Semi"].Enabled))
            {
                SpellConfig.R.Cast(target);
            }

            if (SpellConfig.E.Ready)
            {
                var count = GameObjects.EnemyHeroes.Count(x => x.Distance(target) < 500);

                if (MenuConfig.Combo["Count"].Enabled && count >= 2 || 
                    MenuConfig.Combo["Immovable"].Enabled && target.ActionState == ActionState.Immovable)
                {
                    SpellConfig.E.Cast(target);
                }
            }
        }
    }
}
