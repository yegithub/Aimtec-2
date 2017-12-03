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
                        case 3:
                            DelayAction.Queue(250, () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, Game.CursorPos), new CancellationToken(false));
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
                var wall = WallExtension.GetBestWallHopPos(Global.Player.Position, dashRange);

                if (wall.IsZero ||
                    wall.Distance(Global.Player) > SpellConfig.E.Range + 65)
                {
                    return;
                }

                wall = wall.Extend(Global.Player.ServerPosition, 65);

                var distance = wall.Distance(Global.Player.Position);
                Global.Orbwalker.Move(wall);

                if (SpellConfig.E.Ready)
                {
                    Global.Player.SpellBook.CastSpell(SpellSlot.E, Global.Player.ServerPosition.Extend(wall, 400));
                    DelayAction.Queue(190, () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, wall), new CancellationToken(false));
                }
               

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