namespace Adept_AIO.Champions.Ezreal.Update.OrbwalkingEvents
{
    using System.Linq;
    using Core;
    using SDK.Junk;
    using Aimtec.SDK.Extensions;

    internal class JungleClear
    {
        public static void OnUpdate()
        {
            if (SpellConfig.Q.Ready)
            {
                if (MenuConfig.Jungle["QS"].Enabled)
                {
                    var smallMob = GameObjects.JungleSmall.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range) && x.MaxHealth >= 10);
                    if (smallMob != null)
                    {
                        SpellConfig.Q.Cast(smallMob);
                    }
                }

                if (MenuConfig.Jungle["Q"].Enabled)
                {
                    var largeMob = GameObjects.JungleLarge.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range) && x.MaxHealth >= 10);
                    if (largeMob != null)
                    {
                        SpellConfig.Q.Cast(largeMob);
                    }

                    var legendaryMob = GameObjects.JungleLegendary.FirstOrDefault(x => x.IsValidTarget(SpellConfig.Q.Range) && x.MaxHealth >= 10);
                    if (legendaryMob != null)
                    {
                        SpellConfig.Q.Cast(legendaryMob);
                    }
                }
            }

            if (SpellConfig.W.Ready && MenuConfig.Jungle["W"].Enabled && Global.Player.ManaPercent() >= MenuConfig.Jungle["W"].Value)
            {
                var ally = GameObjects.AllyHeroes.FirstOrDefault(x => x.IsValidTarget(SpellConfig.W.Range - 100));
                if (ally != null)
                {
                    SpellConfig.W.Cast(ally);
                }
            }
        }
    }
}
