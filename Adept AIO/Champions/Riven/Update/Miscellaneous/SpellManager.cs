using System;
using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Junk;
using Adept_AIO.SDK.Methods;
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
        private static Obj_AI_Base _unit;
        private static Vector3 _position;

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
                    Animation.Reset();
                    break;
                case "RivenMartyr":
                    _canUseW = false;
                    Global.Orbwalker.ResetAutoAttackTimer();
                    break;
                case "RivenFengShuiEngine":
                    Enums.UltimateMode = UltimateMode.Second;
                    Global.Orbwalker.ResetAutoAttackTimer();
                    break;
                case "RivenIzunaBlade":
                    Enums.UltimateMode = UltimateMode.First;
                    Global.Orbwalker.ResetAutoAttackTimer();
                    break;
            }
        }

        public static void OnUpdate()
        {
            if (_canUseQ && Extensions.DidJustAuto)
            {
                if (_unit == null && !_position.IsZero)
                {
                    Global.Player.SpellBook.CastSpell(SpellSlot.Q, _position);
                    _position = Vector3.Zero;
                }
                else if (_unit != null)
                {
                    if (Extensions.CurrentQCount == 3 && Items.CanUseTiamat())
                    {
                        Items.CastTiamat(false);
                        DelayAction.Queue(250 + Game.Ping / 2,
                            () => Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit), new CancellationToken(false));
                    }
                    else
                    {
                        Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit);
                    }
                }


                Extensions.DidJustAuto = false;
            }

            if (!_canUseW || _unit == null)
            {
                return;
            }

            SpellConfig.W.Cast(_unit);
        }

        public static void CastQ(Obj_AI_Base target)
        {
            if (target.HasBuff("FioraW") || target.HasBuff("PoppyW"))
            {
                return;
            }

            _unit = target;
            _canUseQ = true;
        }

        public static void CastQ(Vector3 pos)
        {
            _position = pos;
            _canUseQ = true;
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target.HasBuff("FioraW"))
            {
                return;
            }

            _canUseW = InsideKiBurst(target);
            _unit = target;  
        }

        public static void CastR2(Obj_AI_Base target)
        {
            if (target.ValidActiveBuffs()
                .Where(buff => Extensions.InvulnerableList.Contains(buff.Name))
                .Any(buff => buff.Remaining > Time(target)))
            {
                return;
            }

            SpellConfig.R2.Cast(target);

            if (target.Distance(Global.Player) <= Global.Player.AttackRange)
            {
                Items.CastTiamat();
            }
        }

        private static int Time(GameObject target)
        {
            return (int)(Global.Player.Distance(target) / (SpellConfig.R2.Speed * 1000 + SpellConfig.R2.Delay));
        }

        public static bool InsideKiBurst(GameObject target)
        {
            return Global.Player.HasBuff("RivenFengShuiEngine")
                 ? Global.Player.Distance(target) <= 200 + target.BoundingRadius
                 : Global.Player.Distance(target) <= 135 + target.BoundingRadius;
        }

        public static bool InsideKiBurst(Vector3 position)
        {
            return Global.Player.HasBuff("RivenFengShuiEngine")
                ? Global.Player.Distance(position) <= 265
                : Global.Player.Distance(position) <= 195;
        }
    }
}
