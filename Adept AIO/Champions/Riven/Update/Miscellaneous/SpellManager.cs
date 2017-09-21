﻿using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Unit_Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.Miscellaneous
{
    internal class SpellManager
    {
        private static bool _canUseQ;
        private static bool _canUseW;
        private static bool _canWQ;

        private static Obj_AI_Base _unit;
        private static bool _serverPosition;

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }
        
            switch (args.SpellData.Name)
            {
                case "RivenTriCleave":
                    Extensions.LastQCastAttempt = Game.TickCount;
                    _canUseQ = false;
                    _canWQ = false;
                    _serverPosition = false;
                  
                    break;
                case "RivenMartyr":
                    _canUseW = false;
                    break;
                case "RivenFengShuiEngine":
                    Enums.UltimateMode = UltimateMode.Second;
                    Global.Orbwalker.ResetAutoAttackTimer();
                    break;
                case "RivenIzunaBlade":
                    Enums.UltimateMode = UltimateMode.First;
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (_unit == null || _unit.HasBuff("FioraW") || _unit.HasBuff("FioraW") || _unit.HasBuff("PoppyW"))
            {
                return;
            }

            if (_canWQ)
            {
                SpellConfig.W.Cast();
                Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit);
            }

            if (_serverPosition && _canUseQ)
            {
                SpellConfig.Q.CastOnUnit(_unit);
            }

            if (_canUseQ && Extensions.DidJustAuto)
            {
                if (Items.CanUseTiamat() && Global.Player.IsFacingUnit(_unit) && Extensions.CurrentQCount != 2)
                {
                    Items.CastTiamat(false);
                    DelayAction.Queue(230 + Game.Ping / 2,
                        () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit), new CancellationToken(false));
                    Extensions.DidJustAuto = false;
                }
                else
                {
                    Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit);
                    Extensions.DidJustAuto = false;
                }
            }

            if (!_canUseW)
            {
                return;
            }

            Items.CastTiamat();
            SpellConfig.W.Cast(_unit);
            _canUseW = false;
        }

        public static void CastQ(Obj_AI_Base target, bool serverPosition = false)
        {
            _unit = target;
            _canUseQ = true;
            _serverPosition = serverPosition;
        }

        public static void CastW(Obj_AI_Base target)
        {
            _canUseW = InsideKiBurst(target.ServerPosition, target.BoundingRadius);
            _unit = target;  
        }

        public static void CastWQ(Obj_AI_Base target)
        {
            _unit = target;
            _canWQ = true;
        }

        private static readonly string[] InvulnerableList = { "FioraW", "kindrednodeathbuff", "Undying Rage", "JudicatorIntervention" };

        public static void CastR2(Obj_AI_Base target)
        {
            if (target.ValidActiveBuffs().Any(buff => InvulnerableList.Contains(buff.Name)))
            {
                return;
            }

            SpellConfig.R2.Cast(target);

            if (target.IsValidTarget(Global.Player.AttackRange))
            {
                Items.CastTiamat();
            }
        }

        public static bool InsideKiBurst(Vector3 position, float extra = 0)
        {
            return Global.Player.HasBuff("RivenFengShuiEngine")
                 ? Global.Player.Distance(position) <= 135 + extra
                 : Global.Player.Distance(position) <= 125 + extra;
        }
    }
}
