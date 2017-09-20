using System.Linq;
using Adept_AIO.Champions.Jinx.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Jinx.Update.Miscellaneous
{
    internal class Misc
    {
        private readonly SpellConfig _spellConfig;
        private readonly MenuConfig _menuConfig;

        public Misc(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            _spellConfig = spellConfig;
            _menuConfig = menuConfig;
        }

        public void OnUpdate()
        {
            if (_spellConfig.E.Ready)
            {
                if (_menuConfig.Combo["Teleport"].Enabled)
                {
                    var enemyTeleport = ObjectManager.Get<Obj_AI_Minion>().FirstOrDefault(x => x.IsEnemy && 
                                                                                               x.Distance(Global.Player) <= _spellConfig.E.Range &&
                                                                                               x.Buffs.Any(y => y.IsActive && y.Name.ToLower().Contains("teleport")));
                    if (enemyTeleport != null)
                    {
                        _spellConfig.E.Cast(enemyTeleport.ServerPosition);
                    }
                }
            }

            var target = Global.TargetSelector.GetTarget(_menuConfig.Killsteal["Range"].Value);

            if (target == null || Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (_spellConfig.R.Ready && _menuConfig.Killsteal["Range"].Enabled && _menuConfig.Whitelist[target.ChampionName].Enabled && (target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) && target.Distance(Global.Player) > Global.Player.AttackRange || _menuConfig.Combo["Semi"].Enabled))
            {
                _spellConfig.R.Cast(target);
            }

            if (_spellConfig.E.Ready)
            {
                var count = GameObjects.EnemyHeroes.Count(x => x.Distance(target) < 500 && x.NetworkId != target.NetworkId); // Todo: Check if this is buggy?

                if (_menuConfig.Combo["Count"].Enabled && count >= 3 || 
                    _menuConfig.Combo["Immovable"].Enabled && TargetState.IsHardCc(target))
                {
                    _spellConfig.E.Cast(target);
                }
            }
        }
    }
}
