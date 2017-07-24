using System;
using System.Linq;
using Adept_AIO.Champions.Riven.Core;
using Adept_AIO.Champions.Riven.Update.Miscellaneous;
using Adept_AIO.SDK.Extensions;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.TargetSelector;
using Aimtec.SDK.Util;

namespace Adept_AIO.Champions.Riven.Update.OrbwalkingEvents
{
    public class Harass
    {
        public static void OnPostAttack()
        {
            var target = TargetSelector.GetTarget(Extensions.GetRange() + 100);

            if (target == null)
            {
                return;
            }

            if (!MenuConfig.Harass[target.ChampionName].Enabled)
            {
                return;
            }

            var antiPosition = GetDashPosition(target);

            switch (Extensions.Current)
            {
                case HarassPattern.SemiCombo:
                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount == 2)
                    {
                        SpellManager.CastQ(target);
                    }

                    if (SpellConfig.W.Ready && SpellManager.InsideKiBurst(target))
                    {
                        SpellManager.CastW(target);
                    }
                    break;
                case HarassPattern.AvoidTarget:
                    if (SpellConfig.W.Ready)
                    {
                        SpellManager.CastW(target);
                    }

                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount == 2)
                    {
                        SpellManager.CastQ(target);
                    }
                    break;
                case HarassPattern.BackToTarget:
                    if (SpellConfig.W.Ready)
                    {
                        SpellManager.CastW(target);
                    }

                    if (SpellConfig.Q.Ready && Extensions.CurrentQCount >= 2)
                    {
                        SpellManager.CastQ(target);
                    }
                    break;
            }
        }

        public static void OnUpdate()
        {
            var target = TargetSelector.GetTarget(Extensions.GetRange() + 1000);

            if (target == null)
            {
                return;
            }

            if (!MenuConfig.Harass[target.ChampionName].Enabled)
            {
                return;
            }

            var qwRange = target.Distance(ObjectManager.GetLocalPlayer()) < SpellConfig.Q.Range + SpellConfig.W.Range + target.BoundingRadius;
            var antiPosition = GetDashPosition(target);

            Extensions.Current = Generate(target);

            switch (Extensions.Current)
            {
                case HarassPattern.SemiCombo:
                    #region SemiCombo
                    if (SpellConfig.Q.Ready)
                    {
                        switch (Extensions.CurrentQCount)
                        {
                            case 1: SpellManager.CastQ(target);
                                break;
                            case 3:
                                SpellConfig.Q.Cast(antiPosition);
                                break;
                        }
                    }

                    if (SpellConfig.E.Ready && Extensions.CurrentQCount == 3 && !Orbwalker.Implementation.CanAttack() && Orbwalker.Implementation.CanMove())
                    {
                        SpellConfig.E.Cast(antiPosition);
                    }
                    #endregion
                    break;
                case HarassPattern.AvoidTarget:
                    #region Away

                    if (SpellConfig.Q.Ready && SpellConfig.W.Ready && Extensions.CurrentQCount == 1 && qwRange)
                    {
                        SpellManager.CastQ(target.ServerPosition);
                    }

                    if (SpellConfig.Q.Ready && SpellConfig.E.Ready && Extensions.CurrentQCount == 3 && !Orbwalker.Implementation.CanAttack())
                    {
                        SpellConfig.E.Cast(antiPosition);
                        DelayAction.Queue(190, () => SpellConfig.Q.Cast(antiPosition));
                    }
                    else if (SpellConfig.Q.Ready && Extensions.CurrentQCount == 3)
                    {
                        DelayAction.Queue(190, () => SpellConfig.Q.Cast(antiPosition));
                    }
                    #endregion
                    break;
                case HarassPattern.BackToTarget:
                    #region Target

                    if (SpellConfig.Q.Ready && SpellConfig.W.Ready && Extensions.CurrentQCount == 1 && qwRange)
                    {
                        SpellManager.CastQ(target.ServerPosition);
                    }

                    if (SpellConfig.Q.Ready && SpellConfig.E.Ready && Extensions.CurrentQCount == 3 && !Orbwalker.Implementation.CanAttack())
                    {
                        SpellConfig.E.Cast(antiPosition);
                        DelayAction.Queue(210, () => SpellConfig.Q.Cast(target.ServerPosition));
                    }
                    #endregion
                    break;
            }
        }

        private static Vector3 GetDashPosition(Obj_AI_Base target)
        {
            switch (MenuConfig.Harass["Dodge"].Value)
            {
                case 0:
                    var turret = GameObjects.AllyTurrets.Where(x => x.IsValid).OrderBy(x => x.Distance(ObjectManager.GetLocalPlayer())).FirstOrDefault();
                    return turret != null ? turret.ServerPosition : Game.CursorPos;
                case 1:
                    return Game.CursorPos;

                case 2:
                    return ObjectManager.GetLocalPlayer().ServerPosition + (ObjectManager.GetLocalPlayer().ServerPosition - target.ServerPosition).Normalized() * 300;
            }
            return Vector3.Zero;
        }

        // Prob going to need a rework
        private static HarassPattern Generate(Obj_AI_Hero target)
        {
            if (target.IsUnderEnemyTurret() || Dangerous.Contains(target.ChampionName))
            {
                return HarassPattern.AvoidTarget;
            }
         
            if (Melee.Contains(target.ChampionName))
            {
                return HarassPattern.BackToTarget;
            }

            return SemiCombo.Contains(target.ChampionName) ? 
                   HarassPattern.SemiCombo : HarassPattern.AvoidTarget;
        }

        // Goes for CC heavy & (or) ranged enemies
        private static readonly string[] Dangerous = { "Darius", "Garen", "Galio", "Kled", "Malphite", "Maokai",
            "Trundle", "Swain", "Tahm Kench", "Ryze", "Shen", "Singed",
            "Poppy", "Pantheon", "Nasus", "Renekton", "Quinn" };

        // Melee's who are weak with CC
        private static readonly string[] Melee = { "Fiora", "Irelia", "Akali", "Udyr", "Rengar", "Jarvan IV" };

        // Mostly fighters
        private static readonly string[] SemiCombo = { "Yasuo", "LeeSin", "Xin Zhao", "Aatrox" };
    }
}
