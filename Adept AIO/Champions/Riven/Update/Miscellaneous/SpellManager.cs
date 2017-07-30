using System;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class SpellManager
    {
        private static bool CanUseQ;
        private static bool CanUseW;
        private static Obj_AI_Base Unit;
      
        /// <summary>
        /// Tracks spells recently used by Riven
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.SpellData.Name)
            {
                case "RivenTriCleave":
                    CanUseQ = false;
                    Extensions.LastQTime = Environment.TickCount;
                    Extensions.CurrentQCount++;
                    if (Extensions.CurrentQCount > 3) { Extensions.CurrentQCount = 1; }

                    var delay = Game.Ping + Extensions.CurrentQCount < 3 ? 300 : 350;
                    DelayAction.Queue(Animation.GetDelay(delay), Animation.Reset);
                    break;
                case "RivenMartyr":
                    CanUseW = false;
                    Extensions.LastWTime = Environment.TickCount;
                    break;
                case "RivenFengShuiEngine":
                    Extensions.UltimateMode = UltimateMode.Second;
                    Extensions.LastR2Time = Environment.TickCount;
                    break;
                case "RivenIzunaBlade":
                    Extensions.UltimateMode = UltimateMode.First;
                    break;
            }
        }

        public static void OnUpdate()
        {
            /*
            if (Environment.TickCount - Extensions.LastR2Time >= 14000 && Extensions.LastR2Time > 0)
            {
                var target = GlobalExtension.TargetSelector.GetTarget(SpellConfig.R2.Range);
                if (target != null)
                {
                    SpellConfig.R2.Cast(target);
                }
            }
            */

            if (Unit == null)
            {
                return;
            }

            if (CanUseQ)
            {
                if (GlobalExtension.Orbwalker.CanMove()) // Not fast enough
                {
                    if (Extensions.CurrentQCount == 3)
                    {
                        Items.CastTiamat();
                    }

                    GlobalExtension.Player.SpellBook.CastSpell(SpellSlot.Q, Unit);
                }
            }

            if (CanUseW)
            {
                Items.CastTiamat();
                SpellConfig.W.Cast();
                CanUseW = false;
            }
        }

        public static void CastQ(Obj_AI_Base target)
        {
            if (target.HasBuff("FioraW") || target.HasBuff("PoppyW"))
            {
                return;
            }

            Unit = target;
            CanUseQ = true;
            DelayAction.Queue(150, () => CanUseQ = false);
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target.HasBuff("FioraW"))
            {
                return;
            }

            CanUseW = SpellConfig.W.Ready && InsideKiBurst(target);
            Unit = target;  
            DelayAction.Queue(300, () => CanUseW = false);
        }

        public static void CastR2(Obj_AI_Base target)
        {
            if (target.HasBuff(Extensions.InvulnerableList.Any().ToString()) && target.ValidActiveBuffs()
                .Where(buff => buff.Name.Contains(Extensions.InvulnerableList.Any().ToString()))
                .Any(buff => buff.Remaining > Time(target)))
            {
                return;
            }
            Items.CastTiamat();
            SpellConfig.R2.Cast(target);
        }

        private static int Time(GameObject target)
        {
            return (int)(GlobalExtension.Player.Distance(target) / (SpellConfig.R2.Speed * 1000 + SpellConfig.R2.Delay));
        }

        public static bool InsideKiBurst(GameObject target)
        {
            return GlobalExtension.Player.HasBuff("RivenFengShuiEngine")
                 ? GlobalExtension.Player.Distance(target) <= 265 + target.BoundingRadius
                 : GlobalExtension.Player.Distance(target) <= 195 + target.BoundingRadius;
        }
    }
}
