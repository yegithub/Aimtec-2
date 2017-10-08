using System.Linq;
using Adept_AIO.Champions.Vayne.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Vayne.OrbwalkingMode
{
    internal class Flee
    {
        public static void OnKeyPressed()
        {
            if (SpellManager.Q.Ready)
            {
                SpellManager.Q.Cast(Game.CursorPos);
            }

            if (SpellManager.E.Ready)
            {
                var t = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(Global.Player) <= SpellManager.E.Range);
                if (t == null)
                {
                    return;
                }

                SpellManager.CastE(t);
            }
        }
    }
}
