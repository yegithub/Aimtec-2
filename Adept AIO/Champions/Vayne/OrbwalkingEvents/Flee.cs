namespace Adept_AIO.Champions.Vayne.OrbwalkingEvents
{
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class Flee
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

                SpellManager.E.CastOnUnit(t);
            }
        }
    }
}