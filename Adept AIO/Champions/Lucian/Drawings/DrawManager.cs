namespace Adept_AIO.Champions.Lucian.Drawings
{
    using System.Drawing;
    using System.Linq;
    using Aimtec;
    using Aimtec.SDK.Extensions;
    using Core;
    using SDK.Unit_Extensions;

    class DrawManager
    {
        public static void OnPresent()
        {
            if (Global.Player.IsDead || !MenuConfig.Drawings["Dmg"].Enabled)
            {
                return;
            }

            foreach (var target in GameObjects.EnemyHeroes.Where(x =>
                !x.IsDead && x.IsFloatingHealthBarActive && x.IsVisible))
            {
                var damage = Dmg.Damage(target);

                Global.DamageIndicator.Unit = target;
                Global.DamageIndicator.DrawDmg((float) damage, Color.FromArgb(153, 12, 177, 28));
            }
        }

        public static void OnRender()
        {
            if (Global.Player.IsDead)
            {
                return;
            }

            if (MenuConfig.Drawings["Debug"].Enabled)
            {
                var target = Global.Orbwalker.GetOrbwalkingTarget() as Obj_AI_Base;

                if (target != null && target.IsValidTarget() && target.IsHero)
                {
                    if (SpellManager.Q.Ready)
                    {
                        var qRectangle = SpellManager.GetQRectangle(target);
                        qRectangle?.Draw(Color.Cyan);
                    }

                    if (SpellManager.R.Ready)
                    {
                        var rRectangle = SpellManager.GetRRectangle(target);
                        rRectangle?.Draw(Color.Crimson);
                    }
                }
            }

            if (SpellManager.Q.Ready)
            {
                if (MenuConfig.Drawings["Q"].Enabled)
                {
                    Render.Circle(Global.Player.Position,
                        SpellManager.Q.Range,
                        (uint) MenuConfig.Drawings["Segments"].Value,
                        Color.Cyan);
                }

                if (MenuConfig.Drawings["Extended"].Enabled)
                {
                    Render.Circle(Global.Player.Position,
                        SpellManager.Q.Range + 400,
                        (uint) MenuConfig.Drawings["Segments"].Value,
                        Color.Crimson);
                }
            }
        }
    }
}