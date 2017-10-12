﻿using System.Linq;
using Adept_AIO.SDK.Geometry_Related;
using Adept_AIO.SDK.Unit_Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.Zed.Core
{
    internal class SpellManager
    {
        public static Spell Q, W, E, R;
        public static float LastR, LastW;
        public static int WCastRange = 650;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 900);
            Q.SetSkillshot(0.25f, 50, 1600, false, SkillshotType.Line);
       
            W = new Spell(SpellSlot.W, 1300);
            W.SetSkillshot(0.75f, 75, 1750, false, SkillshotType.Line);

            E = new Spell(SpellSlot.E, 290);

            R = new Spell(SpellSlot.R, 625);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.SpellSlot)
            {
                case SpellSlot.R:
                    LastR = Game.TickCount;
                    break;
                case SpellSlot.W:
                    LastW = Game.TickCount;
                    break;
            }
        }

        public static void CastW(Obj_AI_Base target, bool switchToShadow = false)
        {
            if (switchToShadow && ShadowManager.CanSwitchToShadow())
            {
                W.Cast();
            }
            else if(ShadowManager.CanCastW1() && Game.TickCount - LastW > Game.Ping + 100)
            {
                LastW = Game.TickCount;
                W.Cast(target.ServerPosition);
            }
        }

        public static void CastQ(Obj_AI_Base target, int minHit = 1)
        {
            if (Global.Player.Mana < Global.Player.GetSpell(SpellSlot.Q).Cost)
            {
                return;
            }

            if (target.IsValidTarget(Q.Range, false, false, Global.Player.ServerPosition))
            {
                Q.Cast(target);
            }
            else
            {
                foreach (var shadow in ShadowManager.Shadows)
                {
                    var pred = Q.GetPrediction(target, shadow.ServerPosition, shadow.ServerPosition);
                    if (minHit == 1)
                    {
                        Q.Cast(pred.CastPosition);
                    }
                    else
                    {
                        var rect = new Geometry.Rectangle(shadow.ServerPosition.To2D(), pred.CastPosition.To2D(), Q.Width);
                        if (GameObjects.EnemyMinions.Count(x => rect.IsInside(x.ServerPosition.To2D())) >= minHit)
                        {
                            Q.Cast(pred.CastPosition);
                        }
                    }
                  
                }
            }
        }

        public static void CastE(Obj_AI_Base target, int minHit = 1, bool minion = false)
        {
            if (Global.Player.Mana < Global.Player.GetSpell(SpellSlot.E).Cost)
            {
                return;
            }

            if (target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }
            else
            {
                foreach (var shadow in ShadowManager.Shadows)
                {
                    if (minHit == 1)
                    {
                        if (target.Distance(shadow) > E.Range)
                        {
                            return;
                        }

                        E.Cast(target);
                    }
                    
                    if(minion)
                    {
                        if (GameObjects.EnemyMinions.Count(x => x.IsValidTarget(E.Range, false, false, shadow.ServerPosition)) >= minHit)
                        {
                            E.Cast(target);
                        }
                    }
                  
                }
            }
        }

        public static void CastR(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(R.Range) || Global.Player.HasBuff("zedr2"))
            {
                return;
            }

            R.CastOnUnit(target);
        }
    }
}
