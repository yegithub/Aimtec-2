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
            if (target == null || !AzirHelper.InsecMode.Active && !(MenuConfig.InsecMenu["Auto"].Enabled &&
                                                                    MenuConfig.InsecMenu["Auto"].Value <= target.CountEnemyHeroesInRange(500)))
            {
                return;
            }

            var dist = Global.Player.Distance(target);
            var allyT = GameObjects.AllyTurrets.OrderBy(x => x.Distance(Global.Player)).FirstOrDefault(x => x.IsValid && !x.IsDead);

            var pos = target.ServerPosition;
            var soldierPos = SoldierHelper.GetSoldierNearestTo(target.ServerPosition);
            
            if (!SpellConfig.E.Ready && SpellConfig.R.Ready
                && soldierPos != Vector3.Zero
                && soldierPos.Distance(target.ServerPosition) > 450
                && Game.TickCount - AzirHelper.LastQ <= 900 
                && Game.TickCount - AzirHelper.LastQ >= 400
                && SummonerSpells.IsValid(SummonerSpells.Flash) && MenuConfig.InsecMenu["Flash"].Enabled)
            {
              //  SummonerSpells.Flash.Cast(pos);
            }

            var tempPos = Global.Player.ServerPosition + (Global.Player.ServerPosition - allyT.ServerPosition).Normalized();
            var rect = new Geometry.Rectangle(Global.Player.ServerPosition.Extend(tempPos, (float)SpellConfig.RSqrt / 2f).To2D(), Global.Player.ServerPosition.Extend(target.ServerPosition, (float)SpellConfig.RSqrt / 2f).To2D(), SpellConfig.R.Width);

            if (SpellConfig.Q.Ready && soldierPos.Distance(Global.Player) <= MenuConfig.InsecMenu["Range"].Value)
            {
                if(target.Distance(Global.Player) <= 550)
                {
                    SpellConfig.Q.Cast(allyT.ServerPosition);
                }
                else
                {
                    SpellConfig.Q.Cast(pos);
                }
                
            }

            if (dist > InsecRange())
            {
                return;
            }

            if (soldierPos != Vector3.Zero && soldierPos.Distance(target) <= Global.Player.Distance(target))
            {
                SpellConfig.E.Cast(soldierPos);
            }
            else
            {
                SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(pos, SpellConfig.W.Range));
            }

            if (rect.IsInside(target.ServerPosition.To2D()))
            {
                if (SpellConfig.E.Ready && soldierPos.Distance(Global.Player) > 350)
                {
                    SpellConfig.E.Cast(allyT.ServerPosition);
                }

                if (SpellConfig.R.Ready)
                {
                    SpellConfig.R.Cast(allyT.ServerPosition);
                }
            }
        }

        public static float InsecRange()
        {
            var range = 250f;
            if (SpellConfig.E.Ready)
            {
                range += SpellConfig.E.Range - 65;
            }

            if (MenuConfig.InsecMenu["Flash"].Enabled && SummonerSpells.IsValid(SummonerSpells.Flash))
            {
                //range += SummonerSpells.Flash.Range;
            }

            return range;
        }
    }
}
