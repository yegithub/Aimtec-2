namespace Adept_AIO.Champions.Jinx.Miscellaneous
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Misc
    {
        private readonly MenuConfig _menuConfig;
        private readonly SpellConfig _spellConfig;

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
                    var enemyTeleport = ObjectManager.Get<Obj_AI_Minion>().
                        FirstOrDefault(x =>
                            x.IsEnemy && x.Distance(Global.Player) <= _spellConfig.E.Range && x.Buffs.Any(y => y.IsActive && y.Name.ToLower().Contains("teleport")));
                    if (enemyTeleport != null)
                    {
                        _spellConfig.E.Cast(enemyTeleport.ServerPosition);
                    }
                }
            }

            var target = Global.TargetSelector.GetTarget(_menuConfig.Killsteal["Range"].Value);

            if (target == null)
            {
                return;
            }

            if (_spellConfig.R.Ready &&
                _menuConfig.Killsteal["Range"].Enabled &&
                _menuConfig.Whitelist[target.ChampionName].Enabled &&
                (target.Health < Global.Player.GetSpellDamage(target, SpellSlot.R) && target.Distance(Global.Player) > Global.Player.AttackRange ||
                 _menuConfig.Combo["Semi"].Enabled))
            {
                _spellConfig.R.Cast(target);
            }

            if (_spellConfig.E.Ready)
            {
                var count = GameObjects.EnemyHeroes.Count(x => x.Distance(target) < _spellConfig.E.Range * 3);

                if (_menuConfig.Combo["Count"].Enabled && count >= 2 || _menuConfig.Combo["Immovable"].Enabled && TargetState.IsHardCc(target))
                {
                    _spellConfig.E.Cast(target);
                }
            }
        }
    }
}