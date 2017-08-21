using System.Linq;
using Adept_AIO.Champions.Tristana.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Tristana.Update.OrbwalkingEvents
{
    class LaneClear
    {
        private readonly SpellConfig SpellConfig;
        private readonly MenuConfig MenuConfig;

        public LaneClear(SpellConfig spellConfig, MenuConfig menuConfig)
        {
            SpellConfig = spellConfig;
            MenuConfig = menuConfig;
        }

        public void OnPostAttack()
        {
            if (!SpellConfig.Q.Ready || MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0)
            {
                return;
            }

            var minions = GameObjects.EnemyMinions.Count(x => x.Health > Global.Player.GetAutoAttackDamage(x) && x.IsValid);

            if (minions <= 3)
            {
                return;
            }

            SpellConfig.Q.Cast();
        }

        public void OnUpdate()
        {
            if (MenuConfig.LaneClear["Check"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) > 0)
            {
                return;
            }

            if (Global.Orbwalker.IsWindingUp)
            {
                return;
            }

            if (SpellConfig.E.Ready)
            {
                var turret = GameObjects.EnemyTurrets.FirstOrDefault(x => x.IsValid && x.Distance(Global.Player) <= SpellConfig.FullRange);

                if (MenuConfig.LaneClear["Turret"].Enabled && turret != null && turret.HealthPercent() >= 35)
                {
                    SpellConfig.E.CastOnUnit(turret);
                }
                else
                {
                    var minions = GameObjects.EnemyMinions.Count(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.E) + Global.Player.GetAutoAttackDamage(x) * 5 && x.IsValid);
                    var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.E) + Global.Player.GetAutoAttackDamage(x) * 5 && x.IsValid);
                    var cannon = GameObjects.EnemyMinions.FirstOrDefault(x => x.UnitSkinName.ToLower().Contains("cannon") && x.IsValid);

                    if (minions >= MenuConfig.LaneClear["E"].Value)
                    {
                        if (cannon != null)
                        {
                            SpellConfig.E.CastOnUnit(cannon);
                        }
                        else if (minion != null)
                        {
                            SpellConfig.E.CastOnUnit(minion);
                        }
                    }
                }
            }
        }
    }
}
