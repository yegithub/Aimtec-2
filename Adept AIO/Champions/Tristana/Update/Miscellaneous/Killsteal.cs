using System.Linq;
using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.Update.Miscellaneous
{
    internal class Killsteal
    {
        private readonly SpellConfig _spellConfig;
        private readonly MenuConfig _menuConfig;

        public Killsteal(MenuConfig menuConfig, SpellConfig spellConfig)
        {
            _menuConfig = menuConfig;
            _spellConfig = spellConfig;
        }

        public void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) < _spellConfig.FullRange);
            if (target == null || !target.IsValid)
            {
                return;
            }

            if (_spellConfig.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) && _menuConfig.Killsteal["E"].Enabled)
            {
                _spellConfig.E.CastOnUnit(target);
            }
            else if (_spellConfig.R.Ready && _menuConfig.Killsteal["R"].Enabled)
            {
                if (target.Health < Global.Player.GetAutoAttackDamage(target) + 
                    (Global.Player.GetSpellDamage(target, SpellSlot.R) + (target.HasBuff("TristanaECharge") ? Global.Player.GetSpellDamage(target, SpellSlot.E) : 0)))
                {
                    _spellConfig.R.CastOnUnit(target);
                    Global.Orbwalker.Attack(target);
                }
            }
            else if(_spellConfig.W.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.W) && _menuConfig.Killsteal["W"].Enabled)
            {
                _spellConfig.W.Cast(target);
            }
        }
    }
}
