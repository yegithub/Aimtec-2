using System.Linq;
using Adept_AIO.Champions.Gragas.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Gragas.Miscellaneous
{
    class Automatic
    {
        public static void OnUpdate()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(1500)))
            {
                if (MenuConfig.Automatic["Q"].Enabled 
                    && SpellManager.Q.Ready 
                    && SpellManager.Barrel != null
                    && SpellManager.Barrel.Center.Distance(target) > SpellManager.Barrel.Radius - 150 
                    && SpellManager.Barrel.Center.Distance(target) <= SpellManager.Barrel.Radius)
                {
                    SpellManager.Q.Cast();
                }

                if (MenuConfig.Automatic["E"].Enabled && SpellManager.E.Ready && target.Health < Global.Player.GetSpellDamage(target, SpellSlot.E))
                {
                    SpellManager.CastE(target);
                }

                if (MenuConfig.Automatic["Disengage"].Enabled && SpellManager.R.Ready &&
                    target.CountEnemyHeroesInRange(SpellManager.RHitboxRadius) >= 5)
                {
                    SpellManager.CastR(target);
                }
            }
        }
    }
}
