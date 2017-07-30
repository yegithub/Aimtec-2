using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;
using GameObjects = Adept_AIO.SDK.Extensions.GameObjects;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    internal class Flee
    {
        public static void OnKeyPressed()
        {
            if (MenuConfig.Miscellaneous["Walljump"].Enabled &&
                GlobalExtension.Player.CountEnemyHeroesInRange(1800) == 0)
            {
                if (Extensions.CurrentQCount != 3)
                {
                    return;
                }

                const int dashRange = 350;
                var end = GlobalExtension.Player.Position.Extend(Game.CursorPos, dashRange);
                var wall = WallExtension.GeneratePoint(GlobalExtension.Player.Position, end).OrderBy(x => x.Distance(GlobalExtension.Player.Position)).FirstOrDefault();

                if (wall == Vector3.Zero)
                {
                    return;
                }

                var distance = wall.Distance(GlobalExtension.Player.Position);

                if (distance <= 5)
                {
                    return;
                }

                if (SpellConfig.E.Ready && distance <= SpellConfig.E.Range + 200 && distance > 100)
                {
                    SpellConfig.E.Cast(wall);
                    DelayAction.Queue(190, () => GlobalExtension.Player.SpellBook.CastSpell(SpellSlot.Q, wall));
                }

                if (distance > 125)
                {
                    GlobalExtension.Player.IssueOrder(OrderType.MoveTo, wall);
                    return;
                }

                GlobalExtension.Player.SpellBook.CastSpell(SpellSlot.Q, wall);
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
