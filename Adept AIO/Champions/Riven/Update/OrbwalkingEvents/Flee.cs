using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;
using GameObjects = Adept_AIO.SDK.Junk.GameObjects;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Flee
    {
        public static void OnKeyPressed()
        {
            if (MenuConfig.Miscellaneous["Walljump"].Enabled &&
                Global.Player.CountEnemyHeroesInRange(1800) == 0)
            {
                if (Extensions.CurrentQCount != 3)
                {
                    return;
                }

                const int dashRange = 350;
                var end = Global.Player.Position.Extend(Game.CursorPos, dashRange);
                var wall = WallExtension.GeneratePoint(Global.Player.Position, end).OrderBy(x => x.Distance(Global.Player.Position)).FirstOrDefault();

                if (wall.IsZero)
                {
                    return;
                }

                var distance = wall.Distance(Global.Player.Position);

                if (distance <= 5)
                {
                    return;
                }

                if (SpellConfig.E.Ready && distance <= SpellConfig.E.Range + 200 && distance > 100)
                {
                    SpellConfig.E.Cast(wall);
                    DelayAction.Queue(190, () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, wall), new CancellationToken(false));
                }

                if (distance > 125)
                {
                    Global.Player.IssueOrder(OrderType.MoveTo, wall);
                    return;
                }

                Global.Player.SpellBook.CastSpell(SpellSlot.Q, wall);
            }
            else
            {
                if (SpellConfig.W.Ready)
                {
                    foreach (var enemy in GameObjects.EnemyHeroes.Where(SpellManager.InsideKiBurst))
                    {
                        SpellManager.CastW(enemy);
                    }
                }

                if (SpellConfig.Q.Ready)
                {
                    SpellConfig.Q.Cast(Game.CursorPos);
                }
                else if (SpellConfig.E.Ready)
                {
                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount != 3)
                    {
                        return;
                    }
                    SpellConfig.E.Cast(Game.CursorPos);
                }
            }
        }
    }
}
