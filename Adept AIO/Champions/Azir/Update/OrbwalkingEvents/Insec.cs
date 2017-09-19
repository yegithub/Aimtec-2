using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
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

            var targetPos = target.ServerPosition;
            var soldierPos = SoldierHelper.GetSoldierNearestTo(target.ServerPosition);

            var targetExtend = Global.Player.ServerPosition.Extend(target.ServerPosition, SpellConfig.R.Range - target.BoundingRadius - 30);

            AzirHelper.Rect = new Geometry.Rectangle(targetExtend.To2D(), Global.Player.ServerPosition.To2D(), SpellConfig.R.Width /2f - target.BoundingRadius);
            
            if (SpellConfig.Q.Ready)
            {
                if (soldierPos.Distance(target) <= 350)
                {
                    if (dist <= MenuConfig.InsecMenu["Range"].Value) // Todo: Continue working on this.
                    {
                        SpellConfig.Q.Cast(allyT.ServerPosition);
                    }
                }
                else if (soldierPos.Distance(Global.Player) <= MenuConfig.InsecMenu["Range"].Value)
                {
                    SpellConfig.Q.Cast(targetPos);
                }
            }

            if (dist > InsecRange())
            {
                return;
            }

            if (soldierPos != Vector3.Zero)
            {
                SpellConfig.E.Cast(soldierPos);
            }
            else
            {
                SpellConfig.W.Cast(Global.Player.ServerPosition.Extend(targetPos, SpellConfig.W.Range));
            }

            if (AzirHelper.Rect.IsInside(target.ServerPosition.To2D()))
            {
                if (SpellConfig.E.Ready && soldierPos.Distance(Global.Player) > 600)
                {
                    SpellConfig.E.Cast(allyT.ServerPosition);
                }

                if (SpellConfig.R.Ready)
                {
                    SpellConfig.R.Cast(allyT.ServerPosition);
                }
            }

            if (SummonerSpells.IsValid(SummonerSpells.Flash) && MenuConfig.InsecMenu["Flash"].Enabled
            &&  SpellConfig.R.Ready
            && !SpellConfig.Q.Ready
            && !SpellConfig.E.Ready
            &&  dist > 450
            &&  soldierPos.Distance(target) > 450
            && !Global.Player.IsDashing())
            {
                if (Game.TickCount - AzirHelper.LastE <= 900
                    || Game.TickCount - AzirHelper.LastQ <= 900)
                {
                    return;
                }
                SummonerSpells.Flash.Cast(target.ServerPosition);
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
                range += SummonerSpells.Flash.Range;
            }

            return range;
        }
    }
}
