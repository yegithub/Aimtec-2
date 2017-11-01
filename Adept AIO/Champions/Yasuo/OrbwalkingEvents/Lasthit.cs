namespace Adept_AIO.Champions.Yasuo.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Lasthit
    {
        public static void OnUpdate()
        {
            if (SpellConfig.E.Ready && MenuConfig.Misc["LasthitE"].Enabled)
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => MinionHelper.IsDashable(x) && x.Health <= Global.Player.GetSpellDamage(x, SpellSlot.E));

                if (minion == null)
                {
                    return;
                }

                SpellConfig.E.CastOnUnit(minion);
            }

            if (SpellConfig.Q.Ready && (MenuConfig.Misc["LasthitQ"].Enabled && Extension.CurrentMode == Mode.Normal || MenuConfig.Misc["LasthitQ3"].Enabled && Extension.CurrentMode == Mode.Tornado))
            {
                var minion = GameObjects.EnemyMinions.FirstOrDefault(x => x.Health <= Global.Player.GetSpellDamage(x, SpellSlot.Q) && x.Distance(Global.Player) <= SpellConfig.Q.Range - 100);
                if (minion == null)
                {
                    return;
                }

                SpellConfig.Q.Cast(minion);
            }
        }
    }
}