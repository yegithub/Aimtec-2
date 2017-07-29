using System;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util;
using Spell = Aimtec.SDK.Spell;

namespace Adept_AIO.Champions.LeeSin.Core
{
    internal class SpellConfig
    {
        public static Spell Q, W, E, R, R2;
        public static float LastQ, LastW, LastR, LastFlash;
        public static bool QAboutToEnd => Environment.TickCount - LastQ >= 1900 + Game.Ping / 2f && LastQ > 0;

        public static void Load()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            Q.SetSkillshot(0.25f, 60, 1800, false, SkillshotType.Line, false, HitChance.Medium);

            W = new Spell(SpellSlot.W, 700);
         
            E = new Spell(SpellSlot.E, 350);

            R = new Spell(SpellSlot.R, 375);
            R2 = new Spell(SpellSlot.R, 1200);
            R2.SetSkillshot(0.25f, 100, 1600, true, SkillshotType.Line);
        }

        public static void CastE(Obj_AI_Base target)
        {
            if (target == null)
            {
                return;
            }

            E.Cast();
            DelayAction.Queue(400, Items.CastTiamat);
        }

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            switch (args.SpellSlot)
            {
                case SpellSlot.Q:
                    LastQ = Extension.IsQ2 ? 0 : Environment.TickCount;
                    break;
                case SpellSlot.W:
                    LastW = Environment.TickCount;
                    break;
                case SpellSlot.R:
                    LastR = Environment.TickCount;
                    break;
            }

            if (SummonerSpells.Flash != null && args.SpellSlot == SummonerSpells.Flash.Slot)
            {
                LastFlash = Environment.TickCount;
            }
        }
    }
}
