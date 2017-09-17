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
            var soldierPos = SoldierHelper.GetSoldierNearestTo(target.ServerPosition);
            
            if (!SpellConfig.E.Ready
                && soldierPos.Distance(target.ServerPosition) > 350
                && Game.TickCount - AzirHelper.LastQ <= 890 
                && Game.TickCount - AzirHelper.LastQ >= 300
                && soldierPos != Vector3.Zero
                && SummonerSpells.IsValid(SummonerSpells.Flash) && MenuConfig.InsecMenu["Flash"].Enabled)
            {
                if (Game.TickCount - AzirHelper.LastR <= 2000)
                {
                    return;
                }
                SummonerSpells.Flash.Cast(pos);
            }

            var tempPos = Global.Player.ServerPosition + (Global.Player.ServerPosition - allyT.ServerPosition).Normalized();
            var rect = new Geometry.Rectangle(Global.Player.ServerPosition.Extend(tempPos, (float)SpellConfig.RSqrt / 2f).To2D(), Global.Player.ServerPosition.Extend(target.ServerPosition, (float)SpellConfig.RSqrt / 2f).To2D(), SpellConfig.R.Width);

            if (SpellConfig.Q.Ready && soldierPos.Distance(Global.Player) <= MenuConfig.InsecMenu["Range"].Value && !rect.IsInside(target.ServerPosition.To2D()))
            {
                SpellConfig.Q.Cast(pos);
            }

            if (pos == Vector3.Zero || dist > InsecRange() && soldierPos.Distance(target) > InsecRange())
            {
                return;
            }

            if (soldierPos != Vector3.Zero)
            {
                if (soldierPos.Distance(target) <= SpellConfig.RSqrt - 65)
                {
                    SpellConfig.E.Cast(soldierPos);
                }
            }
            else
            {
                SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(pos, SpellConfig.W.Range));
            }

        
           
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
            }
        }

        public static float InsecRange()
        {
            var range = 0f;
            if (SpellConfig.E.Ready)
            {
                range += SpellConfig.E.Range - 65;
            }

            if (MenuConfig.InsecMenu["Flash"].Enabled && SummonerSpells.IsValid(SummonerSpells.Flash))
            {
                range += SummonerSpells.Flash.Range;
            }

            return range;
        }
    }
}
