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
    class Insec
    {
   
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

            if (SummonerSpells.Flash == null || target == null || !MenuConfig.InsecMenu[target.ChampionName].Enabled)
            {
                return;
            }

            if (!Extension.InsecMode.Active && !Extension.KickFlashMode.Active)
            {
                return;
            }

            if (ObjectManager.GetLocalPlayer().Distance(InsecPosition) <= 380 &&
                Environment.TickCount - WardManager.LastWardCreated < 1500)
            {
                return;
            }

            if (MenuConfig.InsecMenu["Kick"].Value == 1 && args.SpellSlot == SpellSlot.R &&
                SummonerSpells.Flash.Ready)
            {
               
                Console.WriteLine("FLASHING");
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
                    var minion = GameObjects.EnemyMinions.Where(x => x.Distance(InsecPosition) < 700 && 
                                  ObjectManager.GetLocalPlayer().Distance(x) <= SpellConfig.Q.Range &&
                                  x.Health > ObjectManager.GetLocalPlayer().GetSpellDamage(x, SpellSlot.Q, DamageStage.AgainstMinions) * 1.75f)
                                  .OrderBy(x => x.Distance(InsecPosition))
                                  .FirstOrDefault(x => x.Distance(InsecPosition) < ObjectManager.GetLocalPlayer().Distance(InsecPosition));

                    if (minion == null)
                    {
                        return;
                    }

                    SpellConfig.Q.Cast(minion.ServerPosition);
                }
            }

            if (SpellConfig.W.Ready && Extension.IsFirst(SpellConfig.W) && InsecPosition.Distance(ObjectManager.GetLocalPlayer()) < 500)
            {
                WardManager.WardJump(InsecPosition, false);
            }
            else if (!SpellConfig.R.Ready)
            {
                return;
            }
            else if (MenuConfig.InsecMenu["Kick"].Value == 0 && SummonerSpells.Flash != null && SummonerSpells.Flash.Ready &&
                InsecPosition.Distance(ObjectManager.GetLocalPlayer()) <= 425 &&
              !(Environment.TickCount - WardManager.LastWardCreated <= 1500))
            {
                SummonerSpells.Flash.Cast(InsecPosition);
            }
            else if (!target.IsValidTarget(SpellConfig.R.Range) || Environment.TickCount - WardManager.LastWardCreated < 100 && ObjectManager.GetLocalPlayer().Distance(InsecPosition) >= 350)
            {
                return;
            }
            else if (InsecPosition.Distance(ObjectManager.GetLocalPlayer()) > 100 && (SummonerSpells.Flash == null || !SummonerSpells.Flash.Ready))
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
