using System.Linq;
using System.Threading;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.SDK.Junk;
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
                    _serverPosition = false;
                    Animation.Reset();
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
            if (_unit == null)
            {
                return;
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
                }
                else
                {
                    Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit);
                }
                Extensions.DidJustAuto = false;
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
            if (target.HasBuff("FioraW") || target.HasBuff("PoppyW"))
            {
                return;
            }
            
            _unit = target;
            _canUseQ = true;
            _serverPosition = serverPosition;
        }

        public static void CastW(Obj_AI_Base target)
        {
            if (target.HasBuff("FioraW"))
            {
                return;
            }

            _canUseW = InsideKiBurst(target.ServerPosition, target.BoundingRadius);
            _unit = target;  
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
