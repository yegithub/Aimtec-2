using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Damage.JSON;
using Aimtec.SDK.Events;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    internal class Insec
    {
        private static bool WardFlash;
        private static float LastQTime;
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

            if (args.SpellSlot == SpellSlot.Q && args.SpellData.Name.ToLower().Contains("one"))
            {
                LastQTime = Environment.TickCount;
            }

            if(WardManager.IsWardReady || SummonerSpells.Flash == null || target == null || args.SpellSlot != SpellSlot.R || MenuConfig.InsecMenu["Kick"].Value != 1 ||
               !SummonerSpells.Flash.Ready)
            {
                return;
            }

            if (!MenuConfig.InsecMenu[target.ChampionName].Enabled || !Extension.InsecMode.Active && !Extension.KickFlashMode.Active ||
                ObjectManager.GetLocalPlayer().Distance(InsecPosition) <= 100 ||
                Environment.TickCount - WardManager.LastWardCreated < 1500 &&
                !WardFlash)
            {
                return;
            }

            Console.WriteLine("FLASHING");
            SummonerSpells.Flash.Cast(InsecPosition);
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
                    LastQTime = Environment.TickCount;
                }
                else if(target.IsValidTarget(SpellConfig.Q.Range))
                {
                    SpellConfig.Q.Cast(target);
                    LastQTime = Environment.TickCount;
                }
                else if(MenuConfig.InsecMenu["Object"].Enabled)
                {
                    var minion = GameObjects.EnemyMinions.Where(x => x.Distance(InsecPosition) < 700 && 
                                  ObjectManager.GetLocalPlayer().Distance(x) <= SpellConfig.Q.Range &&
                                  x.Health > ObjectManager.GetLocalPlayer().GetSpellDamage(x, SpellSlot.Q, DamageStage.AgainstMinions) * 1.75f)
                                  .OrderBy(x => x.Distance(InsecPosition))
                                  .FirstOrDefault(x => x.Distance(InsecPosition) < ObjectManager.GetLocalPlayer().Distance(InsecPosition));

                    if (minion == null)
                    {
                        return;
                    }

                    if (minion.Distance(InsecPosition) <= 500)
                    {
                        WardFlash = false;
                    }
                    SpellConfig.Q.Cast(minion.ServerPosition);
                    LastQTime = Environment.TickCount;
                }
            }

            if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W))
            {
                if (InsecPosition.Distance(ObjectManager.GetLocalPlayer()) < 500)
                {
                    WardFlash = false;
                    WardManager.WardJump(InsecPosition, false);
                }
                else if (SummonerSpells.Flash != null && SummonerSpells.Flash.Ready &&
                         InsecPosition.Distance(ObjectManager.GetLocalPlayer()) < 500 + 425 && Environment.TickCount - LastQTime > 650)
                {
                    WardFlash = true;
                    WardManager.WardJump(InsecPosition, true);
                }
            }
            else if (!SpellConfig.R.Ready)
            {
                return;
            }
            else if (MenuConfig.InsecMenu["Kick"].Value == 0 && SummonerSpells.Flash != null && SummonerSpells.Flash.Ready &&
                InsecPosition.Distance(ObjectManager.GetLocalPlayer()) <= 425)
            {
                if (Environment.TickCount - WardManager.LastWardCreated <= 1500 && !WardFlash)
                {
                    return;
                }
                SummonerSpells.Flash.Cast(InsecPosition);
            }
            else if (!target.IsValidTarget(SpellConfig.R.Range) || Environment.TickCount - WardManager.LastWardCreated < 250 && ObjectManager.GetLocalPlayer().Distance(InsecPosition) >= 350)
            {
                return;
            }
            else if (InsecPosition.Distance(ObjectManager.GetLocalPlayer()) > 200 && (SummonerSpells.Flash == null || !SummonerSpells.Flash.Ready))
            {
                return; 
            }

            SpellConfig.R.CastOnUnit(target);
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
