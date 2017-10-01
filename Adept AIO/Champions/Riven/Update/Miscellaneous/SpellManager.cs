using System.Linq;
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
        private static bool _canWq;

        private static Obj_AI_Base _unit;
        private static bool _serverPosition;

        public static float LastR;

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
                    _canWq = false;
                    _serverPosition = false;

                    DelayAction.Queue(Game.Ping / 2 + 30, Animation.Reset, new CancellationToken(false));
                    break;
                case "RivenMartyr":
                    _canUseW = false;
                    break;
                case "RivenFengShuiEngine":
                    LastR = Game.TickCount;
                    Enums.UltimateMode = UltimateMode.Second;
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

            if (_unit is Obj_AI_Hero && _unit.HasBuff("FioraW"))
            {
                return;
            }

            if (_canWq)
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
                    Extensions.DidJustAuto = false;
                    Global.Player.SpellBook.CastSpell(SpellSlot.Q, _unit);
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

        public static void CastWq(Obj_AI_Base target)
        {
            
            _unit = target;
            _canWq = true;
        }

        private static readonly string[] InvulnerableSpells = { "FioraW", "kindrednodeathbuff", "Undying Rage", "JudicatorIntervention" };

        public static void CastR2(Obj_AI_Base target)
        {
            if (target.ValidActiveBuffs().Any(buff => InvulnerableSpells.Contains(buff.Name)))
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
