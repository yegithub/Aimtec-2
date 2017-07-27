using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.TargetSelector;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    class Insec
    {
        private static float LastWTime;

        private static Obj_AI_Hero target => TargetSelector.GetSelectedTarget();

        private static Vector3 InsecPosition => target.ServerPosition + (target.ServerPosition - GetTargetEndPosition()).Normalized() * DistanceBehindTarget();

        private static float DistanceBehindTarget()
        {
            return Math.Min((ObjectManager.GetLocalPlayer().BoundingRadius + target.BoundingRadius + 50) * 1.35f, SpellConfig.R.Range);
        }

        public static void Kick()
        {
            if (target == null)
            {
                return;
            }

            if (SpellConfig.W.Ready &&
                Extension.IsFirst(SpellConfig.W) &&
                WardManager.IsWardReady && target.Distance(ObjectManager.GetLocalPlayer()) > SpellConfig.R.Range + 200 &&
                target.Distance(ObjectManager.GetLocalPlayer()) < 1050)
            {
                WardManager.WardJump(InsecPosition, false);
            }

            if (!target.IsValidTarget(SpellConfig.R.Range) || !SpellConfig.R.Ready)
            {
                return;
            }
            SpellConfig.R.CastOnUnit(target);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            if (args.SpellSlot == SpellSlot.W && args.SpellData.Name.ToLower().Contains("one"))
            {
                LastWTime = Environment.TickCount;
            }

            if (SummonerSpells.Flash == null || target == null || !MenuConfig.InsecMenu[target.ChampionName].Enabled)
            {
                return;
            }

            if (!Extension.InsecMode.Active && !Extension.KickFlashMode.Active)
            {
                return;
            }

            if (MenuConfig.InsecMenu["Kick"].Value == 0 && args.SpellSlot == SummonerSpells.Flash.Slot && SpellConfig.R.Ready)
            {
                SpellConfig.R.CastOnUnit(target);
            }
            else if (MenuConfig.InsecMenu["Kick"].Value == 1 && args.SpellSlot == SpellSlot.R &&
                SummonerSpells.Flash.Ready)
            {
                if (ObjectManager.GetLocalPlayer().Distance(InsecPosition) <= 300 &&
                    Environment.TickCount - LastWTime <= 800 && WardManager.LastWardCreated > 0)
                {
                 //   Console.WriteLine("No Need To Flash");
                    return; 
                }
               // Console.WriteLine("FLASHING");
                SummonerSpells.Flash.Cast(InsecPosition);
            }
        }

        public static void OnKeyPressed()
        {
            if (target == null || !MenuConfig.InsecMenu[target.ChampionName].Enabled)
            {
                return;
            }

            if (SpellConfig.Q.Ready)
            {
                if (Extension.IsQ2)
                {
                    SpellConfig.Q.Cast();
                }
                else if(target.IsValidTarget(SpellConfig.Q.Range))
                {
                    SpellConfig.Q.Cast(target);
                }
                else if(MenuConfig.InsecMenu["Object"].Enabled)
                {
                    var minions = GameObjects.EnemyMinions.Where(x => x.Distance(InsecPosition) < 1600 && x.Health > ObjectManager.GetLocalPlayer().GetSpellDamage(x, SpellSlot.Q) * 2f)
                        .OrderBy(x => x.Distance(InsecPosition))
                        .FirstOrDefault(x => x.Distance(InsecPosition) < ObjectManager.GetLocalPlayer().Distance(InsecPosition));

                    if (minions == null)
                    {
                        return;
                    }

                    SpellConfig.Q.Cast(minions);
                }
            }

            if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W))
            {
                if (InsecPosition.Distance(ObjectManager.GetLocalPlayer()) < 600)
                {
                    WardManager.WardJump(InsecPosition, false);
                }
                else if (SummonerSpells.Flash != null && SummonerSpells.Flash.Ready)
                {
                    if (ObjectManager.GetLocalPlayer().IsDashing() || Extension.HasQ2(target) || SpellConfig.Q.Ready && target.IsValidTarget(SpellConfig.Q.Range))
                    {
                        return;
                    }

                    if (MenuConfig.InsecMenu["Kick"].Value == 0 && InsecPosition.Distance(ObjectManager.GetLocalPlayer()) < 950 - DistanceBehindTarget() ||
                        MenuConfig.InsecMenu["Kick"].Value == 1 && target.Distance(ObjectManager.GetLocalPlayer()) < 500 + SpellConfig.R.Range)
                    {
                      //  WardManager.WardJump(InsecPosition, true);
                    }
                }
            }
            else if (SpellConfig.R.Ready && target.IsValidTarget(SpellConfig.R.Range))
            {
                if (Environment.TickCount - LastWTime < 300 && ObjectManager.GetLocalPlayer().Distance(InsecPosition) >= 250)
                {
                    return;
                }

                if (InsecPosition.Distance(ObjectManager.GetLocalPlayer()) > 250 && (SummonerSpells.Flash == null ||
                    !SummonerSpells.Flash.Ready))
                {
                    return; 
                }

                SpellConfig.R.CastOnUnit(target);
            }
        }

        private static Vector3 GetTargetEndPosition()
        {
            switch (MenuConfig.InsecMenu["Position"].Value)
            {
                case 0:
                    var ally = GameObjects.AllyHeroes.FirstOrDefault();
                    var turret = GameObjects.AllyTurrets.Where(x => x.IsValid).OrderBy(x => x.Distance(ObjectManager.GetLocalPlayer())).FirstOrDefault();
                    if (turret != null)
                    {
                        return turret.ServerPosition;
                    }
                    else if (ally != null)
                    {
                        return ally.Position;
                    }
                    break;
                case 1:
                    var ally2 = GameObjects.AllyHeroes.FirstOrDefault();
                    if (ally2 != null)
                    {
                        return ally2.Position;
                    }
                 break;
            }
            return Vector3.Zero;
        }
    }
}
