using System.Drawing;
using System.Linq;
using Adept_AIO.Champions.Azir.Core;
using Adept_AIO.Champions.Azir.Update.OrbwalkingEvents;
using Adept_AIO.SDK.Junk;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Adept_AIO.Champions.Azir.Drawings
{
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

            if (MenuConfig.Drawings["Q"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellConfig.Q.Range, (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.Yellow);
            }

            if (MenuConfig.Drawings["R"].Enabled)
            {
                Render.Circle(Global.Player.Position, SpellConfig.R.Range, (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.Red);
            }

            Render.Circle(Global.Player.ServerPosition, Insec.InsecRange(),
                (uint) MenuConfig.Drawings["Segments"].Value, Color.LightGray);

            if (!SoldierHelper.Soldiers.Any() || !MenuConfig.Drawings["Soldiers"].Enabled)
            {
                return;
            }

            foreach (var soldier in SoldierHelper.Soldiers)
            {
                Render.Circle(soldier.ServerPosition, 325, (uint) MenuConfig.Drawings["Segments"].Value,
                    Color.SlateBlue);
            }

            //foreach (var soldier in SoldierHelper.Soldiers)
            //{
            //    var target = GameObjects.EnemyHeroes.FirstOrDefault(x => x.Distance(soldier) <= 325 + x.BoundingRadius && x.IsValid && !x.IsDead && x.IsFloatingHealthBarActive);
            //    if (target == null)
            //    {
            //        return;
            //    }

            //    var pred = SpellConfig.GetQPred(target);
            //    if (pred != Vector3.Zero)
            //    {
            //        Mixed.RenderArrowFromPoint(soldier.ServerPosition,
            //            soldier.ServerPosition.Extend(pred, soldier.ServerPosition.Distance(target)), 3);
            //    }
               
            //}
        }
    }
}