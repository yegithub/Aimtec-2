using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;
using Geometry = Adept_AIO.SDK.Junk.Geometry;

namespace Adept_AIO.Champions.Azir.Update.OrbwalkingEvents
{
    class Insec
    {
        public static void OnKeyPressed()
        {
            var target = Global.TargetSelector.GetSelectedTarget();
            if (target == null)
            {
                return;
            }

            var dist = Global.Player.Distance(target);
            var allyT = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValid && !x.IsDead);

            var pos = target.ServerPosition;

            if (pos == Vector3.Zero || dist > InsecRange())
            {
                return;
            }

            var soldierPos = SoldierHelper.GetSoldierNearestTo(target.ServerPosition);
            if (soldierPos != Vector3.Zero)
            {
                if (soldierPos.Distance(target) <= SpellConfig.RSqrt - 65)
                {
                    SpellConfig.E.Cast(soldierPos);
                }
            }
            else
            {
                SpellConfig.W.Cast(pos);
            }

            var tempPos = Global.Player.ServerPosition + (Global.Player.ServerPosition - allyT.ServerPosition).Normalized();
            var rect = new Geometry.Rectangle(Global.Player.ServerPosition.Extend(tempPos, (float)SpellConfig.RSqrt / 2f).To2D(), Global.Player.ServerPosition.Extend(target.ServerPosition, (float)SpellConfig.RSqrt / 2f).To2D(), SpellConfig.R.Width);
            
            if (rect.IsInside(target.ServerPosition.To2D()))
            {
                if (SpellConfig.E.Ready)
                {
                    DelayAction.Queue(250, ()=> SpellConfig.E.Cast(allyT.ServerPosition), new CancellationToken(false));
                }

                if (SpellConfig.Q.Ready && soldierPos.Distance(Global.Player) <= 300)
                {
                    SpellConfig.Q.Cast(allyT.ServerPosition);
                }

                if (SpellConfig.R.Ready)
                {
                    SpellConfig.R.Cast(allyT.ServerPosition);
                }
            }
            else
            {
                if (SpellConfig.E.Ready)
                {
                    SpellConfig.E.Cast(pos);
                }

                if (SpellConfig.Q.Ready && soldierPos.Distance(Global.Player) <= 800)
                {
                    SpellConfig.Q.Cast(pos);
                }
              
                 else if (ShouldFlash
                       && !SpellConfig.E.Ready
                       && Game.TickCount - AzirHelper.LastE > 700
                       && pos.Distance(Global.Player) > 600
                       && SummonerSpells.IsValid(SummonerSpells.Flash) && MenuConfig.InsecMenu["Flash"].Enabled)
                {
                    SummonerSpells.Flash.Cast(pos);
                }
            }
        }

        private static bool ShouldFlash;

        private static float InsecRange()
        {
            var range = 0f;
            if (SpellConfig.E.Ready)
            {
                range += SpellConfig.E.Range;
            }

            if (SpellConfig.R.Ready)
            {
                range += (float)SpellConfig.RSqrt - 65;
            }

            if (MenuConfig.InsecMenu["Flash"].Enabled && SummonerSpells.IsValid(SummonerSpells.Flash))
            {
                ShouldFlash = true;
                range += SummonerSpells.Flash.Range;
            }
            else
            {
                ShouldFlash = false;
            }

            return range;
        }
    }
}
