namespace Adept_AIO.Champions.Jhin.Miscellaneous
{
    using System;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Damage;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Killsteal
    {
        public Killsteal()
        {
            Game.OnUpdate += OnUpdate;
        }

        private static void OnUpdate()
        {
            try
            {
                if (Global.Player.IsDead || Global.Orbwalker.IsWindingUp)
                {
                    return;
                }

                if (MenuConfig.Killsteal["E"].Enabled && SpellManager.E.Ready)
                {
                    var t = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.E) && x.IsValidTarget(SpellManager.E.Range));
                    if (t != null)
                    {
                        SpellManager.CastE(t);
                    }
                }

                if (MenuConfig.Killsteal["W"].Enabled && SpellManager.W.Ready)
                {
                    var t = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.W) && x.IsValidTarget(SpellManager.W.Range));
                    if (t != null)
                    {
                        SpellManager.CastW(t);
                    }
                }

                if (MenuConfig.Killsteal["R"].Enabled && SpellManager.R.Ready)
                {
                    var t = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.R) && x.IsValidTarget(SpellManager.R.Range));
                    if (t != null)
                    {
                        SpellManager.CastR(t);
                    }
                }

                if (MenuConfig.Killsteal["Q"].Enabled && SpellManager.Q.Ready)
                {
                    var t = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Health < Global.Player.GetSpellDamage(x, SpellSlot.Q) && x.IsValidTarget(SpellManager.Q.Range));
                    if (t == null)
                    {
                        return;
                    }
                    SpellManager.CastQ(t);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}