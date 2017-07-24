using System.Linq;
using Adept_AIO.Champions.Jax.Core;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;

namespace Adept_AIO.Champions.Jax.Update.Miscellaneous
{
    class Killsteal
    {
        public static void OnUpdate()
        {
            if (!SpellConfig.Q.Ready || !MenuConfig.Killsteal["Q"].Enabled)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x => x.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range && x.Health < ObjectManager.GetLocalPlayer().GetSpellDamage(x, SpellSlot.Q)))
            {
                if (!target.IsValidTarget())
                {
                    continue;
                }

                SpellConfig.Q.CastOnUnit(target);
            }
        }
    }
}
