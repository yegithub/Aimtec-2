using System.Linq;
using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.OrbwalkingEvents
{
    internal class LaneClear
    {
        private readonly SpellConfig _spellConfig;
        private readonly MenuConfig _menuConfig;

        public LaneClear(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            _spellConfig = spellConfig;
            _menuConfig = menuConfig;
        }

        public void OnPostAttack()
        {
            if (!_spellConfig.Q.Ready || !_menuConfig.LaneClear["Q"].Enabled || _menuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2500) > 0)
            {
                return;
            }

            var minions = GameObjects.EnemyMinions.Count(x => x.Health > Global.Player.GetAutoAttackDamage(x) && x.IsValid);

            if (minions <= 3)
            {
                return;
            }

            _spellConfig.Q.Cast();
        }

        public void OnUpdate()
        {
            if (_menuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2500) > 0)
            {
                return;
            }

            if (_spellConfig.E.Ready)
            {
                var turret = GameObjects.EnemyTurrets.FirstOrDefault(x => x.IsValid && x.Distance(Global.Player) <= _spellConfig.FullRange);

                if (_menuConfig.LaneClear["Turret"].Enabled && turret != null && turret.HealthPercent() >= 35)
                {
                    _spellConfig.E.CastOnUnit(turret);
                }
                else
                {
                    var minions = GameObjects.EnemyMinions.Count(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.E) + Global.Player.GetAutoAttackDamage(x) * 5 && x.IsValid);
                    var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.E) + Global.Player.GetAutoAttackDamage(x) * 5 && x.IsValid);
                    var cannon = GameObjects.EnemyMinions.FirstOrDefault(x => x.UnitSkinName.ToLower().Contains("cannon") && x.IsValid);

                    if (minions >= _menuConfig.LaneClear["E"].Value)
                    {
                        if (cannon != null)
                        {
                            _spellConfig.E.CastOnUnit(cannon);
                        }
                        else if (minion != null)
                        {
                            _spellConfig.E.CastOnUnit(minion);
                        }
                    }
                }
            }
        }
    }
}
