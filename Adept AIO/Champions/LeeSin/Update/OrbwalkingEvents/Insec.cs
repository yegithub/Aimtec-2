using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.TargetSelector;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents
{
    class Insec
    {
        private static Obj_AI_Hero target => TargetSelector.GetSelectedTarget();

        private static float DistanceBehindTarget()
        {
            return Math.Min((ObjectManager.GetLocalPlayer().BoundingRadius + target.BoundingRadius + 50) * 1.25f, SpellConfig.R.Range);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe || SummonerSpells.Flash == null)
            {
                return;
            }

            if (target == null || !MenuConfig.Insec[target.ChampionName].Enabled)
            {
                return;
            }

            if (MenuConfig.Insec["Kick"].Value == 0 && args.SpellSlot == SummonerSpells.Flash.Slot && SpellConfig.R.Ready)
            {
                SpellConfig.R.CastOnUnit(target);
            }
            else if (MenuConfig.Insec["Kick"].Value == 1 && args.SpellSlot == SpellSlot.R && SummonerSpells.Flash.Ready)
            {
                var pos = target.ServerPosition + (target.ServerPosition - GetTargetEndPosition()).Normalized() * DistanceBehindTarget();
                SummonerSpells.Flash.Cast(pos);
            }
        }

        public static void OnKeyPressed()
        {
            //if (GlobalExtension.Orbwalker.CanMove())
            //{
            //    GlobalExtension.Orbwalker.Move(Game.CursorPos);
            //}

            if (target == null || !MenuConfig.Insec[target.ChampionName].Enabled)
            {
                return;
            }

            //if (GlobalExtension.Orbwalker.CanAttack() && target.Distance(ObjectManager.GetLocalPlayer()) <= ObjectManager.GetLocalPlayer().AttackRange + 65)
            //{
            //    GlobalExtension.Orbwalker.Attack(target);
            //}

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
                else if(MenuConfig.Insec["Object"].Enabled)
                {
                    var minions = GameObjects.Minions.Where(x => !x.IsAlly && x.Distance(target) < 1800 && x.Distance(target) > 500 && x.Health > ObjectManager.GetLocalPlayer().GetSpellDamage(x, SpellSlot.Q))
                        .OrderBy(x => x.Distance(target))
                        .LastOrDefault(x => x.Distance(target) < ObjectManager.GetLocalPlayer().Distance(target));

                    if (minions == null)
                    {
                        return;
                    }

                    SpellConfig.Q.Cast(minions);
                }
            }

            if (SpellConfig.W.Ready)
            {
                if (SummonerSpells.Flash != null && SummonerSpells.Flash.Ready)
                {
                    if (target.Distance(ObjectManager.GetLocalPlayer()) > Math.Pow(1000 - DistanceBehindTarget(), 2))
                    {
                        return;
                    }

                   GapcloseW();
                }
                else if(target.Distance(ObjectManager.GetLocalPlayer()) < Math.Pow(600 - DistanceBehindTarget(), 2))
                {
                    GapcloseW();
                }
            }
        }

        private static void GapcloseW()
        {
            if (WardManager.CanWardJump)
            {
                var pos = target.ServerPosition + (target.ServerPosition - GetTargetEndPosition()).Normalized() * DistanceBehindTarget();
                if (pos.Distance(ObjectManager.GetLocalPlayer().ServerPosition) <= Math.Pow(600, 2))
                {
                    if (GlobalExtension.Orbwalker.CanMove())
                    {
                        GlobalExtension.Orbwalker.Move(pos);
                    }

                    var objects = WardManager.JumpableObjects.LastOrDefault(x => x.Distance(pos) <= ObjectManager.GetLocalPlayer().Distance(pos));
                    if (objects != null)
                    {
                        SpellConfig.W.CastOnUnit(objects);
                    }
                    else if (WardManager.CanCastWard && WardManager.CanWardJump)
                    {
                        WardManager.WardJump(pos);
                    }
                }
            }
        }

        private static Vector3 GetTargetEndPosition()
        {
            switch (MenuConfig.Harass["Dodge"].Value)
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
