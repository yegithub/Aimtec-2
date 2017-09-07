using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
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

                const int dashRange = 275;
                var end = Global.Player.Position.Extend(Game.CursorPos, dashRange * 2);
                var wall = WallExtension.GeneratePoint(Global.Player.Position, end).FirstOrDefault();

                if (wall.IsZero)
                {
                    return;
                }

                Extensions.FleePos = wall;

                var wallWidth = WallExtension.GetWallWidth(wall, end);

                if (wallWidth > dashRange / 2f)
                {
                    return;
                }

                var distance = wall.Distance(Global.Player.Position);
                Global.Orbwalker.Move(wall);

                if (distance < SpellConfig.E.Range + 100)
                {
                    Global.Player.SpellBook.CastSpell(SpellSlot.E, Global.Player.ServerPosition.Extend(wall, 400));
                    DelayAction.Queue(190, () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, wall), new CancellationToken(false));
                }

                if (distance > 65)
                {
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
