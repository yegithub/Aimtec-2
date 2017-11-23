namespace Adept_AIO.Champions.Riven.Miscellaneous
{
    using System;
    using System.Linq;
    using System.Threading;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Aimtec.SDK.Util;
    using Core;
    using SDK.Generic;
    using SDK.Unit_Extensions;
    using SDK.Usables;

    class SpellManager
    {
        private static bool _canWq;
        private static bool _canUseQ;
        private static bool _canUseW;

        private static Obj_AI_Base _unit;
        private static bool _serverPosition;

        public static float LastR;

        private static readonly string[] InvulnerableSpells =
        {
            "FioraW",
            "kindrednodeathbuff",
            "Undying Rage",
            "JudicatorIntervention"
        };

        public static void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            switch (args.SpellData.Name)
            {
                case "RivenTriCleave":
                    Extensions.LastQCastAttempt = Environment.TickCount;
                    _canUseQ = false;
                    _canWq = false;
                    _serverPosition = false;

                    Extensions.CurrentQCount++;
                    if (Extensions.CurrentQCount > 3)
                    {
                        Extensions.CurrentQCount = 1;
                    }

                    Animation.Reset();
                    break;
                case "RivenMartyr":
                    _canUseW = false;
                    break;
                case "RivenFengShuiEngine":
                    LastR = Environment.TickCount;
                    Enums.UltimateMode = UltimateMode.Second;
                    Maths.DisableAutoAttack(200);
                    break;
                case "RivenIzunaBlade":
                    Enums.UltimateMode = UltimateMode.First;
                    break;
            }
        }

        public static void OnUpdate()
        {
            switch (_unit)
            {
                case null: return;
                case Obj_AI_Hero _ when _unit.HasBuff("FioraW") || _unit.HasBuff("PoppyW"): return;
            }

            if (_canWq)
            {
                SpellConfig.W.Cast();
                Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit);
                DelayAction.Queue(600,
                    () =>
                    {
                        Global.Orbwalker.ResetAutoAttackTimer();
                        Global.Orbwalker.Attack(_unit);
                    }, new CancellationToken(false));
            }

            if (_canUseW && _unit.IsValidTarget(SpellConfig.W.Range))
            {
                _canUseW = false;

                if (Items.CanUseTiamat())
                {
                    Items.CastTiamat();
                    DelayAction.Queue(300, () => SpellConfig.W.Cast(_unit), new CancellationToken(false));
                }

                SpellConfig.W.Cast(_unit);
            }

            if (_canUseQ)
            {
                if (Extensions.DidJustAuto)
                {
                    Extensions.DidJustAuto = false;
                    Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit);
                }
                else if (_serverPosition)
                {
                    SpellConfig.Q.CastOnUnit(_unit);
                }
            }
        }

        public static void CastWq(Obj_AI_Base target)
        {
            _unit = target;
            _canWq = true;
        }

        public static void CastQ(Obj_AI_Base target, bool serverPosition = false)
        {
            _unit = target;
            _canUseQ = true;
            _serverPosition = serverPosition;
        }

        public static void CastW(Obj_AI_Base target)
        {
            _canUseW = true;
            _unit = target;
        }

        public static void CastR2(Obj_AI_Base target)
        {
            if (InvulnerableSpells.Any(target.HasBuff))
            {
                return;
            }

            if (target.IsValidAutoRange())
            {
                if (Items.CanUseTiamat())
                {
                    Items.CastTiamat();
                    DelayAction.Queue(1000, () => SpellConfig.R2.Cast(target));
                }
                else
                {
                    DelayAction.Queue(500, () => SpellConfig.R2.Cast(target));
                }
                
            }
            else
            {
                SpellConfig.R2.Cast(target);
            }
        }
    }
}