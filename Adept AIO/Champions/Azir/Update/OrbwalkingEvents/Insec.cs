using System;
using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
using Adept_AIO.SDK.Usables;
using Aimtec;
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

            if (pos == Vector3.Zero || pos.Distance(target) > InsecRange() || dist > InsecRange())
            {
                return;
            }

            var soldierPos = SoldierHelper.GetSoldierNearestTo(target.ServerPosition);
            if (soldierPos != Vector3.Zero)
            {
                var spos = soldierPos.Extend(soldierPos + (soldierPos - target.ServerPosition).Normalized(), 150);
                if (spos.Distance(target) <= SpellConfig.RSqrt)
                {
                    SpellConfig.E.Cast(spos);
                }
            }
            else
            {
                SpellConfig.W.Cast(pos);
            }

            var rect = new Geometry.Rectangle(Global.Player.ServerPosition.To2D(), Global.Player.ServerPosition.Extend(target.ServerPosition, (float)SpellConfig.RSqrt).To2D(), SpellConfig.R.Width);

            if (rect.IsInside(target.ServerPosition.To2D()) && allyT != null)
            {
                if (SpellConfig.Q.Ready && soldierPos.Distance(Global.Player) <= 350)
                {
                    SpellConfig.Q.Cast(allyT.ServerPosition);
                }

                if (SpellConfig.E.Ready)
                {
                    SpellConfig.E.Cast(allyT.ServerPosition);
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

                if (SpellConfig.Q.Ready && soldierPos.Distance(Global.Player) <= 350)
                {
                    SpellConfig.Q.Cast(pos);
                }

                if (Game.TickCount - AzirHelper.LastQ <= 1100
                 && Game.TickCount - AzirHelper.LastQ > 300
                 && Game.TickCount - AzirHelper.LastE <= 1100 
                 && pos.Distance(Global.Player) > SpellConfig.RSqrt
                 && pos.Distance(Global.Player) < SpellConfig.RSqrt / 2 + 425 && SummonerSpells.IsValid(SummonerSpells.Flash) &&
                    MenuConfig.InsecMenu["Flash"].Enabled)
                {
                    SummonerSpells.Flash.Cast(pos);
                }
            }
        }

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
                range += SummonerSpells.Flash.Range;
            }
            return range;
        }
    }
}
