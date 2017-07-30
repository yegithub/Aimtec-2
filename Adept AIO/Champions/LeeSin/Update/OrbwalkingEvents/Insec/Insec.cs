using System;
using System.Linq;
using Adept_AIO.Champions.LeeSin.Core.Spells;
using Adept_AIO.Champions.LeeSin.Update.Ward_Manager;
using Adept_AIO.SDK.Extensions;
using Adept_AIO.SDK.Usables;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.TargetSelector;

namespace Adept_AIO.Champions.LeeSin.Update.OrbwalkingEvents.Insec
{
    internal class Insec : IInsec
    {
        public bool KickFlashEnabled { get; set; }
        public bool Enabled { get; set; }
        public bool QLast { get; set; }
        public bool ObjectEnabled { get; set; }
        public int InsecPositionValue { get; set; }
        public int InsecKickValue { get; set; }

        private readonly IWardTracker _wardTracker;
        private readonly IWardManager _wardManager;
        private readonly ISpellConfig SpellConfig;
     
        public Insec(IWardTracker wardTracker, IWardManager wardManager, ISpellConfig spellConfig)
        {
            _wardTracker = wardTracker;
            _wardManager = wardManager;
            SpellConfig = spellConfig;
        }

        private bool WardFlash;
        private float LastQTime;
        private Obj_AI_Hero target => GlobalExtension.TargetSelector.GetSelectedTarget();

        private Vector3 InsecPosition => target.ServerPosition + (target.ServerPosition - GetTargetEndPosition()).Normalized() *
                                         DistanceBehindTarget();

        private float DistanceBehindTarget()
        {
            return Math.Min((GlobalExtension.Player.BoundingRadius + target.BoundingRadius + 50) * 1.275f, SpellConfig.R.Range);
        }

        public void Kick()
        {
            if (target == null || !KickFlashEnabled)
            {
                return;
            }

            if (SpellConfig.W.Ready &&
                SpellConfig.IsFirst(SpellConfig.W) &&
                _wardTracker.IsWardReady && target.Distance(GlobalExtension.Player) > SpellConfig.R.Range + 200 && target.Distance(GlobalExtension.Player) < 1050)
            {
                _wardManager.WardJump(InsecPosition, false);
            }

            if (!target.IsValidTarget(SpellConfig.R.Range) || !SpellConfig.R.Ready)
            {
                return;
            }
            SpellConfig.R.CastOnUnit(target);
        }

        public void OnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (sender == null || !sender.IsMe)
            {
                return;
            }

            if (args.SpellSlot == SpellSlot.Q && args.SpellData.Name.ToLower().Contains("one"))
            {
                LastQTime = Environment.TickCount;
            }

            if (!Enabled && InsecKickValue != 1 || SummonerSpells.Flash == null || target == null || args.SpellSlot != SpellSlot.R ||
                !SummonerSpells.Flash.Ready)
            {
                return;
            }

            if (KickFlashEnabled)
            {
                SummonerSpells.Flash.Cast(InsecPosition);
            }
            if (_wardTracker.IsWardReady)
            {
                return;
            }

            if (GlobalExtension.Player.Distance(InsecPosition) <= 100 ||
                Environment.TickCount - _wardTracker.LastWardCreated < 1500 &&
                !WardFlash)
            {
                return;
            }

            Console.WriteLine("FLASHING");
            SummonerSpells.Flash.Cast(InsecPosition);
        }

        private int GetGapcloseDistance()
        {
            var count = 0;

            if (_wardManager.IsWardReady() && SpellConfig.W.Ready)
            {
                count += 600;
            }

            if(SummonerSpells.Flash != null && SummonerSpells.Flash.Ready)
            {
                count += 425;
            }

            return count;
        }

        public void OnKeyPressed()
        {
            if (target == null || !Enabled)
            {
                SpellConfig.InsecPosition = Vector3.Zero;
                return;
            }

            SpellConfig.InsecPosition = InsecPosition;

            if (SpellConfig.Q.Ready)
            {
                if (SpellConfig.IsQ2())
                {
                    SpellConfig.Q.Cast();
                    LastQTime = Environment.TickCount;
                }
                else if (target.IsValidTarget(SpellConfig.Q.Range))
                {
                    SpellConfig.Q.Cast(target);
                    LastQTime = Environment.TickCount;
                }
                else if (ObjectEnabled) 
                {
                    var minion = GameObjects.EnemyMinions.Where(x => x.Distance(InsecPosition) < 700 &&
                                                                     GlobalExtension.Player.Distance(x) <= SpellConfig.Q.Range &&
                                                                     x.Health > GlobalExtension.Player.GetSpellDamage(x, SpellSlot.Q))
                        .OrderBy(x => x.Distance(InsecPosition))
                        .FirstOrDefault(x => x.Distance(InsecPosition) < GlobalExtension.Player.Distance(InsecPosition));

                    if (minion == null)
                    {
                        return;
                    }

                    if (minion.Distance(InsecPosition) <= 500)
                    {
                        WardFlash = false;
                    }
                    SpellConfig.Q.Cast(minion.ServerPosition);
                    LastQTime = Environment.TickCount;
                }
            }

            if (SpellConfig.W.Ready && SpellConfig.IsFirst(SpellConfig.W))
            {
                if (InsecPosition.Distance(GlobalExtension.Player) <= 600)
                {
                    WardFlash = false;
                    _wardManager.WardJump(InsecPosition, false);
                }
                else if (GetGapcloseDistance() <= 500 + 425)
                {
                    if (Environment.TickCount - LastQTime > 650)
                    {
                        return;
                    }

                    WardFlash = true;
                    _wardManager.WardJump(InsecPosition, true);
                }
            }

            if (!SpellConfig.R.Ready)
            {
                return;
            }

            if (InsecKickValue == 0 &&
                SummonerSpells.Flash != null &&
                SummonerSpells.Flash.Ready && 
                InsecPosition.Distance(GlobalExtension.Player) <= 425 && 
                !(Environment.TickCount - _wardTracker.LastWardCreated <= 1500 && !WardFlash))
            {
                SummonerSpells.Flash.Cast(InsecPosition);
            }
            else if (!target.IsValidTarget(SpellConfig.R.Range) ||
                Environment.TickCount - _wardTracker.LastWardCreated > 1000 || Environment.TickCount - _wardTracker.LastWardCreated < 500 ||
                GlobalExtension.Player.Distance(InsecPosition) >= 425)
            {
                return;
            }
          
            SpellConfig.R.CastOnUnit(target);
        }

        private Vector3 GetTargetEndPosition()
        {
            switch (InsecPositionValue)
            {
                case 0:
                    var ally = GameObjects.AllyHeroes.FirstOrDefault();
                    var turret = GameObjects.AllyTurrets.Where(x => x.IsValid).OrderBy(x => x.Distance(GlobalExtension.Player)).FirstOrDefault();
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