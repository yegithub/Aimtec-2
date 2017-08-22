using System.Linq;
using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.Update.Miscellaneous
{
    internal class Killsteal
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;

        public Killsteal(MenuConfig menuConfig, SpellConfig spellConfig)
        {
            MenuConfig = menuConfig;
            SpellConfig = spellConfig;
        }

        public void OnUpdate()
        {
            var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) < SpellConfig.FullRange);
            if (target == null || !target.IsValid)
            {
                return;
            }

            if (SpellConfig.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E) && MenuConfig.Killsteal["E"].Enabled)
            {
                SpellConfig.E.CastOnUnit(target);
            }
            else if (SpellConfig.R.Ready && MenuConfig.Killsteal["R"].Enabled)
            {
                if (target.Health < Global.Player.GetAutoAttackDamage(target) + 
                    (Global.Player.GetSpellDamage(target, SpellSlot.R) + (target.HasBuff("TristanaECharge") ? Global.Player.GetSpellDamage(target, SpellSlot.E) : 0)))
                {
                    SpellConfig.R.CastOnUnit(target);
                    Global.Orbwalker.Attack(target);
                }
            }
            else if(SpellConfig.W.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.W) && MenuConfig.Killsteal["W"].Enabled)
            {
                SpellConfig.W.Cast(target);
            }
        }
    }
}
