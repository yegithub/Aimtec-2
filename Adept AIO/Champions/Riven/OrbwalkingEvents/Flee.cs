namespace Adept_AIO.Champions.Riven.OrbwalkingEvents
{
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using Miscellaneous;
    using SDK.Geometry_Related;
    using SDK.Unit_Extensions;

    class Flee
    {
        public static void OnKeyPressed()
        {
            if (MenuConfig.Miscellaneous["Walljump"].Enabled && Global.Player.CountEnemyHeroesInRange(2000) == 0)
            {
                if (MenuConfig.Miscellaneous["Force"].Enabled && Extensions.CurrentQCount != 3)
                {
                    switch (Extensions.CurrentQCount)
                    {
                        case 2:
                            DelayAction.Queue(250,
                                () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, Game.CursorPos),
                                new CancellationToken(false));
                            break;
                        default:
                            Global.Player.SpellBook.CastSpell(SpellSlot.Q, Game.CursorPos);
                            break;
                    }
                }

                if (Extensions.CurrentQCount != 3)
                {
                    return;
                }

                const int dashRange = 275;
                var end = Global.Player.Position.Extend(Game.CursorPos, dashRange);
                var wall = WallExtension.GeneratePoint(Global.Player.Position, end);

                if (wall.IsZero ||
                    wall.Distance(Global.Player) > SpellConfig.E.Range + 65 ||
                    !(Global.Player.Orientation.To2D().
                          Perpendicular().
                          AngleBetween(WallExtension.EndPoint.To2D() - Global.Player.ServerPosition.To2D()) <
                      160))
                {
                    return;
                }

                wall = wall.Extend(Global.Player.ServerPosition, 65);

                Extensions.FleePos = wall;

                var distance = wall.Distance(Global.Player.Position);
                Global.Orbwalker.Move(wall);

                Global.Player.SpellBook.CastSpell(SpellSlot.E, Global.Player.ServerPosition.Extend(wall, 400));
                DelayAction.Queue(190,
                    () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, wall),
                    new CancellationToken(false));

                if (distance > 40)
                {
                    return;
                }

                Global.Player.SpellBook.CastSpell(SpellSlot.Q, wall);
            }
            else
            {
                if (SpellConfig.W.Ready)
                {
                    foreach (var enemy in GameObjects.EnemyHeroes.Where(x => x.IsValidTarget(SpellConfig.W.Range)))
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