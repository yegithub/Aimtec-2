using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adept_AIO.Champions.Yasuo.Core;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using SpellConfig = Adept_AIO.Champions.Jax.Core.SpellConfig;

namespace Adept_AIO.Champions.Yasuo.Update.OrbwalkingEvents
{
    class Flee
    {
        public static void OnKeyPressed()
        {
            GlobalExtension.Orbwalker.Move(Game.CursorPos);

            if (!SpellConfig.E.Ready)
            {
                return;
            }

            var mob = GameObjects.Minions.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) <= SpellConfig.E.Range && !x.HasBuff("YasuoDashWrapper"));
            var minion = Extension.GetDashableMinion();
            var enemy = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(ObjectManager.GetLocalPlayer()) <= 1800);

            if (enemy != null)
            {
                if (minion != null && minion.Distance(enemy) <= 300 || mob != null && mob.Distance(enemy) <= 300)
                {
                    return;
                }
            }

            if (minion != null)
            {
                SpellConfig.E.CastOnUnit(minion);
            }
            else if (mob != null)
            {
                SpellConfig.E.CastOnUnit(mob);
            }
        }
    }
}
